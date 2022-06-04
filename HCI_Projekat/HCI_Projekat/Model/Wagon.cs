using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace HCI_Projekat.Model
{
    public class Wagon
    {
        public int id { get; set; }
        public WagonClass wagonClass { get; set; }
        public int capacity { get; set; }
        public System.Collections.Generic.List<bool> seatAvailability { get; set; }

        public Wagon() { }

        public Wagon(int id, WagonClass rang, int capacity)
        {
            this.id = id;
            this.wagonClass = rang;
            this.capacity = capacity;
            seatAvailability = new List<bool>();
            for (int i = 0; i < capacity; i++) {
                seatAvailability.Add(true);
            }
        }
        
        public Wagon(int id, WagonClass rang, int capacity, System.Collections.Generic.List<bool> availability)
        {
            this.id = id;
            this.wagonClass = rang;
            this.capacity = capacity;
            seatAvailability = availability;
        }

        public enum WagonClass { 
            first,
            second
        }
    }
}
