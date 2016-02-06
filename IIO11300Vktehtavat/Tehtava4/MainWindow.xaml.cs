using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.Win32;

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
            } else status("Täytä pelaajan tiedot oikein", 225, 0, 0);
        }

        private void listPelaajat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(listPelaajat.SelectedIndex >= 0 && listPelaajat.SelectedItem.GetType() == typeof(Pelaaja)) {
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
            openFileDialog.Filter = "CSV file (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() == true)
            {
                lataa(openFileDialog.FileName);
            }
            else status("Tallennus epäonnistui", 225, 0, 0);
        }

        private void btnKirjoita_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV file (*.csv)|*.csv";
            if (saveFileDialog.ShowDialog() == true)
            {
                tallenna(saveFileDialog.FileName);
            }
            else status("Tallennus epäonnistui", 225, 0, 0);
        }

        private void btnLopetus_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(1);
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

        private void lataa(string polku)
        {

            if (File.Exists(polku))
            {
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
                catch
                {
                    status("Tietojen laataaminen epäonnistui", 225, 0, 0);
                }
                listPelaajat.ItemsSource = null;
                listPelaajat.ItemsSource = pelaajat;
            }
            else status("Syötä pelaajien tietoja");
        }

        private bool tallenna(string polku)
        {
            string csv = "";
            for (int i = 0; i < pelaajat.Count(); i++)
            {
                csv += pelaajat[i].tallenna() + "\n";
            }
            if (csv == "")
            {
                status("Ei tallennettavaa", 225, 0, 0);
                return false;
            }

            try
            {
                File.WriteAllText(polku, csv);
                status("Tiedosto tallennettu");
                return true;
            }
            catch
            {
                status("Tallennus epäonnistui", 225, 0, 0);
                return false;
            }
        }
    }
}
