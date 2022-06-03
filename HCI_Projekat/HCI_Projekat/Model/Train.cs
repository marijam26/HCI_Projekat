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

        public List<Wagon> wagons { get; set; }

        public Train() { }

        public Train(int id, string name, TrainRang rang, int capacity)
        {
            this.id = id;
            this.name = name;
            this.rang = rang;
            this.capacity = capacity;
            this.wagons = new List<Wagon>();
        }


    }

    public enum TrainRang
    {
        Soko,
        Obican
    }

    public class Wagon
    {
        public int id { get; set; }

        public int classNum;

        public List<Seat> seats;

        public Wagon() { }

        public Wagon(int id,int classNum)
        {
            this.id = id;
            this.classNum = classNum;
            this.seats = new List<Seat>();
        }
    }

    public class Seat
    {
        public int id { get; set; }

        public int seatNum;

        public bool isAvailable;

        public Seat() { }
        public Seat(int id,int seatNum, bool isAvailable)
        {
            this.id=id;
            this.seatNum = seatNum;
            this.isAvailable = true;
        }
    }
}
