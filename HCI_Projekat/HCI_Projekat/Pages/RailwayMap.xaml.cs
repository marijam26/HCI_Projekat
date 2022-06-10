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
        public MapPolyline polyline { get; set; }
        public List<StationLineDto> stationsDto { get; set; }
        public TrainLine TrainLine { get; set; }

        public RailwayMap(Data dataBase)
        {
            InitializeComponent();
            this.dataBase = dataBase;
            DataContext = this;
            this.stationsDto = new List<StationLineDto>();
            List<string> cbItems = new List<string>();
            foreach(TrainLine tl in dataBase.trainLines)
            {
                string text = tl.from.name + " - " + tl.to.name;
                cbItems.Add(text);
            }
            cb_lines.ItemsSource = cbItems;
        }

        private void bt_back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Window.GetWindow(this);
            ClientHomepage ch = new ClientHomepage(this.dataBase,this.dataBase.currentUser);
            window.Content = ch;
        }

        private void cb_lines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            string selectedLine = cb_lines.SelectedItem.ToString();
            string selectedFrom = selectedLine.Split('-')[0].Trim();
            string selectedTo = selectedLine.Split('-')[1].Trim();


            this.TrainLine = this.dataBase.trainLines.Where(x => x.from.name == selectedFrom && x.to.name == selectedTo).FirstOrDefault();
            this.stationsDto.Clear();
            myMap.Children.Clear();

            this.polyline = new MapPolyline();
            this.polyline.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Blue);
            this.polyline.StrokeThickness = 5;
            this.polyline.Opacity = 0.7;
            this.polyline.Locations = new LocationCollection();


            addPinToMap();
            dg_stations.ItemsSource = null;
            dg_stations.ItemsSource = this.stationsDto;

            myMap.Children.Add(this.polyline);
        }

        public void addPinToMap()
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
                this.polyline.Locations.Add(loc);
                myMap.Children.Add(p);
            }
        }
    }
}
