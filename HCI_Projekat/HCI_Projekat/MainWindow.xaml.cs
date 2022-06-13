using HCI_Projekat.help;
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
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace HCI_Projekat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Data dataBase { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Uri uri = new Uri("../../Images/icon.png", UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(uri);
            dataBase = new Data();
            Login p = new Login(dataBase);
            this.Content = p;

        }


        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var p = this.Content as Page;
            HelpProvider.ShowHelp(p.Title, this);
        }
    }
}
