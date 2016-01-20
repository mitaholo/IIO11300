using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tehtava2Lotto
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnArvo_Click(object sender, RoutedEventArgs e)
        {
            int riveja;
            string riviTeksti = "";
            if (cbPeli.SelectedIndex >= 0 && int.TryParse(txtRiviLkm.Text, out riveja) && riveja > 0)
            {
                if (cbPeli.Text == "Eurojackpot")
                {
                    Lotto lottokone = new Lotto(3);
                    Lotto lottokoneTahti = new Lotto(4);
                    List<int> rivi, riviTahti;

                    for (int j = 0; j < riveja; j++)
                    {
                        rivi = lottokone.arvoRivi();
                        riviTahti = lottokoneTahti.arvoRivi();

                        for (int i = 0; i < rivi.Count(); i++)
                        {
                            riviTeksti += rivi[i].ToString() + ", ";
                        }

                        for (int i = 0; i < riviTahti.Count(); i++)
                        {
                            riviTeksti += "*" + riviTahti[i].ToString();
                            if (i < riviTahti.Count() - 1) riviTeksti += ", ";
                        }

                        riviTeksti += "\n";
                    }
                }
                else if(cbPeli.Text == "Lotto" || cbPeli.Text == "Viking Lotto")
                {
                    Lotto lottokone;
                    List<int> rivi;


                    if (cbPeli.Text == "Lotto")
                    {
                        lottokone = new Lotto(1);
                    }
                    else
                    {
                        lottokone = new Lotto(2);
                    }

                    for (int j = 0; j < riveja; j++) {
                        rivi = lottokone.arvoRivi();

                        for (int i = 0; i < rivi.Count(); i++)
                        {
                            riviTeksti += rivi[i].ToString();
                            if(i < rivi.Count() - 1) riviTeksti += ", ";
                        }

                        riviTeksti += "\n";
                    }
                }
                else riviTeksti = "Virhe!";
            }
            else riviTeksti = "Virhe!";

            txtRivit.Text = riviTeksti;
            windowIkkuna.Title = "This is not your lucky day!";
        }

        private void btnTyhjenna_Click(object sender, RoutedEventArgs e)
        {
            cbPeli.SelectedIndex = 0;
            txtRiviLkm.Text = "1";
            txtRivit.Text = "";
        }
    }
}
