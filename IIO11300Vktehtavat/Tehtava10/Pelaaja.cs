using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tehtava3
{
    public class Pelaaja
    {
        public long id { get; set; }
        public string etunimi { get; set; }
        public string sukunimi { get; set; }
        public string seura { get; set; }
        public double hinta { get; set; }
        public string kuvaUrl { get; set; }
        public bool updated { get; set; }

        public string KokoNimi
        {
            get { return etunimi + " " + sukunimi + ", " + seura; }
        }


        public Pelaaja() { }

        public Pelaaja(string etunimi, string sukunimi, string seura, double hinta, string kuvaUrl) : this(-1, etunimi, sukunimi, seura, hinta, kuvaUrl) { }

        public Pelaaja(long id, string etunimi, string sukunimi, string seura, double hinta, string kuvaUrl)
        {
            this.id = id;
            this.etunimi = etunimi;
            this.sukunimi = sukunimi;
            this.seura = seura;
            this.hinta = hinta;
            this.kuvaUrl = kuvaUrl;
            updated = false;
        }

        public void Paivita(string etunimi, string sukunimi, string seura, double hinta, string kuvaUrl)
        {
            this.etunimi = etunimi;
            this.sukunimi = sukunimi;
            this.seura = seura;
            this.hinta = hinta;
            this.kuvaUrl = kuvaUrl;
            updated = true;
        }

        public bool OnkoKaima(string etunimi, string sukunimi)
        {
            return (etunimi == this.etunimi && sukunimi == this.sukunimi);
        }

        public string Status()
        {
            if (id < 0) return "new";
            if (updated) return "updated";
            return "";
        }

        override public string ToString()
        {
            return KokoNimi;
        }
    }
}
