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
    /// Interaction logic for ManagerHomepage.xaml
    /// </summary>
    public partial class ManagerHomepage : Page
    {
        public Data dataBase { get; set; }


        public ManagerHomepage(Data database)
        {
            InitializeComponent();
            this.dataBase = dataBase;
            DataContext = this;
        }

    }
}
