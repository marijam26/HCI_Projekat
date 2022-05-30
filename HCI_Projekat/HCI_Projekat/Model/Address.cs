using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Projekat.Model
{
    public class Address
    {
        public int id { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string street { get; set; }

        public Address() { }

        public Address(int id, string city, string country, string street) {
            this.city = city;
            this.country = country;
            this.street = street;
            this.id = id;
        }
    }
}
