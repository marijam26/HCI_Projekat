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
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Page
    {
        public Data dataBase { get; set; }

        public Registration(Data dataBase)
        {
            InitializeComponent();
            this.dataBase = dataBase;
            DataContext = this;
        }

        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Window.GetWindow(this);
            Login r = new Login(this.dataBase);
            window.Content = r;
        }

        private void btn_register_Click(object sender, RoutedEventArgs e)
        {
            string username = tb_username.Text;
            string password = tb_password.Text;
            string name = tb_name.Text;
            string surname = tb_surname.Text;
            UserType usertype = UserType.Client;
            if(rb_client.IsChecked == true)
            {
                usertype = UserType.Client;
            }else if(rb_manager.IsChecked == true)
            {
                usertype = UserType.Manager;
            }
            else
            {
                MessageBox.Show("Please fill in all the fields.", "Serbian Raliways", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if(username != "" && password != "" && name != "" && surname != "")
            {
                if (!this.dataBase.users.Select(x => x.username).ToList().Contains(username))
                {
                    User newUser = new User(username,password,name,surname, usertype);
                    this.dataBase.users.Add(newUser);
                    MessageBox.Show("Successfully registered.", "Serbian Raliways", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainWindow window = (MainWindow)Window.GetWindow(this);
                    Login r = new Login(this.dataBase);
                    window.Content = r;
                }
                else
                {
                    MessageBox.Show("Username already exists.", "Serbian Raliways", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                
            }
            else
            {
                MessageBox.Show("Please fill in all the fields.", "Serbian Raliways", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

        }
        
    }
}
