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
    /// Interaction logic for AddTimetable.xaml
    /// </summary>
    public partial class AddTimetable : Page
    {
        public Data dataBase { get; set; }

        public AddTimetable(Data dataBase)
        {
            InitializeComponent();
            this.dataBase = dataBase;
        }
    }
}
