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
    /// Interaction logic for ManagerNavbar.xaml
    /// </summary>
    public partial class ManagerNavbar : UserControl
    {
        public ManagerNavbar()
        {
            InitializeComponent();
        }

        private void bt_trains_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Window.GetWindow(this);
            TrainCRUD tc = new TrainCRUD(window.dataBase);
            window.Content = tc;
        }
        
        private void bt_trainLines_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Window.GetWindow(this);
            TrainLineCRUD tc = new TrainLineCRUD(window.dataBase);
            window.Content = tc;
        }


        private void bt_timetable_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Window.GetWindow(this);
            TimetableCRUD tc = new TimetableCRUD(window.dataBase);
            window.Content = tc;
        }
    }
}
