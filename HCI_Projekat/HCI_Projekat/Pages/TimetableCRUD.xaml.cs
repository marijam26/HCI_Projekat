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
 
    public partial class TimetableCRUD : Page
    {
        public Data dataBase { get; set; }
        public List<TimetableDTO> timetableList { get; set; }

        public List<TimetableDTO> allTimetables { get; set; }

        public TimetableCRUD(Data database, List<TimetableDTO> searchList = null)
        {
          
            InitializeComponent();
            this.dataBase = database;
            DataContext = this;
          


            this.timetableList = new List<TimetableDTO>();
            this.allTimetables = new List<TimetableDTO>();
            for (int i = 0; i < dataBase.timetables.Count; i++)
            {
                String day = database.timetables[i].isWeekday ? "Weekday" : "Weekend";
                this.timetableList.Add(new TimetableDTO(database.timetables[i].id, database.timetables[i].line, database.timetables[i].start, database.timetables[i].line.stations[0].name,
                     database.timetables[i].line.stations[database.timetables[i].line.stations.Count() - 1].name,
                      database.timetables[i].line.price, database.timetables[i].train, day, database.timetables[i].ValidFrom, database.timetables[i].ValidTo));
                this.allTimetables.Add(new TimetableDTO(database.timetables[i].id, database.timetables[i].line, database.timetables[i].start, database.timetables[i].line.stations[0].name,
                    database.timetables[i].line.stations[database.timetables[i].line.stations.Count() - 1].name,
                     database.timetables[i].line.price, database.timetables[i].train, day, database.timetables[i].ValidFrom, database.timetables[i].ValidTo));

            }

            if (searchList != null)
            {
                this.timetableList = searchList;
            }

            fromPlace.ItemsSource = getStations();
            toPlace.ItemsSource = getStations();

        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)  // promijeni tako da brise i iz dto i iz database
        {
            int selectedCells = timetable_table.SelectedCells.Count();
            if (selectedCells == 0)
            {
                MessageBox.Show("Must select timetable.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var Result = MessageBox.Show("Do you want to delete timetable?", "Check", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Result == MessageBoxResult.Yes)
            {
                Timetable t = (Timetable)timetable_table.SelectedItem;
                this.dataBase.timetables.Remove(t);
                timetable_table.ItemsSource = null;
                timetable_table.ItemsSource = dataBase.timetables;
            }
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)   // ici ce preko tabele
        {
            MainWindow window = (MainWindow)Window.GetWindow(this);
            AddTimetable tc = new AddTimetable(this.dataBase);
            window.Content = tc;
        }

        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            int selectedCells = timetable_table.SelectedCells.Count();
            if (selectedCells == 0)
            {
                MessageBox.Show("You must select a timetable first.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            TimetableDTO t = (TimetableDTO)timetable_table.SelectedItem;

            MainWindow window = (MainWindow)Window.GetWindow(this);
            EditTimetable et = new EditTimetable(this.dataBase, t);
            window.Content = et;
        }

        private void searchTimetable(object sender, RoutedEventArgs e)
        {
            String start = fromPlace.Text.Trim().ToLower();
            String end = toPlace.Text.Trim().ToLower();

            String day = "";
            if ((bool)rb_weekday.IsChecked)
            {
                day = "Weekday";
            }
            else if ((bool)rb_weekend.IsChecked)
            {
                day = "Weekend";
            }

            List<TimetableDTO> timetables = new List<TimetableDTO>();


            foreach (TimetableDTO dto in allTimetables)
            {
                if ((dto.line.stations[0].name.ToLower() == start || start == "") && (dto.line.stations[dto.line.stations.Count-1].name.ToLower() == end || end == "") && (dto.day == day || day == ""))
                {
                    timetables.Add(dto);
                }
            }

            this.timetableList = timetables;
            MainWindow window = (MainWindow)Window.GetWindow(this);
            TimetableCRUD tc = new TimetableCRUD(this.dataBase, timetables);
            window.Content = tc;

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
    }
}
