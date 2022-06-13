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
    /// Interaction logic for MonthlyTicketReport.xaml
    /// </summary>
    public partial class MonthlyTicketReport : Page
    {
        public List<TicketShowDTO> tickets { get; set; }
        public Data dataBase { get; set; }
        public List<String> months { get; set; }

        public MonthlyTicketReport(Data dataBase)
        {
            InitializeComponent();
            this.dataBase = dataBase;
            DataContext = this;
            this.tickets = new List<TicketShowDTO>();
            foreach (User u in this.dataBase.users)
            {
                if (u.type == UserType.Client)
                {
                    foreach (Ticket t in u.tickets)
                    {
                        this.tickets.Add(new TicketShowDTO(t,u));
                    }
                }
            }
            this.months = new List<String> { "January","February","March","April","May","June","July","August","September","October","November","December" };
            cb_months.ItemsSource = this.months;
        }

        private void cb_months_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.tickets.Clear();
            string month = cb_months.SelectedItem.ToString();
            foreach(User u in this.dataBase.users)
            {
                if(u.type == UserType.Client)
                {
                    foreach(Ticket t in u.tickets)
                    {
                        if(t.purchasementDate.Month == this.months.IndexOf(month) + 1)
                        {
                            this.tickets.Add(new TicketShowDTO(t,u));
                        }
                    }
                }
            }
            dg_tickets.ItemsSource = null;
            dg_tickets.ItemsSource = this.tickets;
        }


    }
}
