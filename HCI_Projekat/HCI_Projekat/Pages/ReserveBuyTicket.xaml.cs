using HCI_Projekat.Model;
using System;
using System.Collections;
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
    /// Interaction logic for ReserveBuyTicket.xaml
    /// </summary>
    public partial class ReserveBuyTicket : Page
    {
        public Data dataBase { get; set; }

        public ReserveBuyTicket(Data database)
        {
            InitializeComponent();
            this.dataBase = database;
            fromPlace.ItemsSource = getStations();
            toPlace.ItemsSource = getStations();
            btn_chooseSeat.IsEnabled = false;
        }

        private IEnumerable getStations()
        {
            List<string> stations = new List<string>();
            foreach (TrainLine line in dataBase.trainLines) {
                foreach (Station place in line.stations) {
                    if (!stations.Contains(place.name.ToLower())) {
                        stations.Add(place.name.ToLower());
                    }
                }
            }
            return stations;
        }

        private void timetable_table_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {



        }

        
      private void btn_chooseSeat_Click(object sender, RoutedEventArgs e)
        {
            int selectedCells = timetable_table.SelectedCells.Count();
            if (selectedCells == 0)
            {
                MessageBox.Show("Must select timetable.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            TimetableDTO t = (TimetableDTO)timetable_table.SelectedItem;

            MainWindow window = (MainWindow)Window.GetWindow(this);
            ChooseSeat r = new ChooseSeat(this.dataBase, t);
            window.Content = r;

        }
        
        
    private void searchStations(object sender, RoutedEventArgs e)
        {
            String start = fromPlace.Text.Trim().ToLower();
            String end = toPlace.Text.Trim().ToLower();
            DateTime? startDate = startDatePick.SelectedDate;
            if (startDate == null) { 
                MessageBox.Show("Must enter date of departure.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            List<TimetableDTO> timetables = new List<TimetableDTO>();

            foreach (Timetable timetable in dataBase.timetables) { 
                if (timetable.line.stations[0].name.ToLower() == start) {
                    String day = timetable.isWeekday ? "Weekday" : "Weekend";
                    timetables.Add(new TimetableDTO(timetable.start,timetable.line.stations[0].name, timetable.line.stations[timetable.line.stations.Count()-1].name,timetable.line.price,timetable.train, day, timetable.ValidFrom, timetable.ValidTo));
                 }
            }
            if (timetables.Count() == 0)
            {
                btn_chooseSeat.IsEnabled = false;
            }
            else {
                btn_chooseSeat.IsEnabled = true;
            }
            timetable_table.ItemsSource = null;
            timetable_table.ItemsSource = timetables;

        }
    }


    public class TimetableDTO { 

        public String startTime {get; set;}

        public DateTime startDateTime { get; set; }
        public String startStation { get; set; }
        public String endStation { get; set; }
        public int price { get; set; }
        public Train train { get; set; }

        public String day { get; set; }    // weekday ili weekend
        public DateTime validFrom { get; set; }
        public DateTime validTo { get; set; }

        public TimetableDTO() { }

        public TimetableDTO(DateTime startDateTime, String startStation, String endStation, int price,Train train, string day, DateTime validFrom, DateTime validTo)
        {
            this.startDateTime = startDateTime;
            this.endStation = endStation;
            this.startStation = startStation;
            this.price = price;
            this.train = train;
            this.day = day;
            this.validFrom = validFrom;
            this.validTo = validTo;

            startTime = startDateTime.ToString("HH:mm");
        }
    }
}
