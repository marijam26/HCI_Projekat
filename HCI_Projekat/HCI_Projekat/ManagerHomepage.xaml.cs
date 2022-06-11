using HCI_Projekat.Model;
using HCI_Projekat.Pages;
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
using System.Windows.Shapes;

namespace HCI_Projekat
{
    /// <summary>
    /// Interaction logic for ManagerHomepage.xaml
    /// </summary>
    public partial class ManagerHomepage : Window
    {

        public Data dataBase { get; set; }

        public ManagerHomepage(Data dataBase)
        {
            InitializeComponent();
            Uri uri = new Uri("../../Images/icon.png", UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(uri);
            this.dataBase = dataBase;
            DataContext = this;
            this.managerHomepage.Navigate(new WelcomeManager());
        }


        private void MenuItem_Click_train(object sender, RoutedEventArgs e)
        {
            TrainCRUD r = new TrainCRUD(dataBase);
            this.managerHomepage.Navigate(r);

        }

        private void MenuItem_Click_train_line(object sender, RoutedEventArgs e)
        {

            TrainLineCRUD r = new TrainLineCRUD(dataBase);
            this.managerHomepage.Navigate(r);

        }

        private void MenuItem_Click_timetable(object sender, RoutedEventArgs e)
        {
            TimetableCRUD r = new TimetableCRUD(dataBase);
            this.managerHomepage.Navigate(r);

        }

        private void mi_railmap_Click(object sender, RoutedEventArgs e)
        {
            RailwayMap r = new RailwayMap(dataBase);
            this.managerHomepage.Navigate(r);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
        
        private void MenuItem_Click_month_report(object sender, RoutedEventArgs e)
        {
            MonthlyTicketReport r = new MonthlyTicketReport(dataBase);
            this.managerHomepage.Navigate(r);
        }
        
        private void MenuItem_Click_timetable_report(object sender, RoutedEventArgs e)
        {
            ManagerTicketReportByTimetable r = new ManagerTicketReportByTimetable(dataBase);
            this.managerHomepage.Navigate(r);
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            Close();

        }
    }
}
