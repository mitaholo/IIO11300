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
        public static List<Customer> ReadToList()
        {
            List<Customer> customers = new List<Customer>();
            try
            {
                SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["connectionString"]);
                using (connection)
                {
                    SqlCommand command = new SqlCommand("SELECT * FROM customer", connection);
                    connection.Open();
                    using (command)
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        int idIndex = 0, firstnameIndex = 0, lastnameIndex = 0, addressIndex = 0, zipIndex = 0, cityIndex = 0;
                        int i = 0;
                        while (reader.Read())
                        {
                            if (i == 0)
                            {
                                idIndex = reader.GetOrdinal("id");
                                firstnameIndex = reader.GetOrdinal("firstname");
                                lastnameIndex = reader.GetOrdinal("lastname");
                                addressIndex = reader.GetOrdinal("address");
                                zipIndex = reader.GetOrdinal("zip");
                                cityIndex = reader.GetOrdinal("city");
                                i++;
                            }

                            customers.Add(new Customer(
                                reader.IsDBNull(idIndex) ? "" : reader.GetInt32(idIndex).ToString(),
                                reader.IsDBNull(firstnameIndex) ? "" : reader.GetString(firstnameIndex),
                                reader.IsDBNull(lastnameIndex) ? "" : reader.GetString(lastnameIndex),
                                reader.IsDBNull(addressIndex) ? "" : reader.GetString(addressIndex),
                                reader.IsDBNull(zipIndex) ? "" : reader.GetString(zipIndex),
                                reader.IsDBNull(cityIndex) ? "" : reader.GetString(cityIndex)
                            ));
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

        public static bool SaveCustomer(Customer customer)
        {
            try
            {
                SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["connectionString"]);
                using (connection)
                {
                    SqlCommand command = connection.CreateCommand();
                    using (command)
                    {
                        command.CommandText = "INSERT INTO customer (firstname, lastname, address, zip, city) VALUES (@firstname, @lastname, @address, @zip, @city)";
                        command.Parameters.AddWithValue("@firstname", customer.Firstname);
                        command.Parameters.AddWithValue("@lastname", customer.Lastname);
                        command.Parameters.AddWithValue("@address", customer.Address);
                        command.Parameters.AddWithValue("@zip", customer.Zip);
                        command.Parameters.AddWithValue("@city", customer.City);

                        connection.Open();
                        command.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                return false;
            }
        }

        public static bool DeleteCustomer(string id)
        {
            try
            {
                SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["connectionString"]);
                using (connection)
                {
                    SqlCommand command = connection.CreateCommand();
                    using (command)
                    {
                        command.CommandText = "DELETE FROM customer WHERE id = @id";
                        command.Parameters.AddWithValue("@id", id);

                        connection.Open();
                        command.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                return false;
            }
        }
    }
}
