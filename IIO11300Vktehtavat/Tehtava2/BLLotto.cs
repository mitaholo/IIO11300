using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tehtava2Lotto
{
    class Lotto
    {
        public int tyyppi; // 1: Lotto, 2: Viking Lotto, 3: Eurojackpot päänumerot, 4: Eurojackpot tähtinumerot
        private int pieninNumero, suurinNumero, lkmNumero;
        private Random arpoja;  // Lisätty jäsenmuuttujaksi, jotta "satunnaisuus" toimisi useaa riviä arvottaessa.
                                // Mikäli arvoRivi-metodi loisi aina uuden Random-olion, monta peräkkäistä oliota 
                                // saisi saman kellonajasta otetun seedin ja tuottaisi siksi saman lottorivin 

        public Lotto(int tyyppi)
        {
            if (tyyppi < 1 || tyyppi > 4) tyyppi = 1;
            this.tyyppi = tyyppi;
            arpoja = new Random();

            if (tyyppi == 1) // Lotto
            {
                pieninNumero = 1;
                suurinNumero = 39;
                lkmNumero = 7;
            }
            else if (tyyppi == 2) // Viking Lotto
            {
                pieninNumero = 1;
                suurinNumero = 48;
                lkmNumero = 6;
            }
            else if (tyyppi == 3) // Eurojackpot päänumerot
            {
                pieninNumero = 1;
                suurinNumero = 50;
                lkmNumero = 5;
            }
            else if (tyyppi == 4) // Eurojackpot tähtinumerot
            {
                pieninNumero = 1;
                suurinNumero = 10;
                lkmNumero = 2;
            }
        }

        public List<int> arvoRivi()
        {
            // Luo numerot, joista arvotaan
            List<int> numerot = new List<int>();
            for (int i = pieninNumero; i <= suurinNumero; i++)
            {
                numerot.Add(i);
            }

            // Arpoo numerot
            List<int> rivi = new List<int>();
            
            int numeroitaJaljella = suurinNumero;
            int valinta;
            for (int i = 0; i < lkmNumero; i++)
            {
                valinta = arpoja.Next(0, numeroitaJaljella);
                rivi.Add(numerot[valinta]);
                numerot.RemoveAt(valinta);
                numeroitaJaljella--;
            }

            return rivi;
        }
    }
}