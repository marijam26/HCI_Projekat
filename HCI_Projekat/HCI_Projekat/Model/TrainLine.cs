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
        public List<Station> stations { get; set; }
        public List<int> time { get; set; }

        public Station from { get; set; }

        public Station to { get; set; }

        public TrainLine() { }

        public TrainLine(int id, int price, Train train) {
            this.id = id;
            this.price = price;
            this.train = train;
            this.stations = new List<Station>();
            this.time = new List<int>();
        }
    }

    public class Station
    {
        public int id { get; set; }
        public string name { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }

        public Station(int id, string name, double latitude, double longitude)
        {
            this.id = id;
            this.name = name;
            this.longitude = longitude;
            this.latitude = latitude;
        }
        public Station() { }
    }
}
