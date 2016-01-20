using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Management;
using System.Data.SqlTypes;

namespace Tehtava5
{
    class Attendance
    {
        int id;
        string asioid, firstname, lastname;
        DateTime date;

        public Attendance(int id, string asioid, string lastname, string firstname, DateTime date)
        {
            this.id = id;
            this.asioid = asioid;
            this.firstname = firstname;
            this.lastname = lastname;
            this.date = date;
        }

        public string[] ToStringArray()
        {
            string[] data = new string[] {asioid, lastname, firstname, date.ToString()};
            return data;
        }

        override public string ToString()
        {
            return asioid + ", " + lastname + ", " + firstname + ", " + date.ToString();
        }
    }
}
