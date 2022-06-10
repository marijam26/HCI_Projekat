using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Projekat.Model
{
    public class Ticket
    {

        public List<Timetable> timetable { get; set; }
        public DateTime date { get; set; }
        public List<Wagon> wagon { get; set; }
        public List<int> seatPosition { get; set; }

        public Ticket() { }

        public Ticket(List<Timetable> timetable,DateTime date, List<Wagon> wagon, List<int> seat) 
        {
            this.timetable = timetable;
            this.date = date;
            this.wagon = wagon;
            this.seatPosition = seat;
        }
}
}
