using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Projekat.Model
{
    public class Data
    {
        public List<User> users { get; set; }
        public User currentUser { get; set; }
        public List<Address> addresses { get; set; }
        public List<TrainLine> trainLines { get; set; }
        public List<Train> trains { get; set; }

        public List<Timetable> timetables { get; set; }

        public Data()
        {
            users = new List<User>();
            User u1 = new User("maki", "m", "Marija", "Milosevic", UserType.Manager);
            users.Add(u1);
            users.Add(new User("coki", "c", "Coka", "Cokic", UserType.Client));

            List<Wagon> wagons1 = new List<Wagon>();
            wagons1.Add(new Wagon(1,Wagon.WagonClass.first,16));
            wagons1.Add(new Wagon(2, Wagon.WagonClass.second, 24));
            wagons1.Add(new Wagon(3, Wagon.WagonClass.first, 16));
            List<Wagon> wagons2 = new List<Wagon>();
            wagons2.Add(new Wagon(4, Wagon.WagonClass.first, 17));
            wagons2.Add(new Wagon(5, Wagon.WagonClass.second, 8));
            wagons2.Add(new Wagon(6, Wagon.WagonClass.first, 24));

            trains = new List<Train>();
            Train train1 = new Train(1, "Soko1", Rang.Soko ,wagons1);
            trains.Add(train1);
            Train train2 = new Train(2, "Soko2",Rang.obicni, wagons2);
            trains.Add(train2);
            trains.Add(new Train(3, "Jadan voz", Rang.Soko, new List<Wagon> { new Wagon(3, Wagon.WagonClass.first, 15)}));
            trains.Add(new Train(4, "Jos jadniji voz", Rang.Soko, new List<Wagon> { new Wagon(3, Wagon.WagonClass.second, 15) }));

            addresses = new List<Address>();
            Address address1 = new Address(1,"Novi Sad","Srbija","Puskinova");
            Address address2 = new Address(1,"Novi Sad","Srbija","Mise Dimitrijevica");
            Address address3 = new Address(1,"Novi Sad","Srbija","Detelinara");
            addresses.Add(address1);
            addresses.Add(address2);
            addresses.Add(address3);

            Station s1 = new Station(1, "Beograd", 44.818109449391756, 20.465894172199075);
            Station s2 = new Station(2, "Nova Pazova",44.95983320985867, 20.23084652428375);
            Station s3 = new Station(3, "Novi Sad", 45.27167160883285, 19.8209432003093);

         

            trainLines = new List<TrainLine>();
            TrainLine tl1 = new TrainLine(1, 200, train1);
            TrainLine tl2 = new TrainLine(2, 150, train2);
            tl1.stations.Add(s1);
            tl1.stations.Add(s2);
            tl1.stations.Add(s3);
            tl2.stations.Add(s3);
            tl2.stations.Add(s2);
            tl1.from = s1;
            tl1.to = s3;
            tl2.from = s3;
            tl2.to = s2;
            tl1.time.Add(30);
            tl1.time.Add(20);
            tl1.time.Add(50);
            tl2.time.Add(30);
            tl2.time.Add(40);

            trainLines.Add(tl1);
            trainLines.Add(tl2);


            this.timetables = new List<Timetable>();
            Timetable tt1 = new Timetable(1, tl1, new DateTime(2022,6,2,10,10,0), train1, true, new DateTime(2022, 6, 2, 13, 10, 0), new DateTime(2022, 6, 30, 13, 10, 0));     // valid
            Timetable tt2 = new Timetable(2, tl1, new DateTime(2022, 6, 2, 15, 0, 0), train2, false, new DateTime(2022, 6, 2, 17, 10, 0), new DateTime(2022, 6, 30, 17, 10, 0));  // valid
            Timetable tt3 = new Timetable(3, tl1, new DateTime(2022, 6, 3, 16, 10, 0), train1, true, new DateTime(2022, 6, 15, 13, 10, 0), new DateTime(2022, 6, 30, 17, 10, 0));  // nije jos validan
            Timetable tt4 = new Timetable(4, tl1, new DateTime(2022, 6, 4, 17, 10, 0), train1, false, new DateTime(2022, 6, 4, 13, 10, 0), new DateTime(2022, 6, 6, 17, 10, 0));   // nije validan

            Timetable tt5 = new Timetable(5, tl2, new DateTime(2022, 6, 2, 17, 10, 0), train2, true,  new DateTime(2022, 6, 2, 19, 10, 0), new DateTime(2022, 6, 30, 17, 10, 0));
            Timetable tt6 = new Timetable(6, tl2, new DateTime(2022, 6, 3, 10, 10, 0), train2, false, new DateTime(2022, 6, 3, 13, 10, 0), new DateTime(2022, 6, 30, 17, 10, 0));
            this.timetables.Add(tt1);
            this.timetables.Add(tt2);
            this.timetables.Add(tt3);
            this.timetables.Add(tt4);
            this.timetables.Add(tt5);
            this.timetables.Add(tt6);

        }
    }
}
