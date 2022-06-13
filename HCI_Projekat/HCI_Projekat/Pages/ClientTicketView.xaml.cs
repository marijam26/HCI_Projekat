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
    /// Interaction logic for ClientTicketView.xaml
    /// </summary>
    public partial class ClientTicketView : Page
    {

        public User loggedUser { get; set; }
        public List<TicketShowDTO> tickets { get; set; }

        public ClientTicketView(User loggedUser)
        {
            InitializeComponent();
            this.loggedUser = loggedUser;
            DataContext = this;
            this.tickets = formTicketShowDTO(loggedUser.tickets);
        }

        private List<TicketShowDTO> formTicketShowDTO(List<Ticket> tickets)
        {
            List<TicketShowDTO> ticketDTO = new List<TicketShowDTO>();
            foreach (Ticket ticket in tickets) {
                ticketDTO.Add(new TicketShowDTO(ticket));
            }
            return ticketDTO;
        }

        private void timetable_table_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {



        }
        

    }

}
