using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.Win32;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace Tehtava3
{
    public partial class MainWindow : Window
    {
        private List<Pelaaja> pelaajat = new List<Pelaaja>();
        private string[] joukkueet = new string[] {"Ahmat","Avain","DuiPa","EVO","EVVK","GTFO","Hertat","Jazz","JiiKoo","Jock","Kirves","Leijona","Penguins","TsuiPa","WTF"
        };

        public MainWindow()
        {
            InitializeComponent();
            paivitaListBox();
            for (int i = 0; i < joukkueet.Count(); i++) cbSeura.Items.Add(joukkueet[i]);
        }

        private void tyhjennaLomake()
        {
            txtEtu.Text = txtSuku.Text = txtHinta.Text = txtUrl.Text = "";
            cbSeura.SelectedIndex = -1;
        }

        private void paivitaListBox()
        {
            listPelaajat.SelectedIndex = -1;
            listPelaajat.ItemsSource = null;
            listPelaajat.ItemsSource = pelaajat;
        }

        private void status(string teksti)
        {
            txtStatus.Text = teksti;
            txtStatus.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        }

        private void status(string teksti, byte r, byte g, byte b)
        {
            txtStatus.Text = teksti;
            txtStatus.Foreground = new SolidColorBrush(Color.FromRgb(r, g, b));
        }

        private void lataaCsv(string polku)
        {
            if (File.Exists(polku))
            {
                pelaajat = new List<Pelaaja>();
                try
                {
                    StreamReader tiedosto = new StreamReader(File.OpenRead(polku));
                    while (!tiedosto.EndOfStream)
                    {
                        string rivi = tiedosto.ReadLine();
                        string[] arvot = rivi.Split(';');
                        float hinta;
                        if (float.TryParse(arvot[3], out hinta))
                        {
                            if (arvot.Count() == 4)
                            {
                                pelaajat.Add(new Pelaaja(arvot[0], arvot[1], arvot[2], hinta, ""));
                            }
                            else if (arvot.Count() == 5)
                            {
                                pelaajat.Add(new Pelaaja(arvot[0], arvot[1], arvot[2], hinta, arvot[4]));
                            }
                        }
                    }
                    status("Tiedot ladattu");
                }
                catch (Exception)
                {
                    status("Tietojen laataaminen epäonnistui", 225, 0, 0);
                }

                listPelaajat.ItemsSource = null;
                listPelaajat.ItemsSource = pelaajat;
            }
        }

        private void lataaXml(string polku)
        {
            if (File.Exists(polku))
            {
                pelaajat = new List<Pelaaja>();
                FileStream tiedosto = null;
                try
                {
                    tiedosto = new FileStream(polku, FileMode.Open);
                    XmlSerializer serializer = new XmlSerializer(pelaajat.GetType());
                    pelaajat = serializer.Deserialize(tiedosto) as List<Pelaaja>;
                    status("Tiedot ladattu");
                }
                catch (Exception)
                {
                    status("Tietojen laataaminen epäonnistui", 225, 0, 0);
                }
                finally
                {
                    if (tiedosto != null) tiedosto.Close();
                }

                listPelaajat.ItemsSource = null;
                listPelaajat.ItemsSource = pelaajat;
            }
        }

        private void lataaBin(string polku)
        {
            if (File.Exists(polku))
            {
                pelaajat = new List<Pelaaja>();
                FileStream tiedosto = null;
                try
                {
                    tiedosto = new FileStream(polku, FileMode.Open);
                    BinaryFormatter formatter = new BinaryFormatter();
                    pelaajat = formatter.Deserialize(tiedosto) as List<Pelaaja>;
                    status("Tiedot ladattu");
                }
                catch (Exception)
                {
                    status("Tietojen laataaminen epäonnistui", 225, 0, 0);
                }
                finally
                {
                    if(tiedosto != null) tiedosto.Close();
                }

                listPelaajat.ItemsSource = null;
                listPelaajat.ItemsSource = pelaajat;
            }
        }

        private void lataa(string polku)
        {
            if (File.Exists(polku))
            {
                switch(Path.GetExtension(polku))
                {
                    case ".csv":
                        lataaCsv(polku);
                        break;
                    case ".xml":
                        lataaXml(polku);
                        break;
                    case ".bin":
                        lataaBin(polku);
                        break;
                    default:
                        status("Tiedosto on väärää tyyppiä", 225, 0, 0);
                        break;
                }
            }
            else status("Tiedostoa ei löydy", 225, 0, 0);
        }

        private bool tallennaCsv(string polku)
        {
            string csv = "";
            for (int i = 0; i < pelaajat.Count(); i++)
            {
                csv += pelaajat[i].tallenna() + "\n";
            }
            if (csv == "") return false;

            try
            {
                File.WriteAllText(polku, csv);
                return true;
            }
            catch (Exception)
            {
                status("Virhe tiedoston tallennuksessa", 225, 0, 0);
            }

            return false;
        }

        private bool tallennaXml(string polku)
        {
            FileStream tiedosto = null;

            try
            {
                tiedosto = new FileStream(polku, FileMode.Create);
                XmlSerializer serializer = new XmlSerializer(pelaajat.GetType());
                serializer.Serialize(tiedosto, pelaajat);
                return true;
            }
            catch (Exception e)
            {
                status("Virhe tiedoston tallennuksessa", 225, 0, 0);
            }
            finally
            {
                tiedosto.Close();
            }
            return false;
        }

        private bool tallennaBin(string polku)
        {
            FileStream tiedosto = null;
            
            try
            {
                tiedosto = new FileStream(polku, FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(tiedosto, pelaajat);
                return true;
            }
            catch (Exception e)
            {
                status("Virhe tiedoston tallennuksessa", 225, 0, 0);
            }
            finally
            {
                tiedosto.Close();
            }
            return false;
        }

        private bool tallenna(string polku)
        {
            if (pelaajat.Count() > 0)
            {
                switch (Path.GetExtension(polku))
                {
                    case ".csv":
                        return tallennaCsv(polku);
                    case ".xml":
                        return tallennaXml(polku);
                    case ".bin":
                        return tallennaBin(polku);
                    default:
                        status("Tiedosto on väärää tyyppiä", 225, 0, 0);
                        return false;
                }
            }
            status("Ei tallennettavia tietoja", 225, 0, 0);
            return false;
        }


        private void btnUusi_Click(object sender, RoutedEventArgs e)
        {
            float hinta;
            string etunimi = txtEtu.Text;
            string sukunimi = txtSuku.Text;

            if (etunimi != "" && sukunimi != "" && txtHinta.Text != "" && float.TryParse(txtHinta.Text, out hinta) && cbSeura.SelectedIndex >= 0)
            {

                bool varattuNimi = false;
                for (int i = 0; i < pelaajat.Count(); i++)
                {
                    varattuNimi = pelaajat[i].onkoKaima(etunimi, sukunimi);
                    if (varattuNimi) break;
                }

                if (!varattuNimi)
                {
                    Pelaaja uusiPelaaja = new Pelaaja(etunimi, sukunimi, cbSeura.Text, hinta, txtUrl.Text);
                    pelaajat.Add(uusiPelaaja);
                    paivitaListBox();
                    tyhjennaLomake();
                    status("Lisätty pelaaja " + etunimi + " " + sukunimi);
                }
                else status("Nimi varattu", 225, 0, 0);
            }
            else status("Täytä pelaajan tiedot oikein", 225, 0, 0);
        }

        private void listPelaajat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listPelaajat.SelectedIndex >= 0 && listPelaajat.SelectedItem.GetType() == typeof(Pelaaja))
            {
                Pelaaja pelaaja = (Pelaaja)listPelaajat.SelectedItem;
                txtEtu.Text = pelaaja.etunimi;
                txtSuku.Text = pelaaja.sukunimi;
                txtHinta.Text = pelaaja.hinta.ToString();
                cbSeura.Text = pelaaja.seura;
                txtUrl.Text = pelaaja.kuvaUrl;

                if (pelaaja.kuvaUrl != null && pelaaja.kuvaUrl != "")
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(pelaaja.kuvaUrl, UriKind.Absolute);
                    bitmap.EndInit();
                    imgPelaaja.Source = bitmap;
                }
            };
        }

        private void btnTallenna_Click(object sender, RoutedEventArgs e)
        {
            float hinta;
            string etunimi = txtEtu.Text;
            string sukunimi = txtSuku.Text;

            if (listPelaajat.SelectedIndex >= 0 && listPelaajat.SelectedItem.GetType() == typeof(Pelaaja))
            {
                if (etunimi != "" && sukunimi != "" && txtHinta.Text != "" && float.TryParse(txtHinta.Text, out hinta) && cbSeura.SelectedIndex >= 0)
                {
                    Pelaaja pelaaja = (Pelaaja)listPelaajat.SelectedItem;
                    pelaaja.paivita(etunimi, sukunimi, cbSeura.Text, hinta, txtUrl.Text);
                    paivitaListBox();
                    tyhjennaLomake();
                    status("Tallennettu pelaaja " + etunimi + " " + sukunimi);
                }
                else status("Täytä pelaajan tiedot oikein", 225, 0, 0);
            }
            else status("Valitse muokattava pelaaja", 225, 0, 0);
        }

        private void btnPoista_Click(object sender, RoutedEventArgs e)
        {
            if (listPelaajat.SelectedIndex >= 0 && listPelaajat.SelectedItem.GetType() == typeof(Pelaaja))
            {
                if (pelaajat.Remove((Pelaaja)listPelaajat.SelectedItem)) status("Pelaaja poistettu");
                else status("Pelaajaa ei löytynyt", 225, 0, 0);
                paivitaListBox();
                tyhjennaLomake();
            }
            else status("Valitse poistettava pelaaja", 225, 0, 0);
        }

        private void btnLataa_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Supported file (*.csv, *.xml, *.bin)|*.csv;*.xml;*.bin";
            if (openFileDialog.ShowDialog() == true)
            {
                lataa(openFileDialog.FileName);
            }
        }

        private void btnKirjoita_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV file (*.csv)|*.csv|XML file (*.xml)|*.xml|Binary file (*.bin)|*.bin";
            if (saveFileDialog.ShowDialog() == true)
            {
                if (tallenna(saveFileDialog.FileName)) status("Tiedot tallennettu");
            }
        }

        private void btnLopetus_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(1);
        }

    }
}
