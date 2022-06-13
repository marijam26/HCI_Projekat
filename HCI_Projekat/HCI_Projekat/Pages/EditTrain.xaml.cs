using HCI_Projekat.help;
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
    /// Interaction logic for EditTrain.xaml
    /// </summary>
    public partial class EditTrain : Page
    {
        public Data dataBase { get; set; }
        public Train train { get; set; }

        public EditTrain(Data database,Train train)
        {
            InitializeComponent();
            this.dataBase = database;
            this.train = train;
            tb_name.Text = train.name;
            
            if (train.rang == Rang.Soko)
            {
                rb_soko.IsChecked = true;
            }
            else
            {
                rb_simple.IsChecked = true;
            }
            DataContext = this;
            BrushConverter bc = new BrushConverter();
            Brush brush = (Brush)bc.ConvertFrom("#e8cfa5");
            brush.Freeze();
            Background = brush;
        }


        private bool checkInput()
        {

            String name = tb_name.Text.Trim();
            if (name == "")
            {
                MessageBox.Show("You must enter a name.", "Serbian Raliways", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (rb_simple.IsChecked == false && rb_soko.IsChecked == false)
            {
                MessageBox.Show("You must choose a rang.", "Serbian Raliways", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private void btn_next_Click(object sender, RoutedEventArgs e)
        {
            if (!checkInput())
            {
                return;
            }
            Train t = new Train();
            int id = this.dataBase.trains.Max(x => x.id);
            t.id = id + 1;
            t.name = tb_name.Text.Trim();
            t.rang = (bool)rb_simple.IsChecked ? Rang.obicni : Rang.Soko;
            WagonCRUD r = new WagonCRUD(t, this.dataBase, "edit");
            ManagerHomepage window = (ManagerHomepage)Window.GetWindow(this);
            window.managerHomepage.Navigate(r);
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            TrainCRUD r = new TrainCRUD(this.dataBase);
            ManagerHomepage window = (ManagerHomepage)Window.GetWindow(this);
            window.managerHomepage.Navigate(r);
        }

        
    }
}
