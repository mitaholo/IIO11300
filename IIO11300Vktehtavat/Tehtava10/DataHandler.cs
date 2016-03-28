using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tehtava3
{
    class DataHandler
    {
        SQLiteConnection connection;
        public List<String> errors { get; set; }

        public DataHandler()
        {
            errors = new List<String>();

            if (!File.Exists(ConfigurationManager.AppSettings["dbPath"])) CreateDatabase();

            connection = new SQLiteConnection("Data Source=" + ConfigurationManager.AppSettings["dbPath"] + ";Version=3;");
        }

        private bool CreateDatabase()
        {
            SQLiteConnection tempConnection = null;
            try
            {
                SQLiteConnection.CreateFile(ConfigurationManager.AppSettings["dbPath"]);
                tempConnection = new SQLiteConnection("Data Source=" + ConfigurationManager.AppSettings["dbPath"] + ";Version=3;");
                tempConnection.Open();

                SQLiteCommand command = tempConnection.CreateCommand();
                command.CommandText = "CREATE TABLE pelaaja (id INTEGER PRIMARY KEY AUTOINCREMENT, etunimi TEXT NOT NULL, sukunimi TEXT NOT NULL, seura TEXT NOT NULL, hinta FLOAT NOT NULL, kuva_url TEXT NULL)";
                command.ExecuteNonQuery();

                tempConnection.Close();
            }
            catch(Exception e)
            {
                errors.Add("Tietokannan luominen epäonnistui");
                return false;
            }
            finally
            {
                if(tempConnection != null) tempConnection.Close();
            }

            return true;
        }

        public List<Pelaaja> ReadPlayers()
        {
            List<Pelaaja> pelaajat = new List<Pelaaja>();

            try
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM pelaaja";
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    pelaajat.Add(new Pelaaja((long)reader["id"], (string)reader["etunimi"], (string)reader["sukunimi"], (string)reader["seura"], (double)reader["hinta"], (string)reader["kuva_url"]));
                }
            }
            catch (Exception e)
            {
                errors.Add("Tietokannan lukeminen epäonnistui");
                return null;
            }
            finally
            {
                connection.Close();
            }

            return pelaajat;
        }

        public bool WritePlayers(List<Pelaaja> pelaajat, List<Pelaaja> poistetutPelaajat)
        {
            List<Pelaaja> uudetPelaajat = new List<Pelaaja>();
            List<Pelaaja> muokatutPelaajat = new List<Pelaaja>();

            foreach (Pelaaja pelaaja in pelaajat)
            {
                if (pelaaja.Status() == "new") uudetPelaajat.Add(pelaaja);
                else if (pelaaja.Status() == "updated") muokatutPelaajat.Add(pelaaja);
            }

            try
            {
                connection.Open();

                SQLiteTransaction transaction = connection.BeginTransaction();
                SQLiteCommand command = connection.CreateCommand();
                command.Transaction = transaction;

                command.Parameters.Add(new SQLiteParameter("@id", DbType.Int32, 0));
                command.Parameters.Add(new SQLiteParameter("@etunimi", DbType.String, 0));
                command.Parameters.Add(new SQLiteParameter("@sukunimi", DbType.String, 0));
                command.Parameters.Add(new SQLiteParameter("@seura", DbType.String, 0));
                command.Parameters.Add(new SQLiteParameter("@hinta", DbType.Double, 0));
                command.Parameters.Add(new SQLiteParameter("@kuva_url", DbType.String, 0));

                foreach (Pelaaja pelaaja in poistetutPelaajat)
                {
                    if (pelaaja.Status() == "new") continue;

                    command.CommandText = "DELETE FROM pelaaja WHERE id = @id";

                    command.Parameters["@id"].Value = pelaaja.id;

                    command.ExecuteNonQuery();
                }

                foreach (Pelaaja pelaaja in muokatutPelaajat)
                {
                    command.CommandText = "UPDATE pelaaja SET etunimi = @etunimi, sukunimi = @sukunimi, seura = @seura, hinta = @hinta, kuva_url = @kuva_url WHERE id = @id";

                    command.Parameters["@etunimi"].Value = pelaaja.etunimi;
                    command.Parameters["@sukunimi"].Value = pelaaja.sukunimi;
                    command.Parameters["@seura"].Value = pelaaja.seura;
                    command.Parameters["@hinta"].Value = pelaaja.hinta;
                    command.Parameters["@kuva_url"].Value = pelaaja.kuvaUrl;
                    command.Parameters["@id"].Value = pelaaja.id;

                    command.ExecuteNonQuery();
                }

                foreach (Pelaaja pelaaja in uudetPelaajat)
                {
                    command.CommandText = "INSERT INTO pelaaja (etunimi, sukunimi, seura, hinta, kuva_url) VALUES (@etunimi, @sukunimi, @seura, @hinta, @kuva_url)";

                    command.Parameters["@etunimi"].Value = pelaaja.etunimi;
                    command.Parameters["@sukunimi"].Value = pelaaja.sukunimi;
                    command.Parameters["@seura"].Value = pelaaja.seura;
                    command.Parameters["@hinta"].Value = pelaaja.hinta;
                    command.Parameters["@kuva_url"].Value = pelaaja.kuvaUrl;

                    command.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch (Exception e)
            {
                errors.Add("Pelaajien tallentaminen epäonnistui");
                return false;
            }
            finally
            {
                connection.Close();
            }

            return true;
        }
    }
}
