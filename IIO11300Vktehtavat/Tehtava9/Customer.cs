using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tehtava5
{
    class Customer
    {
        public string ID { get; set; }
        public string Firstname { get; private set; }
        public string Lastname { get; private set; }
        public string Address { get; private set; }
        public string Zip { get; private set; }
        public string City { get; private set; }

        public Customer(string id, string firstname, string lastname, string address, string zip, string city)
        {
            this.ID = id;
            this.Firstname = firstname;
            this.Lastname = lastname;
            this.Address = address;
            this.Zip = zip;
            this.City = city;
        }

        public Customer(string firstname, string lastname, string address, string zip, string city)
        {
            this.ID = "";
            this.Firstname = firstname;
            this.Lastname = lastname;
            this.Address = address;
            this.Zip = zip;
            this.City = city;
        }

        public override string ToString()
        {
            return Lastname;
        }

        public string[] ToArray()
        {
            return new string[] { ID, Firstname, Lastname, Address, Zip, City };
        }
    }
}
