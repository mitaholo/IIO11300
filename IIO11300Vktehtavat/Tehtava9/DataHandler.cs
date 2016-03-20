using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tehtava5
{
    class DataHandler
    {
        public DataTable customerTable { get; private set; }

        public DataHandler()
        {
            FormatCustomerTable();
        }

        private void FormatCustomerTable()
        {
            customerTable = new DataTable();
            customerTable.Columns.Add("ID", typeof(string));
            customerTable.Columns.Add("Firstname", typeof(string));
            customerTable.Columns.Add("Lastname", typeof(string));
            customerTable.Columns.Add("Address", typeof(string));
            customerTable.Columns.Add("Zip", typeof(string));
            customerTable.Columns.Add("City", typeof(string));
        }

        public bool Load()
        {
            List<Customer> customers = DataReader.ReadToList();

            if (customers == null) return false;

            FormatCustomerTable();
            foreach (Customer customer in customers)
            {
                customerTable.Rows.Add(customer.ToArray());
            }

            return true;
        }

        public bool SaveCustomer(string firstname, string lastname, string address, string zip, string city)
        {
            Customer newCustomer = new Customer(firstname, lastname, address, zip, city);
            if(DataReader.SaveCustomer(newCustomer))
            {
                Load();
                return true;
            }
            else return false;
        }

        public bool DeleteCustomer(string id)
        {
            if (DataReader.DeleteCustomer(id))
            {
                Load();
                return true;
            }
            return false;
        }
    }
}
