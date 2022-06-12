using HCI_Projekat.help;
using HCI_Projekat.Model;
using System;
using System.Collections;
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
    /// Interaction logic for ReserveBuyTicket.xaml
    /// </summary>
    public partial class ReserveBuyTicket : Page
    {
        public Data dataBase { get; set; }
        public User loggedUser { get; set; }
        String transferPlace = "";

        public ReserveBuyTicket(Data database, User user)
        {
            InitializeComponent();
            this.dataBase = database;
            loggedUser = user;
            fromPlace.ItemsSource = getStations();
            toPlace.ItemsSource = getStations();
            btn_chooseSeat.IsEnabled = false;
        }

        private IEnumerable getStations()
        {
            List<string> stations = new List<string>();
            foreach (TrainLine line in dataBase.trainLines) {
                foreach (Station place in line.stations)
                {
                    var elements = stations.Where(x => x.ToLower() == place.name.ToLower());
                    if (elements.Count() ==0)
                    {
                        stations.Add(place.name);
                    }
                }
            }
            return stations;
        }

        private void timetable_table_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {



        }

        
      private void btn_chooseSeat_Click(object sender, RoutedEventArgs e)
        {
            int selectedCells = timetable_table.SelectedCells.Count();
            if (selectedCells == 0)
            {
                MessageBox.Show("You must select the timetable.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            TimetableDTO t = (TimetableDTO)timetable_table.SelectedItem;
            DateTime startDate = new DateTime();
            if (startDatePick.SelectedDate != null) { 
                startDate = (DateTime)startDatePick.SelectedDate;
            }

            TicketDTO ticketInfo = new TicketDTO(loggedUser, transferPlace, startDate);
            ticketInfo.timetableDTO = findTimetable(t,startDate);
          
            foreach (Timetable table in t.timetables) {
                ticketInfo.trains.Add(table.train);
            }
            ChooseSeat r = new ChooseSeat(dataBase, ticketInfo ,"first");
            ClientHomepage window = (ClientHomepage)Window.GetWindow(this);
            window.clientHomepage.Navigate(r);

        }

        private TimetableDTO findTimetable(TimetableDTO t, DateTime startDate)
        {
            List<Timetable> tables = new List<Timetable>();
            foreach (Timetable timetable in t.timetables) {
                tables.Add(cloneOrFindTimetable(timetable,startDate));
            }
            t.timetables = tables;
            return t;
        }

        private Timetable cloneOrFindTimetable(Timetable timetable, DateTime startDate)
        {
            foreach (User u in dataBase.users) {
                foreach (Ticket ticket in u.tickets) {
                    foreach (Timetable t in ticket.timetable) {
                        if (t.id == timetable.id && startDate.Equals(ticket.date.Date)) {
                            return t;
                        }
                    }
                }
            }
            return (Timetable)timetable.Clone();
        }

        private void searchStations(object sender, RoutedEventArgs e)
        {
            String start = fromPlace.Text.Trim().ToLower();
            String end = toPlace.Text.Trim().ToLower();
            DateTime? date = startDatePick.SelectedDate;
            if (date == null) { 
                MessageBox.Show("You must enter the date of departure.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DateTime startDate = (DateTime)date;
            if (startDate < DateTime.Now) {
                MessageBox.Show("Departure date can't be in the past.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;

            }
            if (start == "") { 
                MessageBox.Show("You must choose a start station.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            if (end == "") { 
                MessageBox.Show("You must choose an end station.", "Invalid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            List<TimetableDTO> timetables = new List<TimetableDTO>();

            foreach (Timetable timetable in dataBase.timetables) { 
                if (timetable.line.stations[0].name.ToLower() == start && timetable.line.stations[timetable.line.stations.Count-1].name.ToLower() == end && DateTime.Now > timetable.ValidFrom && DateTime.Now<timetable.ValidTo) {

                    if ( ((startDate.DayOfWeek == DayOfWeek.Saturday || startDate.DayOfWeek == DayOfWeek.Sunday) && !timetable.isWeekday) || ((startDate.DayOfWeek != DayOfWeek.Saturday && startDate.DayOfWeek != DayOfWeek.Sunday) && timetable.isWeekday)) { 

                    String day = timetable.isWeekday ? "Weekday" : "Weekend";
                    TimetableDTO dto = new TimetableDTO(timetable.id, timetable.line, timetable.start, timetable.line.stations[0].name, timetable.line.stations[timetable.line.stations.Count() - 1].name, timetable.line.price, timetable.train, day, timetable.ValidFrom, timetable.ValidTo);
                    dto.stationInfo = createStationInfo(timetable);
                    dto.timetables.Add(timetable);
                        timetables.Add(dto);
                    }
                }
            }
            if (timetables.Count == 0) {
                timetables = findTimetableWithTransfer(start,end,timetables);
            }

            if (timetables.Count() == 0)
            {
                btn_chooseSeat.IsEnabled = false;
            }
            else {
                btn_chooseSeat.IsEnabled = true;
            }
            timetable_table.ItemsSource = null;
            timetable_table.ItemsSource = timetables;

        }

        private List<TimetableDTO> findTimetableWithTransfer(String start,String end, List<TimetableDTO> timetables) 
        {
            foreach (Timetable timetable in dataBase.timetables)
            {
                if (timetable.line.stations[0].name.ToLower() == start && DateTime.Now > timetable.ValidFrom && DateTime.Now < timetable.ValidTo)
                {
                    foreach (Timetable timetable2 in dataBase.timetables)
                    {
                        var listCommon = timetable.line.stations
                                        .Select(a => a.name)
                                        .Intersect(timetable2.line.stations.Select(b => b.name))
                                        .Distinct();

                        if (timetable2.line.stations[timetable2.line.stations.Count - 1].name.ToLower() == end && listCommon.Count() != 0 && DateTime.Now > timetable2.ValidFrom && DateTime.Now < timetable2.ValidTo)
                        {
                            List<DateTime> departuresFirstTrain = calculateDepartureTimes(timetable);
                            List<DateTime> departuresSecondTrain = calculateDepartureTimes(timetable2);
                            transferPlace = findTransferPlace(departuresFirstTrain, departuresSecondTrain, timetable, timetable2);
                            if (transferPlace != "")
                            {
                                String info = createStationInfoTransfer(timetable, timetable2, departuresFirstTrain, departuresSecondTrain, transferPlace);
                                String day = timetable.isWeekday ? "Weekday" : "Weekend";
                                TimetableDTO dto = new TimetableDTO(timetable.id, timetable.line, timetable.start, start, end, timetable.line.price + timetable2.line.price, timetable.train, day, timetable.ValidFrom, timetable.ValidTo);
                                dto.stationInfo = info;
                                dto.timetables.Add(timetable);
                                dto.timetables.Add(timetable2);
                                timetables.Add(dto);
                            }

                        }

                    }
                }
            }
            return timetables;
        }

        private List<DateTime> calculateDepartureTimes(Timetable timetable)
        {
            DateTime startDepartureTime = timetable.start;
            List<DateTime> departureTimes = new List<DateTime>();
            foreach (int travelTime in timetable.line.time) {
                departureTimes.Add(startDepartureTime);
                startDepartureTime = startDepartureTime.AddMinutes(travelTime);
            }
            return departureTimes;
        }

        private string findTransferPlace(List<DateTime> departuresFirstTrain, List<DateTime> departuresSecondTrain, Timetable timetable, Timetable timetable2)
        {
            String departure = "";
            for (int i = 0; i < timetable.line.stations.Count; i++) {
                for (int j = 0; j < timetable2.line.stations.Count; j++) {
                    if (timetable.line.stations[i].name == timetable2.line.stations[j].name && departuresFirstTrain[i] < departuresSecondTrain[j])
                    {
                        departure = timetable.line.stations[i].name;
                    }
                }
                
            }
            return departure;

        }

        private string createStationInfoTransfer(Timetable timetable, Timetable timetable2, List<DateTime> departuresFirstTrain, List<DateTime> departuresSecondTrain, String transferPlace )
        {
            String stringBuilder = "";
            stringBuilder +="\tFirst train".PadRight(80)+"Second train".PadRight(50)+ "\n\n";
            stringBuilder += "       Station".PadRight(30)+"Departure time".PadRight(42)+"Station".PadRight(20)+"Departure time".PadRight(30)+"Transfer".PadRight(20)+"\n\n";
            String time = departuresFirstTrain[0].ToString().Split(' ')[1].Substring(0, 5);
            stringBuilder += ("      " + timetable.line.stations[0].name).PadRight(43 - timetable.line.stations[0].name.Length) + time.PadRight(40);
            time = departuresSecondTrain[0].ToString().Split(' ')[1].Substring(0, 5);
            stringBuilder += timetable2.line.stations[0].name.PadRight(32- timetable2.line.stations[0].name.Length) +time.PadRight(40);
            stringBuilder += transferPlace.PadRight(20)+"\n";
            for (int i = 1; i < timetable.line.stations.Count; i++)
            {
                time = departuresFirstTrain[i].ToString().Split(' ')[1].Substring(0, 5);
                stringBuilder += ("      " + timetable.line.stations[i].name.Trim()).PadRight(45- timetable.line.stations[i].name.Length);
                stringBuilder += time.PadRight(40);
                if (timetable2.line.stations.Count > i) {
                    time = departuresSecondTrain[i].ToString().Split(' ')[1].Substring(0, 5);
                    if (time.EndsWith(":"))
                    {
                        time = " "+time.Substring(0, 4);
                    }
                    stringBuilder += (timetable2.line.stations[i].name).PadRight(33 - timetable2.line.stations[i].name.Length) + time.PadRight(30)+"\n";
                }
                else {
                    stringBuilder += ("".PadRight(30) + "".PadRight(30)+"\n");

                }
            }
            return stringBuilder;

        }

        private string createStationInfo(Timetable timetable)
        {
            String stringBuilder = "";
            DateTime date = timetable.start;
            stringBuilder+="Station".PadRight(55)+"Departure time" + "\n\n";
            for (int i = 0; i < timetable.line.stations.Count; i++) {
                String time = date.ToString().Split(' ')[1].Substring(0,5);
                stringBuilder+=timetable.line.stations[i].name.PadRight(68- timetable.line.stations[i].name.Length) + time+"\n";
                if(i < timetable.line.stations.Count - 1)
                {
                    date = date.AddMinutes(timetable.line.time[i]);
                }
                
            }
            return stringBuilder.ToString();
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                //string str = HelpProvider.GetHelpKey((DependencyObject)focusedControl);
                HelpProvider.ShowHelp("reserveBuy", (ClientHomepage)Window.GetWindow(this));
            }
        }
    }


    public class TimetableDTO { 

        public int id { get; set; }

        public String startTime {get; set;}
        public DateTime startDateTime { get; set; }

        public String startStation { get; set; }
        public String endStation { get; set; }

        public List<Timetable> timetables { get; set; }  

        public int price { get; set; }
        public Train train { get; set; }
        public TrainLine line { get; set; }
        public String day { get; set; }    // weekday ili weekend
        public DateTime validFrom { get; set; }
        public DateTime validTo { get; set; }
        public String stationInfo { get; set; }

        public TimetableDTO() { }

        public TimetableDTO( DateTime startDateTime, String startStation, String endStation, int price, Train train, string day, DateTime validFrom, DateTime validTo)
        {
            this.startDateTime = startDateTime;
            this.endStation = endStation;
            this.startStation = startStation;
            this.price = price;
            this.train = train;
            this.day = day;
            this.validFrom = validFrom;
            this.validTo = validTo;
            timetables = new List<Timetable>();
            startTime = startDateTime.ToString("HH:mm");
        }

        public TimetableDTO(int id,TrainLine line, DateTime startDateTime, String startStation, String endStation, int price,Train train, string day, DateTime validFrom, DateTime validTo)
        {
            this.id = id;
            this.line = line;
            this.startDateTime = startDateTime;
            this.endStation = endStation;
            this.startStation = startStation;
            this.price = price;
            this.train = train;
            this.day = day;
            this.validFrom = validFrom;
            this.validTo = validTo;
            timetables = new List<Timetable>();
            startTime = startDateTime.ToString("HH:mm");
        }
    }


    public class TicketDTO {

        public TimetableDTO timetableDTO { get; set; }
        public List<Wagon> wagons { get; set; }
        public List<int> seats { get; set; }
        public List<Button> wagonBtns { get; set; }
        public List<Button> seatBtns { get; set; }
        public List<Train> trains { get; set; }
        public User user { get; set; }
        public String transferPlace { get; set; }
        public DateTime dateReserved { get; set; }


        public TicketDTO() { }

        public TicketDTO(User user,String place,DateTime dateReserved) {
            this.user = user;
            this.transferPlace = place;
            this.dateReserved = dateReserved;
            wagons = new List<Wagon>();
            seats = new List<int>();
            trains = new List<Train>();
            wagonBtns = new List<Button>();
            seatBtns = new List<Button>();

        }
    
    }
}
