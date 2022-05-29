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
    /// Interaction logic for TrainCRUD.xaml
    /// </summary>
    public partial class TrainCRUD : Page
    {
        public Data dataBase { get; set; }


        public TrainCRUD(Data database)
        {
            InitializeComponent();
            this.dataBase = database;
            DataContext = this;
            btn_save.IsEnabled = false;
        }

        private void train_table_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            

        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            if (!checkInput())
            {
                return;
            }
            var Result = MessageBox.Show("Do you want to change train?", "Check", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(Result == MessageBoxResult.Yes)
            {
                Train t = (Train)train_table.SelectedItem;
                foreach(Train train in dataBase.trains)
                {
                    if(train.id == t.id)
                    {
                        train.name = tb_name.Text;
                        train.capacity = int.Parse(tb_capacity.Text);
                        train.rang = (bool)rb_soko.IsChecked ? TrainRang.Soko : TrainRang.Obican;
                    }
                }
                train_table.ItemsSource = null;
                train_table.ItemsSource = dataBase.trains;
                MessageBox.Show("Successfully changed train!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            }

            tb_name.Text = "";
            tb_capacity.Text = "";

            btn_add.IsEnabled = true;
            btn_edit.IsEnabled = true;
            btn_delete.IsEnabled = true;
            btn_save.IsEnabled = false;

        }

        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            int selectedCells = train_table.SelectedCells.Count();
            if (selectedCells == 0)
            {
                MessageBox.Show("Must select train.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            btn_add.IsEnabled = false;
            btn_edit.IsEnabled = false;
            btn_delete.IsEnabled = false;
            btn_save.IsEnabled = true;

            Train t = (Train)train_table.SelectedItem;

            if (t != null)
            {
                tb_name.Text = t.name;
                tb_capacity.Text = t.capacity.ToString();

                if (t.rang == TrainRang.Soko)
                {
                    rb_soko.IsChecked = true;
                }
                else
                {
                    rb_obican.IsChecked = true;
                }
            }

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
            if(!checkInput())
            {
                return;
            }
            

            var Result = MessageBox.Show("Do you want to add train?", "Check", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Result == MessageBoxResult.Yes)
            {
                Train train = new Train();
                train.name = tb_name.Text;
                train.capacity = int.Parse(tb_capacity.Text);
                train.rang = (bool)rb_soko.IsChecked ? TrainRang.Soko : TrainRang.Obican;
                int id = this.dataBase.trains.Max(x => x.id);
                train.id = id+1;

                this.dataBase.trains.Add(train);
                tb_name.Text = "";
                tb_capacity.Text = "";
                train_table.ItemsSource = null;
                train_table.ItemsSource = dataBase.trains;
            }

        }

        private bool checkInput()
        {
            if (tb_name.Text == "")
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
            }
            return true;
        }
    }
}
