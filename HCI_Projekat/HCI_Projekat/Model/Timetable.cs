using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Projekat.Model
{
    internal class Timetable
    {
        public int id { get; set; }

        public TrainLine line { get; set; }

        public DateTime startDateTime { get; set; }

        public DateTime endDateTime { get; set; }


        public Timetable() { }

        public Timetable(int id, TrainLine line, DateTime startDateTime, DateTime endDateTime)
        {
            this.id = id;
            this.line = line;
            this.startDateTime = startDateTime;
            this.endDateTime = endDateTime;
        }
    }
}
