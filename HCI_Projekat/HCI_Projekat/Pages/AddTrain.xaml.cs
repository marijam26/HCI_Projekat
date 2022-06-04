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
    /// Interaction logic for AddTrain.xaml
    /// </summary>
    public partial class AddTrain : Page
    {

        public Data dataBase { get; set; }
        public Train newTrain { get; set; }

        public AddTrain(Data database)
        {
            InitializeComponent();
            this.dataBase = database;
            DataContext = this;
            this.newTrain = null;
        }
        
        public AddTrain(Data database,Train train)
        {
            InitializeComponent();
            this.dataBase = database;
            DataContext = this;
            this.newTrain = train;
            tb_name.Text = train.name;

            if (train.rang == Rang.Soko)
            {
                rb_soko.IsChecked = true;
            }
            else
            {
                rb_simple.IsChecked = true;
            }
        }

        private bool checkInput()
        {

            String name = tb_name.Text.Trim();
            if (name == "")
            {
                MessageBox.Show("Must enter name.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (rb_simple.IsChecked == false && rb_soko.IsChecked == false)
            {
                MessageBox.Show("Must choose rang.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private void btn_next_Click(object sender, RoutedEventArgs e)
        {
            if (!checkInput()) {
                return;
            }
            Train t;
            if (this.newTrain == null)
            {
                t = new Train();
                int id = this.dataBase.trains.Max(x => x.id);
                t.id = id + 1;
                t.name = tb_name.Text.Trim();
                t.rang = (bool)rb_simple.IsChecked ? Rang.obicni : Rang.Soko;
            }
            else {
                t = this.newTrain;
            }
            MainWindow window = (MainWindow)Window.GetWindow(this);
            WagonCRUD r = new WagonCRUD(t,this.dataBase,"add");
            window.Content = r;
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Window.GetWindow(this);
            TrainCRUD r = new TrainCRUD(this.dataBase);
            window.Content = r;
        }
    }
}
