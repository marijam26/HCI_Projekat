using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Projekat.Model
{
    public class Ticket
    {

        public Timetable timetable { get; set; }
        public DateTime date { get; set; }
        public Wagon wagon { get; set; }
        public int seatPosition { get; set; }

        public Ticket() { }

        public Ticket(Timetable timetable,DateTime date,Wagon wagon,int seat) 
        {
            this.timetable = timetable;
            this.date = date;
            this.wagon = wagon;
            this.seatPosition = seat;
        }
}
}
