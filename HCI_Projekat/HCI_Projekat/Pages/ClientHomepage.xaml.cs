using HCI_Projekat.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HCI_Projekat.Pages
{
    /// <summary>
    /// Interaction logic for ClientHomepage.xaml
    /// </summary>
    public partial class ClientHomepage : Page
    {
        public Data dataBase { get; set; }
        public User loggedUser { get; set; }

        public ClientHomepage(Data dataBase,User u)
        {
            loggedUser = u;
            InitializeComponent();
            this.dataBase = dataBase;
            DataContext = this;
        }

        private void HelloWorld_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Hello world!");
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
            MainWindow window = (MainWindow)Window.GetWindow(this);
            ReserveBuyTicket r = new ReserveBuyTicket(this.dataBase,this.loggedUser);
            window.Content = r;
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {

        }
    }
}
