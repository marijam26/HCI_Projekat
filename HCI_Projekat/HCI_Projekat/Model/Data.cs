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

        public List<Train> trains { get; set; }

        public Data()
        {
            users = new List<User>();
            users.Add(new User("maki","m","Marija","Milosevic",UserType.Manager));
            users.Add(new User("coki", "c", "Coka", "Cokic", UserType.Client));

            trains = new List<Train>();
            trains.Add(new Train(1, "Soko1", TrainRang.Soko, 100));
            trains.Add(new Train(2, "Soko2", TrainRang.Soko, 200));
            trains.Add(new Train(3, "Jadan voz", TrainRang.Obican, 100));
            trains.Add(new Train(4, "Jos jadniji voz", TrainRang.Obican, 50));

        }
    }
}
