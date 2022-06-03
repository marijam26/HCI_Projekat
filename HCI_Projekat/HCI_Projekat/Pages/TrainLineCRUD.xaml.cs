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
    /// Interaction logic for TrainLineCRUD.xaml
    /// </summary>
    public partial class TrainLineCRUD : Page
    {
        public Data dataBase { get; set; }

        public TrainLineCRUD(Data database)
        {
            InitializeComponent();
            this.dataBase = database;
            DataContext = this;
            

        }

        private void Show_Stations(object sender, RoutedEventArgs e)
        {
            TrainLine t = (TrainLine)trainLine_table.SelectedItem;
            Stations s = new Stations(t,this.dataBase);
            MainWindow window = (MainWindow)Window.GetWindow(this);
            window.Content = s;

        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            int selectedCells = trainLine_table.SelectedCells.Count();
            if (selectedCells == 0)
            {
                MessageBox.Show("Must select train line.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var Result = MessageBox.Show("Do you want to delete train line?", "Check", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Result == MessageBoxResult.Yes)
            {
                TrainLine t = (TrainLine)trainLine_table.SelectedItem;
                this.dataBase.trainLines.Remove(t);
                trainLine_table.ItemsSource = null;
                trainLine_table.ItemsSource = dataBase.trainLines;
            }
        }
    }
}
