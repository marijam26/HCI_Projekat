using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Projekat.Model
{
    public class User
    {
        public string username { get; set; }

        public string password { get; set; }

        public string name { get; set; }

        public string surname { get; set; }

        public UserType type { get; set; }

        public User()
        {

        }

        public User(string username, string password,string name,string suername,UserType userType)
        {
            this.username = username;
            this.password = password;
            this.name = name;
            this.surname = surname;
            this.type = userType;
        }

    }

    public enum UserType
    {
        Manager,
        Client
    }
}
