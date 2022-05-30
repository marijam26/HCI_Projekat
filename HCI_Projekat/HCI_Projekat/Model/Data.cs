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

            addresses = new List<Address>();
            Address address1 = new Address(1,"Novi Sad","Srbija","Puskinova");
            Address address2 = new Address(1,"Novi Sad","Srbija","Mise Dimitrijevica");
            Address address3 = new Address(1,"Novi Sad","Srbija","Detelinara");
            addresses.Add(address1);
            addresses.Add(address2);
            addresses.Add(address3);

            List<Address> stations1 = new List<Address>();
            List<Address> stations2 = new List<Address>();
            stations2.Add(address3);
            stations2.Add(address1);
            stations1.Add(address3);
            stations1.Add(address2);

            trainLines = new List<TrainLine>();
            trainLines.Add(new TrainLine(1,200,train1,stations1));
            trainLines.Add(new TrainLine(2, 150, train2, stations2));

        }
    }
}
