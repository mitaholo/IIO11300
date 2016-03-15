using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Management;

namespace Tehtava5
{
    class DataReader
    {
        SqlConnection connection;

        public DataReader()
        {
            string connectionString = ConfigurationManager.AppSettings["connectionString"];
            connection = new SqlConnection(connectionString);
        }

        public List<Customer> ReadToList()
        {
            List<Customer> customers = new List<Customer>();
            try
            {
                using (connection)
                {
                    SqlCommand command  = new SqlCommand("SELECT * FROM vCustomers", connection);
                    connection.Open();
                    using (command)
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            customers.Add(new Customer(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3)));
                        }
                    }
                }
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                return null;
            }

            return customers;
        }
    }
}
