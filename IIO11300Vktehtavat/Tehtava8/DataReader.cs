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

        public ArrayList ReadToArrayList()
        {
            return ReadToArrayList("");
        }

        public ArrayList ReadToArrayList(string search)
        {
            ArrayList attendances = new ArrayList();
            try
            {
                using (connection)
                {
                    SqlCommand command;
                    if (search == "") command = new SqlCommand("SELECT * FROM lasnaolot", connection);
                    else
                    {
                        command = new SqlCommand("SELECT * FROM lasnaolot WHERE asioid = @asioid", connection);
                        command.Parameters.Add(new SqlParameter("asioid", search));
                    }
                    connection.Open();
                    using (command)
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            attendances.Add(new Attendance(reader.GetInt16(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetDateTime(4)));
                        }
                    }
                }
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                return null;
            }

            return attendances;
        }
    }
}
