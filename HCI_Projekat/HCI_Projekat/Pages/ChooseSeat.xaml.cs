﻿using HCI_Projekat.Model;
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
    /// Interaction logic for ChooseSeat.xaml
    /// </summary>
    public partial class ChooseSeat : Page
    {
        public TimetableDTO timetable { get; set; }
        public Button previousSelectedWagonBtn { get; set; }
        public Wagon previousSelectedWagon { get; set; }
        public int previousSelectedSeat { get; set; }
        public Button previousSelectedSeatBtn { get; set; }
        public User loggedUser { get; set; }
        public DateTime date { get; set; }
        public Data database { get; set; }
        public TicketDTO ticketDTO { get; set; }
        public String placeNum { get; set; }
        public String relation { get; set; }

        public ChooseSeat(Data database, TicketDTO ticketDTO,String placeNum)
        {
            InitializeComponent();
            this.database = database;
            this.date = date;
            this.ticketDTO = ticketDTO;
            this.loggedUser = ticketDTO.user;
            this.placeNum = placeNum;
            this.relation = formRelation();
            DataContext = this;
            if (placeNum == "first")
            {
                this.timetable = formTimetableDTO( ticketDTO.timetableDTO.timetables[0]);
            }
            else {
                this.timetable = formTimetableDTO(ticketDTO.timetableDTO.timetables[1]);
            }

            for (int i = 0; i < timetable.train.wagons.Count;i++) {
                wagonsGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            int wagonCounter = 0;
            foreach (Wagon w in timetable.train.wagons) {
                wagonCounter++;
                Button wagonBtn = new Button();
                wagonBtn.Click += new RoutedEventHandler(btn_wagon_Click);
                if (w.wagonClass == Wagon.WagonClass.first)
                {
                    wagonBtn.Background = (Brush)new BrushConverter().ConvertFrom("DarkCyan");
                }
                else {
                    wagonBtn.Background = (Brush)new BrushConverter().ConvertFrom("DarkGoldenrod");
                }
                wagonBtn.Width = 90;
                wagonBtn.Height = 30;
                wagonBtn.HorizontalAlignment = HorizontalAlignment.Center;
                wagonBtn.Content = "Wagon"+wagonCounter;
                Grid.SetRow(wagonBtn, 1);
                Grid.SetColumn(wagonBtn, wagonCounter-1);
                wagonsGrid.Children.Add(wagonBtn);

                if (ticketDTO.transferPlace != "" && placeNum == "first")
                {
                    btn_buy.Visibility = Visibility.Hidden;
                    btn_reserve.Visibility = Visibility.Hidden;
                    btn_next.Visibility = Visibility.Visible;
                }
                else {

                    btn_buy.Visibility = Visibility.Visible;
                    btn_reserve.Visibility = Visibility.Visible;
                    btn_next.Visibility = Visibility.Hidden;

                }
            }
                if (placeNum == "first" && ticketDTO.wagonBtns.Count !=0) {
                    previousSelectedWagonBtn = ticketDTO.wagonBtns[0];
                    previousSelectedSeatBtn = ticketDTO.seatBtns[0];
                previousSelectedWagon = ticketDTO.wagons[0];
                previousSelectedSeat = ticketDTO.seats[0];
                    btn_wagon_Click(ticketDTO.wagonBtns[0],new RoutedEventArgs());
                    btn_seat_Click(ticketDTO.seatBtns[0], new RoutedEventArgs());
                }
                if (placeNum == "second" && ticketDTO.wagonBtns.Count == 2)
                {
                    previousSelectedWagonBtn = ticketDTO.wagonBtns[1];
                    previousSelectedSeatBtn = ticketDTO.seatBtns[1];
                previousSelectedWagon = ticketDTO.wagons[1];
                previousSelectedSeat = ticketDTO.seats[1];
                btn_wagon_Click(ticketDTO.wagonBtns[1], new RoutedEventArgs());
                    btn_seat_Click(ticketDTO.seatBtns[1], new RoutedEventArgs());
                }
            
        }

        private string formRelation()
        {
            if (ticketDTO.transferPlace == "")
            {
                return ticketDTO.timetableDTO.startStation.ToUpper() + "-" + ticketDTO.timetableDTO.endStation.ToUpper();
            }
            else {
                if (placeNum == "first")
                {
                    return ticketDTO.timetableDTO.startStation.ToUpper() + "-" + ticketDTO.transferPlace.ToUpper();
                }
                else {
                    return ticketDTO.transferPlace.ToUpper() + "-" + ticketDTO.timetableDTO.endStation.ToUpper();
                }
            }
        }

        private TimetableDTO formTimetableDTO(Timetable timetable)
        {
            String day = timetable.isWeekday ? "Weekday" : "Weekend";
            TimetableDTO dto = new TimetableDTO(timetable.id, timetable.line, timetable.start, timetable.line.stations[0].name, timetable.line.stations[timetable.line.stations.Count-1].name, timetable.line.price, timetable.train, day, timetable.ValidFrom, timetable.ValidTo);
            return dto;
        }

        private void btn_wagon_Click(object sender, RoutedEventArgs e)
        {
            previousSelectedSeat = -1;
            seatGrid.ColumnDefinitions.Clear();
            seatGrid.RowDefinitions.Clear();
            seatGrid.Children.Clear();
            Button wagonBtn = sender as Button;
            if (previousSelectedWagonBtn != null) { 

                if (previousSelectedWagon.wagonClass == Wagon.WagonClass.first)
                    {
                    ((Button)wagonsGrid.Children[previousSelectedWagon.id - 1]).Background = (Brush)new BrushConverter().ConvertFrom("DarkCyan");
                    }
                    else
                    {
                    ((Button)wagonsGrid.Children[previousSelectedWagon.id - 1]).Background = (Brush)new BrushConverter().ConvertFrom("DarkGoldenrod");
                    }
            }

            Wagon wagon = timetable.train.wagons[Grid.GetColumn(wagonBtn)];
            if (wagon.wagonClass == Wagon.WagonClass.first)
            {
                ((Button)wagonsGrid.Children[wagon.id-1]).Background = (Brush)new BrushConverter().ConvertFrom("#FF3DD0D0");
            }
                else
            {
                ((Button)wagonsGrid.Children[wagon.id - 1]).Background = (Brush)new BrushConverter().ConvertFrom("#FFCAAF49");
            }

            previousSelectedWagonBtn = wagonBtn;
            previousSelectedWagon = wagon;

            for (int i = 0; i < 4; i++) {
                seatGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            double rowNum = wagon.capacity / 4;
            int rows = (int)rowNum;
            if (rowNum != rows)
            {
                rows += 1;
            }
            for (int i = 0; i < rows; i++) {
                seatGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (int i =0;i<wagon.capacity;i++) {
                int row = i / 4;
                int column = i % 4;
                Button seatBtn = new Button();
                seatBtn.Click += new RoutedEventHandler(btn_seat_Click);
                seatBtn.Margin = new Thickness(5);
                seatBtn.MinWidth = 40;
                seatBtn.MaxHeight = 40;
                if (wagon.seatAvailability[i] == false) {
                    seatBtn.IsEnabled = false;
                }
                seatBtn.Background = (Brush)new BrushConverter().ConvertFrom("DarkCyan");
                seatBtn.HorizontalAlignment = HorizontalAlignment.Center;
                seatBtn.Content = i;
                Grid.SetRow(seatBtn, row);
                Grid.SetColumn(seatBtn, column);
                seatGrid.Children.Add(seatBtn);
            }
        }

        private void btn_seat_Click(object sender, RoutedEventArgs e)
        {

            if (previousSelectedSeat != -1) {
                ((Button)seatGrid.Children[previousSelectedSeat]).Background = (Brush)new BrushConverter().ConvertFrom("DarkCyan");
            }
            Button seatBtn = sender as Button;
            int column = Grid.GetColumn(seatBtn);
            int row = Grid.GetRow(seatBtn);
            int position = row * 4 + column;
            previousSelectedSeat = position;
            previousSelectedSeatBtn = seatBtn;
            currentlySaveSeat();
            ((Button)seatGrid.Children[position]).Background = (Brush)new BrushConverter().ConvertFrom("#FF3DD0D0");
            


        }

            private void buy_ticket(object sender, RoutedEventArgs e)
        {
            if (previousSelectedSeat == -1) {
                MessageBox.Show("Must select your seat.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            currentlySaveSeat();
            int id = this.loggedUser.tickets.Max(x => x.id);
            Ticket ticket = new Ticket(id,ticketDTO.timetableDTO.timetables,ticketDTO.dateReserved,ticketDTO.wagons,ticketDTO.seats,ticketDTO.transferPlace,DateTime.Now);
            TicketShowDTO ticketShowDTO = new TicketShowDTO( ticketDTO,loggedUser,false);
            ReserveBuyConfirmDialog d = new ReserveBuyConfirmDialog(ticketShowDTO,ticket,loggedUser)
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            d.ShowDialog();
            MainWindow window = (MainWindow)Window.GetWindow(this);
            ChooseSeat r;
            if (ticketDTO.seats.Count == 2)
            {
                r = new ChooseSeat(this.database, ticketDTO, "second");
            }
            else {
                r = new ChooseSeat(this.database, ticketDTO, "first");
            }
            window.Content = r;
        }

        private Timetable formTimetablefromDTO(TimetableDTO timetable)
        {
            Boolean isWeekend;
            if (timetable.day == "Weekday") {
                isWeekend = false;
            }
            else {
                isWeekend = true;        
            }
            Timetable info = new Timetable(timetable.id,timetable.line,timetable.startDateTime,timetable.train,isWeekend,timetable.validFrom,timetable.validTo);
            return info;
        }

        private void reserve_ticket(object sender, RoutedEventArgs e)
        {
            if (previousSelectedSeat == -1)
            {
                MessageBox.Show("Must select your seat.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            currentlySaveSeat();
            int id = this.loggedUser.tickets.Max(x => x.id);
            Ticket ticket = new Ticket(id,ticketDTO.timetableDTO.timetables, ticketDTO.dateReserved, ticketDTO.wagons, ticketDTO.seats,ticketDTO.transferPlace,DateTime.Now);
            TicketShowDTO ticketShowDTO = new TicketShowDTO(ticketDTO, loggedUser, true);
            if ((date - DateTime.Now).TotalDays >= 5) {
                MessageBox.Show("Reservation is not possible more than 5 days before departure date.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            ReserveBuyConfirmDialog d = new ReserveBuyConfirmDialog(ticketShowDTO, ticket, loggedUser)
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            d.ShowDialog();
            MainWindow window = (MainWindow)Window.GetWindow(this);
            ChooseSeat r = new ChooseSeat(this.database,ticketDTO,"second");
            window.Content = r;


        }


        private void back_Btn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Window.GetWindow(this);
            if (placeNum == "first")
            {
                ReserveBuyTicket r = new ReserveBuyTicket(this.database, this.loggedUser);
                window.Content = r;
            }
            else {
                ChooseSeat r = new ChooseSeat(this.database, this.ticketDTO,"first");
                window.Content = r;

            }

        }
        
        private void next_btn_Click(object sender, RoutedEventArgs e)
        {
            if (previousSelectedSeat == -1)
            {
                MessageBox.Show("Must select your seat.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            MainWindow window = (MainWindow)Window.GetWindow(this);
            currentlySaveSeat();
            ChooseSeat r = new ChooseSeat(this.database, ticketDTO, "second");
            window.Content = r;

        }

        private void currentlySaveSeat() {
            if ((placeNum == "second" && ticketDTO.seats.Count == 1) || ticketDTO.seats.Count == 0)
            {
                ticketDTO.seats.Add(previousSelectedSeat);
                ticketDTO.wagons.Add(previousSelectedWagon);
                ticketDTO.wagonBtns.Add(previousSelectedWagonBtn);
                ticketDTO.seatBtns.Add(previousSelectedSeatBtn);
            }
            else if ((ticketDTO.transferPlace == "" && ticketDTO.seats.Count == 1) || (placeNum=="first") && ticketDTO.wagonBtns.Count>0)
            {
                ticketDTO.seats[0] = previousSelectedSeat;
                ticketDTO.wagons[0] = previousSelectedWagon;
                ticketDTO.wagonBtns[0] = previousSelectedWagonBtn;
                ticketDTO.seatBtns[0] = previousSelectedSeatBtn;
            }
            else if (ticketDTO.transferPlace != "" && ticketDTO.seats.Count == 2)
            {
                ticketDTO.seats[1] = previousSelectedSeat;
                ticketDTO.wagons[1] = previousSelectedWagon;
                ticketDTO.wagonBtns[1] = previousSelectedWagonBtn;
                ticketDTO.seatBtns[1] = previousSelectedSeatBtn;
            }
        }
    }

    
}
