using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Tehtava6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            tbPasswordAnalysis.Text = "...\n...\n...\n...\n...";
            tbPasswordRating.Text = "anna salasana";
            tbPasswordRating.Background = new SolidColorBrush(Color.FromArgb(255, (byte)128, (byte)128, (byte)128));
        }

        private void CheckPassword()
        {
            int upper, lower, digit, symbol, total, categories;
            upper = lower = digit = symbol = total = categories = 0;

            foreach (char c in txtPassword.Text)
            {
                if (Char.IsLower(c)) lower++;
                else if (Char.IsUpper(c)) upper++;
                else if (Char.IsDigit(c)) digit++;
                else symbol++;
                total++;
            }

            categories = Convert.ToInt32(upper > 0) + Convert.ToInt32(lower > 0) + Convert.ToInt32(digit > 0) + Convert.ToInt32(symbol > 0);

            tbPasswordAnalysis.Text = "Merkkejä: " + total + "\nIsoja kirjaimia: " + upper + "\nPieniä kirjaimia: " + lower + "\nNumeroita: " + digit + "\nErikoismerkkejä: " + symbol;

            if (total >= 16 && categories >= 4)
            {
                tbPasswordRating.Text = "good";
                tbPasswordRating.Background = new SolidColorBrush(Color.FromArgb(255, (byte)0, (byte)128, (byte)0));
            }
            else if (total >= 12 && categories >= 3)
            {
                tbPasswordRating.Text = "moderate";
                tbPasswordRating.Background = new SolidColorBrush(Color.FromArgb(255, (byte)144, (byte)238, (byte)144));
            }
            else if (total >= 8 && categories >= 2)
            {
                tbPasswordRating.Text = "fair";
                tbPasswordRating.Background = new SolidColorBrush(Color.FromArgb(255, (byte)255, (byte)255, (byte)0));
            }
            else
            {
                tbPasswordRating.Text = "bad";
                tbPasswordRating.Background = new SolidColorBrush(Color.FromArgb(255, (byte)255, (byte)165, (byte)0));
            }
        }

        private void PasswordTextInput(object sender, TextChangedEventArgs e)
        {
            CheckPassword();
        }
    }
}
