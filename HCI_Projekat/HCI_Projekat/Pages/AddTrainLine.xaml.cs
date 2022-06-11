using HCI_Projekat.Model;
using HCI_Projekat.touring;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ThinkSharp.FeatureTouring.Navigation;

namespace HCI_Projekat.Pages
{
    /// <summary>
    /// Interaction logic for AddTrainLine.xaml
    /// </summary>
    public partial class AddTrainLine : Page
    {
        public List<StationLineDto> stationsDto { get; set; }
        public Data dataBase { get; set; }

        Point startPoint = new Point();
        public MapPolyline polyline { get; set; }
        public TrainLine trainLine { get; set; }


        public bool tour = false;

        public AddTrainLine(Data dataBase,TrainLine trainLine, bool tour=false)
        {
            InitializeComponent();
            this.dataBase = dataBase;
            DataContext = this;


            if(trainLine == null)
            {
                int index = this.dataBase.trainLines.Max(x => x.id);
                this.trainLine = new TrainLine(index + 1, 0); //null
                this.dataBase.trainLines.Add(this.trainLine);
            }
            else
            {
                this.trainLine = trainLine;
            }

            if(this.trainLine.stations.Count >= 2)
            {
                pin.Visibility = Visibility.Visible;
                pin_start.Visibility = Visibility.Hidden;
                pin_end.Visibility = Visibility.Hidden;
            }else if (this.trainLine.stations.Count == 1 && this.trainLine.from != null)
            {
                pin_start.Visibility = Visibility.Hidden;
            }else if (this.trainLine.stations.Count == 1 && this.trainLine.to != null)
            {
                pin_end.Visibility = Visibility.Hidden;
            }


            this.stationsDto = new List<StationLineDto>();

            myMap.Children.Clear();

            this.polyline = new MapPolyline();
            this.polyline.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Blue);
            this.polyline.StrokeThickness = 5;
            this.polyline.Opacity = 0.7;
            this.polyline.Locations = new LocationCollection();

            addPinToMap();

            myMap.Children.Add(this.polyline);

            this.tour = tour;
            if (tour)
            {
                bt_save.IsEnabled = false;
                bt_cancle.IsEnabled = false;
                var navigator = FeatureTour.GetNavigator();
                navigator.OnStepLeft(ElementID.Pin).Execute(s => bt_save.IsEnabled = true);
                navigator.OnStepEntering(ElementID.PinStart).Execute(s => pin_end.Visibility = Visibility.Hidden);
            }

        }

        public void addPinToMap()
        {
            if (this.trainLine.stations.Count>0 && (this.trainLine.from != this.trainLine.stations.First()))
            {
                this.trainLine.stations = this.trainLine.stations.OrderBy(x => x.latitude).ToList();
            }
            for (int i = 0; i < this.trainLine.stations.Count; i++)
            {
                Pushpin p = new Pushpin();

                if (this.trainLine.stations[i] == this.trainLine.from)
                {
                    p.Background = new SolidColorBrush(Colors.Red);
                    p.Content = "start";
                    StationLineDto sldto = new StationLineDto(trainLine.stations[i].name, "/Images/start.png");
                    this.stationsDto.Add(sldto);
                }
                else if (this.trainLine.stations[i] == this.trainLine.to)
                {
                    p.Background = new SolidColorBrush(Colors.Red);
                    p.Content = "end";
                    StationLineDto sldto = new StationLineDto(trainLine.stations[i].name, "/Images/end.png");
                    this.stationsDto.Add(sldto);
                }
                else
                {
                    p.Background = new SolidColorBrush(Colors.Blue);
                    this.stationsDto.Add(new StationLineDto(trainLine.stations[i].name, "/Images/blue.png"));
                }
                Location loc = new Location(trainLine.stations[i].latitude, trainLine.stations[i].longitude);
                p.Location = loc;
                this.polyline.Locations.Add(loc);
                myMap.Children.Add(p);
            }          

        }

        public void StartTour()
        {
            var navigator = FeatureTour.GetNavigator();

            navigator.OnStepEntered(ElementID.PinStart).Execute(s => start.IsEnabled = true) ;
            navigator.OnStepEntered(ElementID.PinEnd).Execute(s => endd.Focus());
            navigator.OnStepEntered(ElementID.Pin).Execute(s => pin.Focus());

            navigator.OnStepEntered(ElementID.AddTrainLineButtonSave).Execute(s => bt_save.Focus());
            TourStarter.StartAddTrainLineTour();
        }

        private void pin_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;
            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                DragDrop.DoDragDrop(pin, pin, DragDropEffects.Move);
            }
        }

        private void pin_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        private void myMap_Drop(object sender, DragEventArgs e)
        {
            var source = e.Data.GetData("System.Windows.Controls.Image") as Image;
            string name = source.Name;


            Point mousePosition = e.GetPosition(myMap);
            Location pinLocation = myMap.ViewportPointToLocation(mousePosition);

            StationDialog d = new StationDialog(this.dataBase, this.trainLine, pinLocation, name,"add", tour);
            d.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            d.ShowDialog();

            
            var navigator = FeatureTour.GetNavigator();
            if(name == "pin_start")
            {
                navigator.IfCurrentStepEquals(ElementID.PinStart).GoNext();
              
            }
            else if(name == "pin_end")
            {
                navigator.IfCurrentStepEquals(ElementID.PinEnd).GoNext();
               
            }
            else
            {
                navigator.IfCurrentStepEquals(ElementID.Pin).GoNext();
            }

        }


        private void pin_start_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        private void pin_end_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        private void pin_start_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;
            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                DragDrop.DoDragDrop(pin_start, pin_start, DragDropEffects.Move);
            }
        }

        private void pin_end_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;
            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                DragDrop.DoDragDrop(pin_end, pin_end, DragDropEffects.Move);
            }
        }

        private void bt_cancle_Click(object sender, RoutedEventArgs e)
        {
            var res = MessageBox.Show("Are you sure you want to quit?", "Check", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(res == MessageBoxResult.Yes)
            {
                this.dataBase.trainLines.Remove(this.trainLine);
                MainWindow window = (MainWindow)Window.GetWindow(this);
                TrainLineCRUD tc = new TrainLineCRUD(window.dataBase);
                window.Content = tc;
            }
            
        }

        private void bt_save_Click(object sender, RoutedEventArgs e)
        {
            if(this.trainLine.stations.Count < 2)
            {
                MessageBox.Show("You must add at least two stations!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("Successfully added train line!", "Success",MessageBoxButton.OK,MessageBoxImage.Information);
            MainWindow window = (MainWindow)Window.GetWindow(this);
            TrainLineCRUD tc = new TrainLineCRUD(window.dataBase);
            window.Content = tc;

            var navigator = FeatureTour.GetNavigator();
            navigator.IfCurrentStepEquals(ElementID.AddTrainLineButtonSave).GoNext();

        }
    }
}
