using HCI_Projekat.Model;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HCI_Projekat.Pages
{
    /// <summary>
    /// Interaction logic for EditTrainLine.xaml
    /// </summary>
    /// 

    public partial class EditTrainLine : Page
    {
        Point startPoint = new Point();
        public Data dataBase { get; set; }
        public TrainLine TrainLine { get; set; }
        public MapPolyline polyline { get; set; }
        public List<StationLineDto> stationsDto { get; set; }
        public TrainLine backupTrainLine { get; set; }

        public EditTrainLine(Data dataBase, TrainLine trainLine)
        {
            InitializeComponent();
            this.dataBase = dataBase;
            this.TrainLine = trainLine;
            DataContext = this;
            this.stationsDto = new List<StationLineDto>();

           
            
            myMap.Children.Clear();

            this.polyline = new MapPolyline();
            this.polyline.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Blue);
            this.polyline.StrokeThickness = 5;
            this.polyline.Opacity = 0.7;
            this.polyline.Locations = new LocationCollection();


            addPinToMap();

            myMap.Children.Add(this.polyline);
            

        }

        public void addPinToMap()
        {
            if(this.TrainLine.from != this.TrainLine.stations.First())
            {
                this.TrainLine.stations = this.TrainLine.stations.OrderBy(x => x.latitude).ToList();
            }
            for (int i = 0; i < this.TrainLine.stations.Count; i++)
            {
                Pushpin p = new Pushpin();

                if (this.TrainLine.stations[i] == this.TrainLine.from)
                {
                    p.Background = new SolidColorBrush(Colors.Red);
                    p.Content = "start";
                    StationLineDto sldto = new StationLineDto(this.TrainLine.stations[i].name, "/Images/start.png");
                    this.stationsDto.Add(sldto);
                }
                else if (this.TrainLine.stations[i] == this.TrainLine.to)
                {
                    p.Background = new SolidColorBrush(Colors.Red);
                    p.Content = "end";
                    StationLineDto sldto = new StationLineDto(this.TrainLine.stations[i].name, "/Images/end.png");
                    this.stationsDto.Add(sldto);
                }
                else
                {
                    p.Background = new SolidColorBrush(Colors.Blue);
                    this.stationsDto.Add(new StationLineDto(this.TrainLine.stations[i].name, "/Images/blue.png"));
                }
                Location loc = new Location(this.TrainLine.stations[i].latitude, this.TrainLine.stations[i].longitude);
                p.Location = loc;
                this.polyline.Locations.Add(loc);
                myMap.Children.Add(p);
            }
        }

        private void pin_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;
            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                DragDrop.DoDragDrop(pin,pin,DragDropEffects.Move);
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

            StationDialog d = new StationDialog(this.dataBase,this.TrainLine,pinLocation,name,"edit");
            d.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            d.ShowDialog();      

        }

        private void bt_cancle_Click(object sender, RoutedEventArgs e)
        {
            var res = MessageBox.Show("Are you sure you want to quit?", "Check", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                this.dataBase.trainLines.Remove(this.TrainLine);
                this.dataBase.trainLines.Add(this.dataBase.backupTrainLine);
                ManagerHomepage window = (ManagerHomepage)Window.GetWindow(this);
                TrainLineCRUD tc = new TrainLineCRUD(window.dataBase);
                window.managerHomepage.Navigate(tc);
            }
        }

        private void bt_save_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Successfully changed train line!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            ManagerHomepage window = (ManagerHomepage)Window.GetWindow(this);
            TrainLineCRUD tc = new TrainLineCRUD(window.dataBase);
            window.managerHomepage.Navigate(tc);
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
    }

    public class StationLineDto
    {
        public string name { get; set; }

        public string path { get; set; }
        public StationLineDto(string name, string path)
        {
            this.name = name;
            this.path = path;
        }
    }
}
