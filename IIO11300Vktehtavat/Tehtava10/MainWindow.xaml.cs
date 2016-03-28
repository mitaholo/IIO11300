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
        private readonly string[] joukkueet = new string[] { "Ahmat", "Avain", "DuiPa", "EVO", "EVVK", "GTFO", "Hertat", "Jazz", "JiiKoo", "Jock", "Kirves", "Leijona", "Penguins", "TsuiPa", "WTF" };
        DataHandler dataHandler;
        private List<Pelaaja> pelaajat;
        private List<Pelaaja> poistetutPelaajat;

        public MainWindow()
        {
            InitializeComponent();
            dataHandler = new DataHandler();
            pelaajat = dataHandler.ReadPlayers();
            if (pelaajat == null)
            {
                ShowError();
                pelaajat = new List<Pelaaja>();
            }
            poistetutPelaajat = new List<Pelaaja>();
            PaivitaListBox();
            for (int i = 0; i < joukkueet.Count(); i++) cbSeura.Items.Add(joukkueet[i]);
        }

        private void TyhjennaLomake()
        {
            txtEtu.Text = txtSuku.Text = txtHinta.Text = txtUrl.Text = "";
            cbSeura.SelectedIndex = -1;
        }

        private void PaivitaListBox()
        {
            listPelaajat.SelectedIndex = -1;
            listPelaajat.ItemsSource = null;
            listPelaajat.ItemsSource = pelaajat;
        }

        private void Status(string teksti)
        {
            txtStatus.Text = teksti;
            txtStatus.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        }

        private void Status(string teksti, byte r, byte g, byte b)
        {
            txtStatus.Text = teksti;
            txtStatus.Foreground = new SolidColorBrush(Color.FromRgb(r, g, b));
        }

        private void ShowError()
        {
            foreach(String error in dataHandler.errors)
            {
                MessageBox.Show(error, "Virhe", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            dataHandler.errors.Clear();
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
                    varattuNimi = pelaajat[i].OnkoKaima(etunimi, sukunimi);
                    if (varattuNimi) break;
                }

                if (!varattuNimi)
                {
                    Pelaaja uusiPelaaja = new Pelaaja(etunimi, sukunimi, cbSeura.Text, hinta, txtUrl.Text);
                    pelaajat.Add(uusiPelaaja);
                    PaivitaListBox();
                    TyhjennaLomake();
                    Status("Lisätty pelaaja " + etunimi + " " + sukunimi);
                }
                else Status("Nimi varattu", 225, 0, 0);
            }
            else Status("Täytä pelaajan tiedot oikein", 225, 0, 0);
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
            double hinta;
            string etunimi = txtEtu.Text;
            string sukunimi = txtSuku.Text;

            if (listPelaajat.SelectedIndex >= 0 && listPelaajat.SelectedItem.GetType() == typeof(Pelaaja))
            {
                if (etunimi != "" && sukunimi != "" && txtHinta.Text != "" && double.TryParse(txtHinta.Text, out hinta) && cbSeura.SelectedIndex >= 0)
                {
                    Pelaaja pelaaja = (Pelaaja)listPelaajat.SelectedItem;
                    pelaaja.Paivita(etunimi, sukunimi, cbSeura.Text, hinta, txtUrl.Text);
                    PaivitaListBox();
                    TyhjennaLomake();
                    Status("Tallennettu pelaaja " + etunimi + " " + sukunimi);
                }
                else Status("Täytä pelaajan tiedot oikein", 225, 0, 0);
            }
            else Status("Valitse muokattava pelaaja", 225, 0, 0);
        }

        private void btnPoista_Click(object sender, RoutedEventArgs e)
        {
            if (listPelaajat.SelectedIndex >= 0 && listPelaajat.SelectedItem.GetType() == typeof(Pelaaja))
            {
                Pelaaja poistettavaPelaaja = (Pelaaja) listPelaajat.SelectedItem;
                if (pelaajat.Remove((Pelaaja)listPelaajat.SelectedItem))
                {
                    poistetutPelaajat.Add(poistettavaPelaaja);
                    Status("Pelaaja poistettu");
                }
                else Status("Pelaajaa ei löytynyt", 225, 0, 0);
                PaivitaListBox();
                TyhjennaLomake();
            }
            else Status("Valitse poistettava pelaaja", 225, 0, 0);
        }

        private void btnKirjoita_Click(object sender, RoutedEventArgs e)
        {
            if(dataHandler.WritePlayers(pelaajat, poistetutPelaajat)) Status("Pelaajat tallennettu");
            pelaajat = dataHandler.ReadPlayers();
            ShowError();
            PaivitaListBox();
        }

        private void btnLopetus_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(1);
        }

    }
}
