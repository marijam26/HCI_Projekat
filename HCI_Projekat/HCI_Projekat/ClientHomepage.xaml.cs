using HCI_Projekat.help;
using HCI_Projekat.Model;
using HCI_Projekat.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for ClientHomepage.xaml
    /// </summary>
    public partial class ClientHomepage : Window
    {
        public Data dataBase { get; set; }
        public User loggedUser { get; set; }

        public ClientHomepage(Data dataBase, User u)
        {
            loggedUser = u;
            InitializeComponent();
            Uri uri = new Uri("../../Images/icon.png", UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(uri);
            this.dataBase = dataBase;
            DataContext = this;
            this.clientHomepage.Navigate(new WelcomeClient());
        }


        

        private void Enable_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Enable_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void Komanda_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Komanda!");
        }


        private void Ugradjene_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void Ugradjene_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void MenuItem_Click_reserve_buy(object sender, RoutedEventArgs e)
        {

            ReserveBuyTicket r = new ReserveBuyTicket(this.dataBase, this.loggedUser);
            this.clientHomepage.Navigate(r);
        }

        private void MenuItem_Click_ticket_view(object sender, RoutedEventArgs e)
        {
            ClientTicketView r = new ClientTicketView(this.loggedUser);
            this.clientHomepage.Navigate(r);
        }

        private void MenuItem_Click_reservation_view(object sender, RoutedEventArgs e)
        {
            ClientReservationView r = new ClientReservationView(this.loggedUser);
            this.clientHomepage.Navigate(r);
        }

        
        private void mi_railmap_Click(object sender, RoutedEventArgs e)
        {
            RailwayMap r = new RailwayMap(this.dataBase,"client");
            this.clientHomepage.Navigate(r);
        }

        private void MenuItem_Click_report_timetable(object sender, RoutedEventArgs e)
        {
            ManagerTicketReportByTimetable r = new ManagerTicketReportByTimetable(this.dataBase);
            this.clientHomepage.Navigate(r);
        }

        private void logout_Click(object sender, RoutedEventArgs e)
        {
            var Result = MessageBox.Show("Are you sure you want to log out?", "Serbian Railways", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Result == MessageBoxResult.Yes)
            {

                MainWindow window = new MainWindow();
                window.Show();
                Close();
            }
        }

        private void help_Click(object sender, RoutedEventArgs e)
        {
            var p = this.clientHomepage.Content as Page;
            HelpProvider.ShowHelp(p.Title, this);
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var p = this.clientHomepage.Content as Page;
            HelpProvider.ShowHelp(p.Title, this);
        }
    }
}

