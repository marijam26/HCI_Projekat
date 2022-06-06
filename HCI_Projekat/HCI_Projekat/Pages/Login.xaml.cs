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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Data dataBase { get; set; }
        public Login(Data dataBase)
        {
            InitializeComponent();
            this.dataBase = dataBase;
            DataContext = this;
            
        }

        private void btn_login_Click(object sender, RoutedEventArgs e)
        {
            string username = tb_username.Text;
            string password = tb_password.Text;
            bool found = false;
            MainWindow window = (MainWindow)Window.GetWindow(this);

            if (username == "" || password == "")
            {
                MessageBox.Show("Please enter username and password.","Invalid",MessageBoxButton.OK,MessageBoxImage.Warning);
                return;
            }
            else
            {
                foreach(User u in dataBase.users){
                    if(u.username == username && u.password == password)
                    {
                        dataBase.currentUser = u;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    MessageBox.Show("Invalid username and password..", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            if(dataBase.currentUser.type == UserType.Client)
            {
                ClientHomepage ch = new ClientHomepage(dataBase);
                window.Content = ch;
            }
            else
            {
                ManagerHomepage mh = new ManagerHomepage(this.dataBase);
                window.Content = mh;
            }
        }

        private void btn_register_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Window.GetWindow(this);
            Registration r = new Registration(this.dataBase);
            window.Content = r;
        }
    }
}
