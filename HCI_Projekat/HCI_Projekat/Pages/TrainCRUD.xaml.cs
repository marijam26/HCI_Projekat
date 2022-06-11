using HCI_Projekat.Model;
using HCI_Projekat.touring;
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
using ThinkSharp.FeatureTouring.Navigation;

namespace HCI_Projekat.Pages
{


    public partial class TrainCRUD : Page
    {
        public Data dataBase { get; set; }
        public bool tour = false;

        public TrainCRUD(Data database)
        {
            InitializeComponent();
            this.dataBase = database;
            DataContext = this;
        }

        private void train_table_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            

        }

        private void btn_tutorial_Click(object sender, RoutedEventArgs e)
        {
            var navigator = FeatureTour.GetNavigator();

            navigator.OnStepEntered(ElementID.TrainButtonAdd).Execute(s => btn_add.Focus());

            btn_edit.IsEnabled = false;
            btn_delete.IsEnabled = false;
            train_table.IsEnabled = false;

            btn_add.IsEnabled = true;

            tour = true;
            TourStarter.StartTrainTour();
        }


        private void Show_Wagons(object sender,RoutedEventArgs e) {
            Train t = (Train)train_table.SelectedItem;
            WagonCRUD wagons = new WagonCRUD(t,dataBase,"view");
            ManagerHomepage window = (ManagerHomepage)Window.GetWindow(this);
            window.managerHomepage.Navigate(wagons);
        
        }

        
        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            int selectedCells = train_table.SelectedCells.Count();
            if (selectedCells == 0)
            {
                MessageBox.Show("Must select train.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
          
            Train t = (Train)train_table.SelectedItem;

            EditTrain r = new EditTrain(this.dataBase,t);
            ManagerHomepage window = (ManagerHomepage)Window.GetWindow(this);
            window.managerHomepage.Navigate(r);
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {

            int selectedCells = train_table.SelectedCells.Count();
            if(selectedCells == 0)
            {
                MessageBox.Show("Must select train.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var Result = MessageBox.Show("Do you want to delete train?", "Check", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Result == MessageBoxResult.Yes)
            { 
                Train t = (Train)train_table.SelectedItem;
                this.dataBase.trains.Remove(t);
                train_table.ItemsSource = null;
                train_table.ItemsSource = dataBase.trains;
            }
                
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            ManagerHomepage window = (ManagerHomepage)Window.GetWindow(this);
            AddTrain r = new AddTrain(this.dataBase, tour);
            window.managerHomepage.Navigate (r);
            if (tour)
            {
                r.StartTour();
            }

        }

    }
}
