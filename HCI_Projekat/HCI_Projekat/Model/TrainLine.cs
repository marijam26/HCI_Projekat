using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Projekat.Model
{
    [Serializable]
    public class TrainLine : ICloneable
    {
        public int id { get; set; }
        public int price { get; set; }
        public List<Station> stations { get; set; }
        public List<int> time { get; set; }

        public Station from { get; set; }

        public Station to { get; set; }

        public TrainLine(TrainLine trainLine)
        {
            this.id = trainLine.id;
            this.price = trainLine.price;
            this.time = trainLine.time;
            this.stations = trainLine.stations;
            this.from = trainLine.from;
            this.to = trainLine.to;
        }

        public TrainLine(int id, int price) {
            this.id = id;
            this.price = price;
            this.stations = new List<Station>();
            this.time = new List<int>();
        }

        public object Clone()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                if (this.GetType().IsSerializable)
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, this);
                    stream.Position = 0;
                    return formatter.Deserialize(stream);
                }
                return null;
            }
        }
    }

    [Serializable]
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
