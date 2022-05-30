using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Projekat.Model
{
    public class TrainLine
    {
        public int id { get; set; }
        public int price { get; set; }
        public Train train { get; set; }
        public List<Address> stations { get; set; }

        public TrainLine() { }

        public TrainLine(int id, int price, Train train, List<Address> stations) {
            this.id = id;
            this.price = price;
            this.train = train;
            this.stations = stations;
        }
    }
}
