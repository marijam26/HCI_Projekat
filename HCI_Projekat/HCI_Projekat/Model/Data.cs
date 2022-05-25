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

        public Data()
        {
            users = new List<User>();
            users.Add(new User("maki","m","Marija","Milosevic",UserType.Manager));
            users.Add(new User("coki", "c", "Coka", "Cokic", UserType.Client));

        }
    }
}
