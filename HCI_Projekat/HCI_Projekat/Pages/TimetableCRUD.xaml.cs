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
        public TimetableCRUD(Data database)
        {
            InitializeComponent();
            this.dataBase = database;
            DataContext = this;
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
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

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Window.GetWindow(this);
            AddTimetable tc = new AddTimetable(this.dataBase);
            window.Content = tc;
        }
    }
}
