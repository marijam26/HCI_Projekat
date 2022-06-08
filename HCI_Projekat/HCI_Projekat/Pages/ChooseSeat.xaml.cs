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
    /// Interaction logic for ChooseSeat.xaml
    /// </summary>
    public partial class ChooseSeat : Page
    {
        public Data database { get; set; }
        public TimetableDTO timetable { get; set; }
        public Button previousSelectedWagonBtn { get; set; }
        public Wagon previousSelectedWagon { get; set; }
        public int previousSelectedSeat { get; set; }
        public Button previousSelectedSeatBtn { get; set; }
        public User loggedUser { get; set; }
        public DateTime date { get; set; }

        public ChooseSeat(Data database,TimetableDTO timetable, User user,DateTime date)
        {
            InitializeComponent();
            this.database = database;
            this.timetable = timetable;
            this.loggedUser = user;
            this.date = date;

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
            }
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
                    previousSelectedWagonBtn.Background = (Brush)new BrushConverter().ConvertFrom("DarkCyan");
                    }
                    else
                    {
                    previousSelectedWagonBtn.Background = (Brush)new BrushConverter().ConvertFrom("DarkGoldenrod");
                    }
            }

            Wagon wagon = timetable.train.wagons[Grid.GetColumn(wagonBtn)];
            if (wagon.wagonClass == Wagon.WagonClass.first)
            {
                wagonBtn.Background = (Brush)new BrushConverter().ConvertFrom("#FF3DD0D0");
            }
                else
            {
                wagonBtn.Background = (Brush)new BrushConverter().ConvertFrom("#FFCAAF49");
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
                previousSelectedSeatBtn.Background = (Brush)new BrushConverter().ConvertFrom("DarkCyan");
            }
            Button seatBtn = sender as Button;
            int column = Grid.GetColumn(seatBtn);
            int row = Grid.GetRow(seatBtn);
            int position = row * 4 + column;
            previousSelectedSeat = position;
            previousSelectedSeatBtn = seatBtn;
            seatBtn.Background = (Brush)new BrushConverter().ConvertFrom("#FF3DD0D0");
            


        }

            private void buy_ticket(object sender, RoutedEventArgs e)
        {
            if (previousSelectedSeat == -1) {
                MessageBox.Show("Must select your seat.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Timetable table = formTimetablefromDTO(this.timetable);
            Ticket ticket = new Ticket(table, date, this.previousSelectedWagon, this.previousSelectedSeat);
            TicketDTO ticketDTO = new TicketDTO(timetable, ticket,loggedUser,false);
            ReserveBuyConfirmDialog d = new ReserveBuyConfirmDialog(ticketDTO,ticket,loggedUser)
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            d.ShowDialog();
            MainWindow window = (MainWindow)Window.GetWindow(this);
            ChooseSeat r = new ChooseSeat(this.database, this.timetable, this.loggedUser, this.date);
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

            Timetable table = formTimetablefromDTO(this.timetable);
            Ticket ticket = new Ticket(table, date, this.previousSelectedWagon, this.previousSelectedSeat);
            TicketDTO ticketDTO = new TicketDTO(timetable, ticket, loggedUser, true);
            if ((date - DateTime.Now).TotalDays >= 5) {
                MessageBox.Show("Reservation is not possible more than 5 days before departure date.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            ReserveBuyConfirmDialog d = new ReserveBuyConfirmDialog(ticketDTO, ticket, loggedUser)
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            d.ShowDialog();
            MainWindow window = (MainWindow)Window.GetWindow(this);
            ChooseSeat r = new ChooseSeat(this.database,this.timetable, this.loggedUser,this.date);
            window.Content = r;


        }


        private void back_Btn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Window.GetWindow(this);
            ReserveBuyTicket r = new ReserveBuyTicket(this.database,this.loggedUser);
            window.Content = r;

        }
    }

    
}
