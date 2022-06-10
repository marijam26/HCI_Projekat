using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Projekat.Model
{
    public class Ticket
    {

        public int id { get; set; }
        public List<Timetable> timetable { get; set; }
        public DateTime date { get; set; }
        public List<Wagon> wagon { get; set; }
        public List<int> seatPosition { get; set; }
        public string transferStation { get; set; }
        public DateTime purchasementDate { get; set; }

        public Ticket() { }

        public Ticket(int id,List<Timetable> timetable,DateTime date, List<Wagon> wagon, List<int> seat,string transfer,DateTime purchasementDate) 
        {
            this.id = id;
            this.timetable = timetable;
            this.date = date;
            this.wagon = wagon;
            this.seatPosition = seat;
            this.transferStation = transfer;
            this.purchasementDate = purchasementDate;
        }
}
}
