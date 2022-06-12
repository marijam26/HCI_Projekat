using HCI_Projekat.help;
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
    /// Interaction logic for ManagerTicketReportByTimetable.xaml
    /// </summary>
    public partial class ManagerTicketReportByTimetable : Page
    {

        public Data database { get; set; }
        List<TicketShowDTO> tickets { get; set; }

        public ManagerTicketReportByTimetable(Data dataBase)
        {
            InitializeComponent();
            this.database = dataBase;
            fromPlace.ItemsSource = getStations();
            toPlace.ItemsSource = getStations();
            DataContext = this;
            this.tickets = new List<TicketShowDTO>();
            getAllSoldTickets();
        }

        private void getAllSoldTickets()
        {
            foreach (User u in database.users)
            {
                foreach (Ticket ticket in u.tickets)
                {
                     
                    this.tickets.Add(new TicketShowDTO(ticket, u));     

                }
            }
            timetable_table.ItemsSource = null;
            timetable_table.ItemsSource = tickets;
        }

        private IEnumerable getStations()
        {
            List<string> stations = new List<string>();
            foreach (TrainLine line in database.trainLines)
            {
                foreach (Station place in line.stations)
                {
                    if (!stations.Contains(place.name.ToLower()))
                    {
                        stations.Add(place.name.ToLower());
                    }
                }
            }
            return stations;
        }


        private void timetable_table_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public void btn_search_Click(object sender, RoutedEventArgs e)
        {
            this.tickets = new List<TicketShowDTO>();
            String startStation = fromPlace.Text.Trim().ToLower();
            String endStation = toPlace.Text.Trim().ToLower();

            if (startStation == "") {
                MessageBox.Show("Must enter start station.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (endStation == "") {
                MessageBox.Show("Must enter end station.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (startDatePick.SelectedDate == null) {
                MessageBox.Show("Must enter departure date.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (tb_time.Text.Trim() == "") {
                MessageBox.Show("Must enter departure time.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DateTime date = (DateTime)startDatePick.SelectedDate;
            DateTime start = DateTime.Parse(tb_time.Text.Trim());

            DateTime datetime = new DateTime(date.Year,date.Month,date.Day,start.Hour,start.Minute,start.Second);

            foreach (User u in database.users) {
                foreach (Ticket ticket in u.tickets) {
                    foreach (Timetable t in ticket.timetable) {
                        if (t.line.stations[0].name.ToLower() == startStation.ToLower() && t.line.stations[t.line.stations.Count-1].name.ToLower() == endStation.ToLower()) {
                            if (ticket.date.Date.Equals(date) && t.start.TimeOfDay.Equals(datetime.TimeOfDay)) {
                                this.tickets.Add(new TicketShowDTO(ticket,u));
                            }
                        
                        }
                    }

                }
            }
            timetable_table.ItemsSource = null;
            timetable_table.ItemsSource = tickets;
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                //string str = HelpProvider.GetHelpKey((DependencyObject)focusedControl);
                HelpProvider.ShowHelp("report2", (ManagerHomepage)Window.GetWindow(this));
            }
        }
    }
}
