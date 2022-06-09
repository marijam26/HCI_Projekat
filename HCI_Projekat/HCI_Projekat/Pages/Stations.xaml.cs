using HCI_Projekat.Model;
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
    /// Interaction logic for Stations.xaml
    /// </summary>
    public partial class Stations : Page
    {
        public TrainLine tl { get; set; }
        public List<stationDto> stationsList { get; set; }
        public Data dataBase { get; set; }


        public Stations(TrainLine trainLine,Data database)
        {
            InitializeComponent();
            this.tl = trainLine;
            this.dataBase = database;
            DataContext = this;
            this.stationsList = new List<stationDto>();
            for(int i = 0; i < tl.stations.Count-1; i++)
            {
                this.stationsList.Add(new stationDto(tl.stations[i].name, tl.stations[i+1].name, tl.time[i]));
            }
        }

        public class stationDto
        {
            public string from { get; set; }

            public string to { get; set; }
            public int time { get; set; }
            public stationDto() { }

            public stationDto(string from,string to, int time)
            {
                this.from = from;
                this.to = to;
                this.time = time;
            }
        }

        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            TrainLineCRUD s = new TrainLineCRUD(this.dataBase);
            MainWindow window = (MainWindow)Window.GetWindow(this);
            window.Content = s;
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            int selectedCells = station_table.SelectedCells.Count();
            if (selectedCells == 0)
            {
                MessageBox.Show("Must select station.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if(this.stationsList.Count == 1)
            {
                MessageBox.Show("You cannot delete stations.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var Result = MessageBox.Show("Do you want to delete station?", "Check", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Result == MessageBoxResult.Yes)
            {
                stationDto sdto = (stationDto)station_table.SelectedItem;
                this.stationsList.Remove(sdto);
                Station s = this.tl.stations.Where(x => x.name == sdto.from).First();
                if(s.id == this.tl.from.id)
                {
                    this.tl.from = this.tl.stations[1];
                }
                this.tl.stations.Remove(s);
                station_table.ItemsSource = null;
                station_table.ItemsSource = this.stationsList;
            }
        }
    }
}
