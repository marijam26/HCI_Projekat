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

namespace HCI_Projekat.Pages
{
    /// <summary>
    /// Interaction logic for AddTimetable.xaml
    /// </summary>
    public partial class AddTimetable : Page
    {
        public Data dataBase { get; set; }

        public Timetable NewTimetable { get; set; }

        public AddTimetable(Data dataBase)
        {
            InitializeComponent();
            this.dataBase = dataBase;
            fromPlace.ItemsSource = getStations();
            toPlace.ItemsSource = getStations();
            train.ItemsSource = getTrains();
        }

        private IEnumerable getStations()
        {
            List<string> stations = new List<string>();
            foreach (TrainLine line in dataBase.trainLines)
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
            foreach (Train t in dataBase.trains)
            {
                if (!trains.Contains(t.name.ToLower()))
                {
                    trains.Add(t.name.ToLower());
                }
            }
            return trains;
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            ManagerHomepage window = (ManagerHomepage)Window.GetWindow(this);
            TimetableCRUD t = new TimetableCRUD(this.dataBase);
            window.managerHomepage.Navigate(t);
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            if (!checkInput())
            {
                return;
            }
            int id = this.dataBase.timetables.Max(x => x.id) + 1;
            Boolean isWeekday = (bool)rb_weekday.IsChecked ? true : false;

            // find line
            String from = fromPlace.Text.Trim().ToLower();
            String to = toPlace.Text.Trim().ToLower();

            TrainLine line = null;
            foreach(TrainLine l in dataBase.trainLines)
            {
                if (l.stations[0].name.ToLower() == from && l.stations[l.stations.Count-1].name.ToLower() == to)
                {
                    line = l; 
                    break;
                }
            }
            if(line == null)
            {
                MessageBox.Show("There is no line with chosen stations.", "Serbian Railways", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Train t = null;
            foreach(Train tr in dataBase.trains)
            {
                if(tr.name.ToLower() == train.Text.Trim().ToLower())
                {
                    t = (Train)tr.Clone();
                    break;
                }
            }

            if(t == null)
            {
                MessageBox.Show("Invalid train name chosen.", "Serbian Railways", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            this.NewTimetable = new Timetable { id = id, start = DateTime.Parse(tb_time.Text.Trim()),  isWeekday = isWeekday, 
                                                ValidFrom = (DateTime)tb_valid_since.SelectedDate, ValidTo = (DateTime)tb_valid_until.SelectedDate,
                                                line = line, train = t};     // ... 
            this.dataBase.timetables.Add(this.NewTimetable);

            MessageBox.Show("Successfully added a new departure time!", "Serbian Railways", MessageBoxButton.OK, MessageBoxImage.Information);
            TimetableCRUD tc = new TimetableCRUD(this.dataBase);

            ManagerHomepage window = (ManagerHomepage)Window.GetWindow(this);
            window.managerHomepage.Navigate(tc);

        }

        private bool checkInput()
        {
            String from = fromPlace.Text.Trim().ToLower();
            if(from == "")
            {
                MessageBox.Show("You must choose a start station.", "Serbian Railways", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            String to = toPlace.Text.Trim().ToLower();
            if (to == "")
            {
                MessageBox.Show("You must choose an end station.", "Serbian Railways", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // provjeri da li postoji ta linija

            String t = train.Text.Trim().ToLower();
            if (t == "")
            {
                MessageBox.Show("You must choose a train.", "Serbian Railways", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            String time = tb_time.Text.Trim();
            if(time == "")
            {
                MessageBox.Show("You must enter a departure time.", "Serbian Railways", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            DateTime datetime = DateTime.Parse(time);

            DateTime? validSince = tb_valid_since.SelectedDate;
            if(validSince == null)
            {
                MessageBox.Show("You must choose a validity start date.", "Serbian Railways", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            DateTime? validUntil = tb_valid_until.SelectedDate;
            if (validUntil == null)
            {
                MessageBox.Show("You must choose a validity end date.", "Serbian Railways", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if(rb_weekday.IsChecked == false && rb_weekend.IsChecked == false)
            {
                MessageBox.Show("You must choose between a weekday and a weekend.", "Serbian Railways", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if(validSince < DateTime.Now || validUntil < DateTime.Now)
            {
                MessageBox.Show("You must choose a validity period in the future.", "Serbian Railways", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

    }
}
