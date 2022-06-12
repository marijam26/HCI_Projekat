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
    /// Interaction logic for RailwayMap.xaml
    /// </summary>
    public partial class RailwayMap : Page
    {
        public Data dataBase { get; set; }
        public List<StationLineDto> stationsDto { get; set; }
        public TrainLine TrainLine { get; set; }
        public int counter { get; set; }
        public string parentPage { get; set; }

        public RailwayMap(Data dataBase,string parentPage)
        {
            InitializeComponent();
            this.dataBase = dataBase;
            this.parentPage = parentPage;
            DataContext = this;
            this.counter = 0;
            this.stationsDto = new List<StationLineDto>();
            List<string> cbItems = new List<string>();
            foreach(TrainLine tl in dataBase.trainLines)
            {
                if(tl.from != null)
                {
                    string text = tl.from.name + " - " + tl.to.name;
                    cbItems.Add(text);
                }
                
            }
            cb_lines.ItemsSource = cbItems;
        }

        private void bt_back_Click(object sender, RoutedEventArgs e)
        {
            if(parentPage == "client")
            {
                ClientHomepage window = (ClientHomepage)Window.GetWindow(this);
                WelcomeClient ch = new WelcomeClient();
                window.clientHomepage.Navigate(ch);
            }
            else
            {
                ManagerHomepage window = (ManagerHomepage)Window.GetWindow(this);
                WelcomeManager ch = new WelcomeManager();
                window.managerHomepage.Navigate(ch);
            }
            
        }

        private void cb_lines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cb_lines.SelectedIndex == -1)
            {
                return;
            }

            if(counter == 7)
            {
                counter = 0;
            }
            string selectedLine = cb_lines.SelectedItem.ToString();
            string selectedFrom = selectedLine.Split('-')[0].Trim();
            string selectedTo = selectedLine.Split('-')[1].Trim();


            this.TrainLine = this.dataBase.trainLines.Where(x => x.from.name == selectedFrom && x.to.name == selectedTo).FirstOrDefault();
            this.stationsDto.Clear();
            //myMap.Children.Clear();

            SolidColorBrush[] colors = { Brushes.Blue, Brushes.Red, Brushes.DarkCyan, Brushes.DarkGoldenrod, Brushes.Gray, Brushes.HotPink,Brushes.Orange };
            MapPolyline polyline = new MapPolyline();
            polyline.Stroke = colors[counter];
            polyline.StrokeThickness = 5;
            polyline.Opacity = 0.7;
            polyline.Locations = new LocationCollection();


            addPinToMap(polyline);
            dg_stations.ItemsSource = null;
            dg_stations.ItemsSource = this.stationsDto;

            myMap.Children.Add(polyline);
            counter++;
        }

        public void addPinToMap(MapPolyline polyline)
        {
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
                polyline.Locations.Add(loc);
                myMap.Children.Add(p);
            }
        }

        private void bt_clear_Click(object sender, RoutedEventArgs e)
        {
            myMap.Children.Clear();
            cb_lines.SelectedIndex = -1;
            dg_stations.ItemsSource = null;
        }
    }
}
