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
    public class Train : ICloneable
    {
        public int id { get; set; }
        public string name { get; set; }
        public Rang rang { get; set; }
        public List<Wagon> wagons { get; set; }

        public Train()
        {
            this.wagons = new List<Wagon> { new Wagon(1, Wagon.WagonClass.first, 10) };
        }


        public Train(int id, string name, Rang rang, List<Wagon> wagons)
        {
            this.id = id;
            this.name = name;
            this.rang = rang;
            this.wagons = wagons;

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
    public enum Rang
    {
        Soko,
        obicni
    }

}