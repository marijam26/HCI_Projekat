﻿using System;
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

            trains = new List<Train>();
            Train train1 = new Train(1, "Soko1", TrainRang.Soko, 100);
            trains.Add(train1);
            Train train2 = new Train(2, "Soko2", TrainRang.Soko, 200);
            trains.Add(train2);
            trains.Add(new Train(3, "Jadan voz", TrainRang.Obican, 100));
            trains.Add(new Train(4, "Jos jadniji voz", TrainRang.Obican, 50));
            train1.wagons.Add(new Wagon());


            addresses = new List<Address>();
            Address address1 = new Address(1,"Novi Sad","Srbija","Puskinova");
            Address address2 = new Address(1,"Novi Sad","Srbija","Mise Dimitrijevica");
            Address address3 = new Address(1,"Novi Sad","Srbija","Detelinara");
            addresses.Add(address1);
            addresses.Add(address2);
            addresses.Add(address3);

            Station s1 = new Station(1, "Beograd");
            Station s2 = new Station(2, "Nova Pazova");
            Station s3 = new Station(3, "Novi Sad");

         

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
            Timetable tt1 = new Timetable(1, tl1, new DateTime(2022,6,2,10,10,0), new DateTime(2022, 6, 2, 13, 10, 0), train1);
            Timetable tt2 = new Timetable(1, tl1, new DateTime(2022, 6, 2, 15, 0, 0), new DateTime(2022, 6, 2, 17, 10, 0), train2);
            Timetable tt3 = new Timetable(1, tl1, new DateTime(2022, 6, 3, 10, 10, 0), new DateTime(2022, 6, 3, 13, 10, 0), train1);
            Timetable tt4 = new Timetable(1, tl1, new DateTime(2022, 6, 4, 10, 10, 0), new DateTime(2022, 6, 4, 13, 10, 0), train1);

            Timetable tt5 = new Timetable(1, tl2, new DateTime(2022, 6, 2, 17, 10, 0), new DateTime(2022, 6, 2, 19, 10, 0), train2);
            Timetable tt6 = new Timetable(1, tl2, new DateTime(2022, 6, 3, 10, 10, 0), new DateTime(2022, 6, 3, 13, 10, 0), train2);
            this.timetables.Add(tt1);
            this.timetables.Add(tt2);
            this.timetables.Add(tt3);
            this.timetables.Add(tt4);
            this.timetables.Add(tt5);
            this.timetables.Add(tt6);

        }
    }
}
