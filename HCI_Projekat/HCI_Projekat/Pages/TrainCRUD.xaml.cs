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
        }

        private void train_table_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Train t = (Train)train_table.SelectedItem;
            if(t.rang == TrainRang.Soko)
            {
                rb_soko.IsChecked = true;
            }
            else
            {
                rb_obican.IsChecked = true;
            }


        }
    }
}
