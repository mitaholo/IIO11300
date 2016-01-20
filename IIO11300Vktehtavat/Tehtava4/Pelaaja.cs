using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tehtava3
{
    class Pelaaja
    {
        public string etunimi { get; private set; }
        public string sukunimi { get; private set; }
        public string seura { get; private set; }
        public float hinta { get; private set; }

        public string KokoNimi
        {
            get
            {
                return etunimi + " " + sukunimi + ", " + seura;
            }
        }


        public Pelaaja(string etunimi, string sukunimi, string seura, float hinta)
        {
            paivita(etunimi, sukunimi, seura, hinta);
        }

        public void paivita(string etunimi, string sukunimi, string seura, float hinta)
        {
            this.etunimi = etunimi;
            this.sukunimi = sukunimi;
            this.seura = seura;
            this.hinta = hinta;
        }

        public bool onkoKaima(string etunimi, string sukunimi)
        {
            return (etunimi == this.etunimi && sukunimi == this.sukunimi);
        }

        public string tallenna()
        {
            return etunimi + ";" + sukunimi + ";" + seura + ";" + hinta;
        }

        override public string ToString()
        {
            return KokoNimi;
        }
    }
}
