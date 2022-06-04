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


    public partial class TrainCRUD : Page
    {
        public Data dataBase { get; set; }


        public TrainCRUD(Data database)
        {
            InitializeComponent();
            this.dataBase = database;
            DataContext = this;
        }

        private void train_table_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            

        }


        private void Show_Wagons(object sender,RoutedEventArgs e) {
            Train t = (Train)train_table.SelectedItem;
            WagonCRUD wagons = new WagonCRUD(t,dataBase,"view");
            MainWindow window = (MainWindow)Window.GetWindow(this);
            window.Content = wagons;
        
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

            MainWindow window = (MainWindow)Window.GetWindow(this);
            EditTrain r = new EditTrain(this.dataBase,t);
            window.Content = r;

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
            MainWindow window = (MainWindow)Window.GetWindow(this);
            AddTrain r = new AddTrain(this.dataBase);
            window.Content = r;

        }

        private bool checkInput()
        {
            /*if (tb_name.Text == "")
            {
                MessageBox.Show("Must enter name.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            int res;
            bool num = int.TryParse(tb_capacity.Text, out res);
            if (!num)
            {
                MessageBox.Show("Capacity must be number.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (rb_obican.IsChecked == false && rb_soko.IsChecked == false)
            {
                MessageBox.Show("Must choose range.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }*/
            return true;
        }
    }
}
