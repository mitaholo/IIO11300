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
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Data;

namespace Tehtava4
{
    /// <summary>
    /// Interaction logic for ListWindow.xaml
    /// </summary>
    public partial class ListWindow : Window
    {
        XDocument xml;
        IEnumerable<string> countries;

        public ListWindow(XDocument xml, IEnumerable<string> countries, string country)
        {
            InitializeComponent();
            this.xml = xml;
            this.countries = countries;

            cbCountrySelection.ItemsSource = null;
            cbCountrySelection.ItemsSource = countries;
            cbCountrySelection.Text = country;

            cbScoreSelection.ItemsSource = null;
            cbScoreSelection.ItemsSource = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            cbScoreSelection.Text = "0";

            reloadDataGrid(country, 0);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(btnLoad))
            {
                int score = 0;
                if (Int32.TryParse(cbScoreSelection.Text, out score))
                {
                    reloadDataGrid(cbCountrySelection.Text, score);
                }
            }
            else if (sender.Equals(btnAll))
            {
                cbCountrySelection.Text = "";
                cbCountrySelection.SelectedIndex = -1;
                cbScoreSelection.Text = "0";
                reloadDataGrid("", 0);
            }
        }

        private void reloadDataGrid(string country, int score)
        {
            var teas = from tea in xml.Root.Descendants("tee")
                       where (country == "" || (string)tea.Element("maa") == country) && (int)tea.Element("arvio") >= score
                       orderby (string)tea.Element("nimi")
                       select new
                       {
                           Name = (string)tea.Element("nimi"),
                           Country = (string)tea.Element("maa"),
                           Score = (int)tea.Element("arvio")
                       };
            var teaTable = new DataTable();
            teaTable.Columns.Add("Nimi", typeof(string));
            teaTable.Columns.Add("Maa", typeof(string));
            teaTable.Columns.Add("Arvio", typeof(int));
            foreach (var tea in teas)
            {
                teaTable.Rows.Add(
                    tea.Name,
                    tea.Country,
                    tea.Score
                );
            }
            dgTeas.DataContext = null;
            dgTeas.DataContext = teaTable;
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
