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
    /// Interaction logic for ClientReservationView.xaml
    /// </summary>
    public partial class ClientReservationView : Page
    {
        public User loggedUser { get; set; }
        public List<TicketShowDTO> tickets { get; set; }

        public ClientReservationView(User loggedUser)
        {
            InitializeComponent();
            this.loggedUser = loggedUser;
            DataContext = this;
            this.tickets = formTicketShowDTO(loggedUser.reservations);
        }


        private List<TicketShowDTO> formTicketShowDTO(List<Ticket> tickets)
        {
            List<TicketShowDTO> ticketDTO = new List<TicketShowDTO>();
            List<Ticket> deleteTickets = new List<Ticket>();
            foreach (Ticket ticket in tickets)
            {
                if ((ticket.date.Date - DateTime.Now.Date).TotalDays <= 1) {
                    deleteTickets.Add(ticket);
                }
                else { 
                ticketDTO.Add(new TicketShowDTO(ticket));
                }
            }
            foreach (Ticket ticket in deleteTickets)
            {
                loggedUser.tickets.Add(ticket);
                loggedUser.reservations.Remove(ticket);
            }
            if (loggedUser.reservations.Count == 0)
            {
                btn_cancel.IsEnabled = false;
                btn_confirm.IsEnabled = false;
            }
            else {
                btn_cancel.IsEnabled = true;
                btn_confirm.IsEnabled = true;
            }

            return ticketDTO;
        }

        private void timetable_table_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {



        }


        public void btn_cancel_Click(object sender, RoutedEventArgs e)
        {

            int selectedCells = timetable_table.SelectedCells.Count();
            if (selectedCells == 0)
            {
                MessageBox.Show("You must select a reservation for canceling.", "Serbian Raliways", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var Result = MessageBox.Show("Do you want to cancel reservation?", "Serbian Raliways", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Result == MessageBoxResult.Yes)
            {
                TicketShowDTO ticket = (TicketShowDTO)timetable_table.SelectedItem;
                foreach (Ticket t in loggedUser.reservations) {
                    if (t.id == ticket.id) {
                        loggedUser.reservations.Remove(t);
                        break;
                    }
                }
                MessageBox.Show("Reservation is successfully canceled.", "Serbian Raliways", MessageBoxButton.OK, MessageBoxImage.Information);
                timetable_table.ItemsSource = null;
                timetable_table.ItemsSource = formTicketShowDTO(loggedUser.reservations);
            }
        }
        public void btn_confirm_Click(object sender, RoutedEventArgs e)
        {

            int selectedCells = timetable_table.SelectedCells.Count();
            if (selectedCells == 0)
            {
                MessageBox.Show("You must select a reservation for confirmation.", "Serbian Raliways", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var Result = MessageBox.Show("Do you want to confirm reservation?", "Serbian Raliways", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Result == MessageBoxResult.Yes)
            {
                TicketShowDTO ticket = (TicketShowDTO)timetable_table.SelectedItem;
                foreach (Ticket t in loggedUser.reservations) {
                    if (t.id == ticket.id) {
                        loggedUser.reservations.Remove(t);
                        loggedUser.tickets.Add(t);
                        break;
                    }
                }
                MessageBox.Show("Reservation is successfully confirmed.", "Serbian Raliways", MessageBoxButton.OK, MessageBoxImage.Information);
                timetable_table.ItemsSource = null;
                timetable_table.ItemsSource = formTicketShowDTO(loggedUser.reservations);
            }
        }

        
    }
}
