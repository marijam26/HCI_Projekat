using HCI_Projekat.help;
using HCI_Projekat.Model;
using HCI_Projekat.touring;
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
using ThinkSharp.FeatureTouring;
using ThinkSharp.FeatureTouring.Navigation;

namespace HCI_Projekat.Pages
{
 
    public partial class TimetableCRUD : Page
    {

        private Placement _placement;

        public Data dataBase { get; set; }
        public List<TimetableDTO> timetableList { get; set; }

        public List<TimetableDTO> allTimetables { get; set; }

        public List<Control> controlList { get; set; }

        public TimetableCRUD(Data database, List<TimetableDTO> searchList = null)
        {
          
            InitializeComponent();
            this.dataBase = database;
            DataContext = this;

            this.controlList = new List<Control>() { fromPlace, toPlace, rb_weekday, rb_weekend, btn_search, timetable_table, btn_edit, btn_delete, btn_add };

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

        private void searchClicked(object sender, RoutedEventArgs e)
        {
            foreach (Control c in this.controlList)
            {
                c.IsEnabled = true;
            }
            var navigator = FeatureTour.GetNavigator();
            navigator.IfCurrentStepEquals(ElementID.ButtonSearch).GoNext();
        }

        private void Rb_weekend_Checked(object sender, RoutedEventArgs e)
        {
            rb_weekend.IsEnabled = false;
            btn_search.IsEnabled = true;
            var navigator = FeatureTour.GetNavigator();
            navigator.IfCurrentStepEquals(ElementID.RadioWeekend).GoNext();
        }

        private void fromPlaceSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(fromPlace.SelectedItem.ToString() == "beograd")
            {
                fromPlace.IsEnabled = false;
                toPlace.IsEnabled = true;
                var navigator = FeatureTour.GetNavigator();
                navigator.IfCurrentStepEquals(ElementID.ComboBoxFrom).GoNext();
            }
        }

        private void toPlaceSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (toPlace.SelectedItem.ToString() == "novi sad")
            {
                toPlace.IsEnabled = false;
                rb_weekend.IsEnabled = true;
                var navigator = FeatureTour.GetNavigator();
                navigator.IfCurrentStepEquals(ElementID.ComboBoxTo).GoNext();
            }
        }


        public Placement Placement
        {
            get { return _placement; }
            set { Placement = value; }
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
           
            AddTimetable tc = new AddTimetable(this.dataBase);
            ManagerHomepage window = (ManagerHomepage)Window.GetWindow(this);
            window.managerHomepage.Navigate(tc);
        }

        private void btn_tutorial_Click(object sender, RoutedEventArgs e)
        {
            var navigator = FeatureTour.GetNavigator();

            navigator.ForStep(ElementID.ComboBoxFrom).AttachDoable(s => fromPlace.SelectedItem = "beograd");
            navigator.ForStep(ElementID.ComboBoxTo).AttachDoable(s => toPlace.SelectedItem = "novi sad");


            navigator.OnStepEntered(ElementID.ComboBoxFrom).Execute(s => fromPlace.Focus());
            navigator.OnStepEntered(ElementID.ComboBoxTo).Execute(s => toPlace.Focus());
            navigator.OnStepEntered(ElementID.RadioWeekend).Execute(s => rb_weekend.Focus());
            navigator.OnStepEntered(ElementID.ButtonSearch).Execute(s => btn_search.Focus());

            fromPlace.SelectionChanged += fromPlaceSelectionChanged;
            toPlace.SelectionChanged += toPlaceSelectionChanged;
            rb_weekend.Checked += Rb_weekend_Checked;
            btn_search.Click += searchClicked;


            foreach (Control c in this.controlList)
            {
                c.IsEnabled = false;
            }
            fromPlace.IsEnabled = true;
            TourStarter.StartTimetableTour();
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

            EditTimetable et = new EditTimetable(this.dataBase, t);
            ManagerHomepage window = (ManagerHomepage)Window.GetWindow(this);
            window.managerHomepage.Navigate(et);
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
            TimetableCRUD tc = new TimetableCRUD(this.dataBase, timetables);
            ManagerHomepage window = (ManagerHomepage)Window.GetWindow(this);
            window.managerHomepage.Navigate(tc);

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

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                string str = HelpProvider.GetHelpKey((DependencyObject)focusedControl);
                HelpProvider.ShowHelp(str, (MainWindow)Window.GetWindow(this));
            }
        }
    }
}
