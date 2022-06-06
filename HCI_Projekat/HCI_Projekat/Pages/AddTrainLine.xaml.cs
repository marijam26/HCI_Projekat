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
    /// Interaction logic for AddTrainLine.xaml
    /// </summary>
    public partial class AddTrainLine : Page
    {
        Point startPoint = new Point();
        public Data dataBase { get; set; }
        public TrainLine TrainLine { get; set; }
        public MapPolyline polyline { get; set; }

        public AddTrainLine(Data dataBase, TrainLine trainLine)
        {
            InitializeComponent();
            this.dataBase = dataBase;
            this.TrainLine = trainLine;
            /*
            var layerp = new MapLayer();
            layerp.Name = "PushpinLayer";
            myMap.Children.Add(layerp);

            var location = new Location(trainLine.stations[0].latitude, trainLine.stations[0].longitude);
            var p = new Pushpin();
            p.Location = location;
            p.Name = "bal";
            p.Background = new SolidColorBrush(Colors.Red);
            layerp.Children.Add(p);
            */
            this.polyline = new MapPolyline();
            this.polyline.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Blue);
            this.polyline.StrokeThickness = 5;
            this.polyline.Opacity = 0.7;
            this.polyline.Locations = new LocationCollection();


            for (int i = 0; i < trainLine.stations.Count; i++)
            {
                Pushpin p = new Pushpin();
                p.Background = new SolidColorBrush(Colors.Blue);
                if (i == 0 || i == trainLine.stations.Count-1)
                {
                    p.Background = new SolidColorBrush(Colors.Red);
                    p.Content = i==0?"start":"end";
                }
                Location loc = new Location(trainLine.stations[i].latitude, trainLine.stations[i].longitude);
                p.Location = loc;
                this.polyline.Locations.Add(loc);
                myMap.Children.Add(p);
            }

            myMap.Children.Add(this.polyline);

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

        private void canvas_DragOver(object sender, DragEventArgs e)
        {
            Point dropPos = e.GetPosition(canvas);

            Canvas.SetLeft(pin, dropPos.X);
            Canvas.SetTop(pin, dropPos.Y);

        }

        private void pin_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        private void myMap_DragEnter(object sender, DragEventArgs e)
        {

        }

        private void myMap_Drop(object sender, DragEventArgs e)
        {
            e.Handled = true;

            Point mousePosition = e.GetPosition(myMap);

            Location pinLocation = myMap.ViewportPointToLocation(mousePosition);
            var p = new Pushpin();
            p.Location = pinLocation;
            p.Background = new SolidColorBrush(Colors.Blue);
            Location end = this.polyline.Locations.Last();
            this.polyline.Locations.Remove(end);

            this.polyline.Locations.Add(pinLocation);
            this.polyline.Locations.Add(end);


            //this.polyline.Locations.OrderByDescending(x => x.Latitude);
            myMap.Children.Remove(this.polyline);
            myMap.Children.Add(this.polyline);
            myMap.Children.Add(p);


        }

        public void sortLocations()
        {
            LocationCollection n = new LocationCollection();
            Location first = this.polyline.Locations.First();
            Location second = this.polyline.Locations[1];
            Location end = this.polyline.Locations.Last();
            n.Add(first);
            double min = Math.Abs(first.Latitude - second.Latitude);
            for(int i = 2; i < this.polyline.Locations.Count-1; i++)
            {
                double min2 = Math.Abs(this.polyline.Locations[i].Latitude - first.Latitude);
                if (min2 < min)
                {
                    min = min2;

                }
            }

        }

    }
}
