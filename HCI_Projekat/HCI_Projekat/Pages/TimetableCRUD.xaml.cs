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
    /// Interaction logic for TimetableCRUD.xaml
    /// </summary>
    public partial class TimetableCRUD : Page
    {
        public Data dataBase { get; set; }
        public List<TimetableDTO> timetableList { get; set; }

        public TimetableCRUD(Data database)
        {
            InitializeComponent();
            this.dataBase = database;
            DataContext = this;
            this.timetableList = new List<TimetableDTO>();
            for(int i = 0; i < dataBase.timetables.Count; i++)
            {
                //timetable.line.price, timetable.train, day, timetable.ValidFrom, timetable.ValidTo));
                String day = database.timetables[i].isWeekday ? "Weekday" : "Weekend";
                this.timetableList.Add(new TimetableDTO(database.timetables[i].start, database.timetables[i].line.stations[0].name,
                     database.timetables[i].line.stations[database.timetables[i].line.stations.Count() - 1].name,
                      database.timetables[i].line.price, database.timetables[i].train, day, database.timetables[i].ValidFrom, database.timetables[i].ValidTo));
            }
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

            Timetable t = (Timetable)timetable_table.SelectedItem;

            MainWindow window = (MainWindow)Window.GetWindow(this);
            EditTimetable et = new EditTimetable(this.dataBase, t);
            window.Content = et;
        }

        private void searchTimetable(object sender, RoutedEventArgs e)
        {
            String start = fromPlace.Text.Trim().ToLower();
            String end = toPlace.Text.Trim().ToLower();
            String day = (bool)rb_weekday.IsChecked ? "weekday" : "weekend";

            List<TimetableDTO> timetables = new List<TimetableDTO>();


            foreach (Timetable timetable in dataBase.timetables)
            {
                if (timetable.line.stations[0].name.ToLower() == start)
                {
                    //timetables.Add(new TimetableDTO(timetable.startDateTime, timetable.line.stations[0].name, timetable.line.stations[timetable.line.stations.Count() - 1].name, timetable.line.price, timetable.train));
                }
            }

        }
    }
}
