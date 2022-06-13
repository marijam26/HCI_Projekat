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
    /// Interaction logic for EditTimetable.xaml
    /// </summary>
    public partial class EditTimetable : Page
    {
        public Data Database { get; set; }
        public TimetableDTO Timetable { get; set; }

        public EditTimetable(Data database, TimetableDTO timetable)
        {
            InitializeComponent();
            this.Database = database;
            this.Timetable = timetable;

            fromPlace.ItemsSource = getStations();
            toPlace.ItemsSource = getStations();
            train.ItemsSource = getTrains();

            fromPlace.SelectedItem = timetable.startStation.ToLower();
            toPlace.SelectedItem = timetable.endStation.ToLower();

            tb_time.SelectedTime = timetable.startDateTime;
            train.SelectedItem = timetable.train.name.ToLower();

            if(timetable.day == "Weekday")
            {
                rb_weekday.IsChecked = true;
            }
            else
            {
                rb_weekend.IsChecked = true;
            }

            tb_valid_since.SelectedDate = timetable.validFrom;
            tb_valid_until.SelectedDate = timetable.validTo;

        }

        private IEnumerable getStations()
        {
            List<string> stations = new List<string>();
            foreach (TrainLine line in Database.trainLines)
            {
                foreach (Station s in line.stations)
                {
                    if (!stations.Contains(s.name.ToLower()))
                    {
                        stations.Add(s.name.ToLower());
                    }
                }
            }
            return stations;
        }

        private IEnumerable getTrains()
        {
            List<string> trains = new List<string>();
            foreach (Train t in Database.trains)
            {
                if (!trains.Contains(t.name.ToLower()))
                {
                    trains.Add(t.name.ToLower());
                }
            }
            return trains;
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            if (!checkInput())
            {
                return;
            }
            Boolean isWeekday = (bool)rb_weekday.IsChecked ? true : false;

            // find line
            String from = fromPlace.Text.Trim().ToLower();
            String to = toPlace.Text.Trim().ToLower();

            TrainLine line = null;
            foreach (TrainLine l in Database.trainLines)
            {
                if (l.stations[0].name.ToLower() == from && l.stations[l.stations.Count - 1].name.ToLower() == to)
                {
                    line = l;
                    break;
                }
            }
            if (line == null)
            {
                MessageBox.Show("There is no line with chosen stations.", "Serbian Raliways", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Train t = null;
            foreach (Train tr in Database.trains)
            {
                if (tr.name.ToLower() == train.Text.Trim().ToLower())
                {
                    t = tr;
                    break;
                }
            }


            foreach (Timetable tt in Database.timetables)
            {
                if (tt.id == Timetable.id)
                {
                    tt.start = DateTime.Parse(tb_time.Text.Trim());
                    tt.isWeekday = isWeekday;
                    tt.ValidFrom = (DateTime)tb_valid_since.SelectedDate;
                    tt.ValidTo = (DateTime)tb_valid_until.SelectedDate;
                    tt.line = line;
                    tt.train = t;
                    break;
                }
            }
            MessageBox.Show("Successfully edited the timetable!", "Serbian Raliways", MessageBoxButton.OK, MessageBoxImage.Information);
            TimetableCRUD tc = new TimetableCRUD(this.Database);
            ManagerHomepage window = (ManagerHomepage)Window.GetWindow(this);
            window.managerHomepage.Navigate(tc);

        }
            private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            TimetableCRUD t = new TimetableCRUD(this.Database);
            ManagerHomepage window = (ManagerHomepage)Window.GetWindow(this);
            window.managerHomepage.Navigate(t);
        }

        private bool checkInput()
        {
            String from = fromPlace.Text.Trim().ToLower();
            if (from == "")
            {
                MessageBox.Show("You must choose a start station.", "Serbian Raliways", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            String to = toPlace.Text.Trim().ToLower();
            if (to == "")
            {
                MessageBox.Show("You must choose an end station.", "Serbian Raliways", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // provjeri da li postoji ta linija

            String t = train.Text.Trim().ToLower();
            if (t == "")
            {
                MessageBox.Show("You must choose a train.", "Serbian Raliways", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            String time = tb_time.Text.Trim();
            if (time == "")
            {
                MessageBox.Show("You must enter a departure time.", "Serbian Raliways", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            DateTime datetime = DateTime.Parse(time);

            DateTime? validSince = tb_valid_since.SelectedDate;
            if (validSince == null)
            {
                MessageBox.Show("You must choose a validity start date.", "Serbian Raliways", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            DateTime? validUntil = tb_valid_until.SelectedDate;
            if (validUntil == null)
            {
                MessageBox.Show("You must choose a validity end date.", "Serbian Raliways", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (rb_weekday.IsChecked == false && rb_weekend.IsChecked == false)
            {
                MessageBox.Show("You must choose between a weekday and a weekend.", "Serbian Raliways", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
    }
}
