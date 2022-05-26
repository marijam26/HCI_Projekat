using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Projekat.Model
{
    public class Train
    {
        public int id { get; set; }
        public string name { get; set; }
        public TrainRang rang { get; set; }

        public int capacity { get; set; }

        public Train() { }

        public Train(int id, string name, TrainRang rang, int capacity)
        {
            this.id = id;
            this.name = name;
            this.rang = rang;
            this.capacity = capacity;
        }


    }

    public enum TrainRang
    {
        Soko,
        Obican
    }
}
