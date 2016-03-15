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
        DataReader dataReader;

        public DataHandler()
        {
            dataReader = new DataReader();
        }

        public DataTable getDataTable()
        {
            DataReader reader = new DataReader();
            List<Customer> customers = reader.ReadToList();

            if (customers == null) return null;

            DataTable customerTable = new DataTable();
            customerTable.Columns.Add("Firstname", typeof(string));
            customerTable.Columns.Add("Lastname", typeof(string));
            customerTable.Columns.Add("Address", typeof(string));
            customerTable.Columns.Add("City", typeof(string));

            foreach (Customer customer in customers)
            {
                customerTable.Rows.Add(customer.ToArray());
            }

            return customerTable;
        }
    }
}
