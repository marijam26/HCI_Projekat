using HCI_Projekat.Model;
using HCI_Projekat.touring;
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
using ThinkSharp.FeatureTouring.Navigation;

namespace HCI_Projekat.Pages
{
    


    public partial class ReserveBuyConfirmDialog : Window
    {
        public TicketShowDTO ticketShowDTO { get; set; }
        public Ticket ticket { get; set; }
        public User loggedUser { get; set; }

        public bool tour = false;
        
        public ReserveBuyConfirmDialog(TicketShowDTO ticketDTO,Ticket ticket,User loggedUser, bool tour = false)
        {
            InitializeComponent();
            Uri uri = new Uri("../../Images/icon.png", UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(uri);
            this.ticketShowDTO = ticketDTO;
            this.ticket = ticket;
            this.loggedUser = loggedUser;
            if (ticketDTO.isReservation)
            {
                titleLabel.Content = "Confirm reservation";
            }
            else {
                titleLabel.Content = "Confirm purchase";
            }
            DataContext = this;

            this.tour = tour;

        }

        private void btn_confirm_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < ticket.seatPosition.Count; i++){
                ticket.timetable[i].
                    train.wagons[ticket.wagon[i].id - 1]
                    .seatAvailability[ticket.seatPosition[i]] = false; 
            }
            if (ticketShowDTO.isReservation)
            {
                loggedUser.reservations.Add(ticket);
                MessageBox.Show("You have successfully reserved your ticket.", "Serbian Raliways", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                loggedUser.tickets.Add(ticket);
                MessageBox.Show("You have successfully bought your ticket.", "Serbian Raliways", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            Close();
        }

        public void ContinueTour()
        {
            var navigator = FeatureTour.GetNavigator();
            navigator.IfCurrentStepEquals(ElementID.BuyTicketButton).GoNext();
            navigator.OnStepEntered(ElementID.ConfirmPurchase).Execute(s => btn_confirm.Focus());
            btn_confirm.Click += btnConfirmClick;
            btn_cancel.IsEnabled = false;
        }


        private void btnConfirmClick(object sender, RoutedEventArgs e)
        {
            var navigator = FeatureTour.GetNavigator();
            navigator.IfCurrentStepEquals(ElementID.ConfirmPurchase).Close();
        }

        public void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

    public class TicketShowDTO
    {
        public int id { get; set; }
        public String userName { get; set; }
        public String userSurname { get; set; }
        public String startTime { get; set; }
        public String startDateTime { get; set; }
        public String startStation { get; set; }
        public String endStation { get; set; }
        public int price { get; set; }
        public String trainName { get; set; }
        public String seat { get; set; }
        public String wagon { get; set; }
        public Boolean isReservation { get; set; }
        public string transferPlace { get; set; }
        public string purchasementDate { get; set; }
        public TicketShowDTO() { }


        public TicketShowDTO(TimetableDTO timetable, Ticket ticket, User loggedUser,Boolean isReservation)
        {
            this.userName = loggedUser.name;
            this.userSurname = loggedUser.surname;
            this.endStation = timetable.endStation;
            this.startStation = timetable.startStation;
            this.startDateTime = ticket.date.ToString().Split(' ')[0]+" "+timetable.startTime;
            this.startTime = timetable.startTime;
            this.trainName = timetable.train.name;
            this.wagon = (ticket.wagon[0].id).ToString();
            this.seat = ticket.seatPosition[0].ToString();
            if (ticket.wagon.Count > 1)
            {
                this.wagon += "/" + (ticket.wagon[1].id).ToString();
                this.seat += "/" + (ticket.seatPosition[1]).ToString();
            }
            this.isReservation = isReservation;

        }

        public TicketShowDTO(TicketDTO ticketDTO, User loggedUser, Boolean isReservation)
        {
            this.userName = loggedUser.name;
            this.userSurname = loggedUser.surname;
            this.startStation = ticketDTO.timetableDTO.startStation;
            this.endStation = ticketDTO.timetableDTO.endStation;
            this.startDateTime = ticketDTO.dateReserved.ToString().Split(' ')[0] + " " + ticketDTO.timetableDTO.startTime;
            this.startTime = ticketDTO.timetableDTO.startTime;
            this.trainName = ticketDTO.timetableDTO.train.name; //mozda i za voz
            this.wagon = (ticketDTO.wagons[0].id).ToString();
            this.seat = ticketDTO.seats[0].ToString();
            if (ticketDTO.wagons.Count > 1) {
                this.wagon += "/"+(ticketDTO.wagons[1].id).ToString();
                this.seat += "/"+(ticketDTO.seats[1]).ToString();
            }
            this.isReservation = isReservation;

        }

        public TicketShowDTO(Ticket ticket) {
            this.startStation = ticket.timetable[0].line.stations[0].name;
            if (ticket.timetable.Count == 1)
            {
                this.endStation = ticket.timetable[0].line.stations[ticket.timetable[0].line.stations.Count-1].name;
            }
            else {
                this.endStation = ticket.timetable[1].line.stations[ticket.timetable[1].line.stations.Count - 1].name;
            }
            if (ticket.transferStation != "")
            {
                this.transferPlace = ticket.transferStation;
            }
            else {
                this.transferPlace = "-";
            }
            this.wagon = ticket.wagon[0].id.ToString();
            this.seat = ticket.seatPosition[0].ToString();
            if (ticket.wagon.Count > 1)
            {
                this.wagon += "/" + (ticket.wagon[1].id).ToString();
                this.seat += "/" + (ticket.seatPosition[1]).ToString();
            }
            this.startDateTime = new DateTime(ticket.date.Year, ticket.date.Month, ticket.date.Day, ticket.timetable[0].start.Hour, ticket.timetable[0].start.Minute, ticket.timetable[0].start.Second).ToString();
            if (ticket.wagon.Count > 1)
            {
                this.price = (ticket.timetable[0].line.price + ticket.timetable[1].line.price);
            }
            else {
                this.price = ticket.timetable[0].line.price;
            }
            this.purchasementDate = ticket.purchasementDate.ToString();
            this.id = ticket.id;
        }
        
        public TicketShowDTO(Ticket ticket,User user) {
            this.userName = user.name;
            this.userSurname = user.surname;
            this.startStation = ticket.timetable[0].line.stations[0].name;
            if (ticket.timetable.Count == 1)
            {
                this.endStation = ticket.timetable[0].line.stations[ticket.timetable[0].line.stations.Count-1].name;
            }
            else {
                this.endStation = ticket.timetable[1].line.stations[ticket.timetable[1].line.stations.Count - 1].name;
            }
            if (ticket.transferStation != "")
            {
                this.transferPlace = ticket.transferStation;
            }
            else {
                this.transferPlace = "-";
            }
            this.wagon = ticket.wagon[0].id.ToString();
            this.seat = ticket.seatPosition[0].ToString();
            if (ticket.wagon.Count > 1)
            {
                this.wagon += "/" + (ticket.wagon[1].id).ToString();
                this.seat += "/" + (ticket.seatPosition[1]).ToString();
            }
            this.startDateTime = new DateTime(ticket.date.Year, ticket.date.Month, ticket.date.Day, ticket.timetable[0].start.Hour, ticket.timetable[0].start.Minute, ticket.timetable[0].start.Second).ToString();
                                 //ticket.date.ToString().Split(' ')[0] + " " + ticket.timetable[0].start.ToString().Split(' ')[1];
            if (ticket.wagon.Count > 1)
            {
                this.price = (ticket.timetable[0].line.price + ticket.timetable[1].line.price);
            }
            else {
                this.price = ticket.timetable[0].line.price;
            }
            this.purchasementDate = ticket.purchasementDate.ToString();
            this.id = ticket.id;
        }

    }

}
