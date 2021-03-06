﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tehtava3
{
    [Serializable]
    public class Pelaaja
    {
        public string etunimi { get; set; }
        public string sukunimi { get; set; }
        public string seura { get; set; }
        public float hinta { get; set; }
        public string kuvaUrl { get; set; }

        public string KokoNimi
        {
            get { return etunimi + " " + sukunimi + ", " + seura; }
        }


        public Pelaaja() { }

        public Pelaaja(string etunimi, string sukunimi, string seura, float hinta, string kuvaUrl)
        {
            paivita(etunimi, sukunimi, seura, hinta, kuvaUrl);
        }

        public void paivita(string etunimi, string sukunimi, string seura, float hinta, string kuvaUrl)
        {
            this.etunimi = etunimi;
            this.sukunimi = sukunimi;
            this.seura = seura;
            this.hinta = hinta;
            this.kuvaUrl = kuvaUrl;
        }

        public bool onkoKaima(string etunimi, string sukunimi)
        {
            return (etunimi == this.etunimi && sukunimi == this.sukunimi);
        }

        public string tallenna()
        {
            return etunimi + ";" + sukunimi + ";" + seura + ";" + hinta + ";" + kuvaUrl;
        }

        override public string ToString()
        {
            return KokoNimi;
        }
    }
}
