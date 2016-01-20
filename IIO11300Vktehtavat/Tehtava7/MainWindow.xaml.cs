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
using System.IO;
using Microsoft.Win32;
using System.Configuration;
using System.Xml.Linq;

namespace Tehtava4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        XDocument xml;
        IEnumerable<string> countries;

        public MainWindow()
        {
            InitializeComponent();

            if (File.Exists(ConfigurationManager.AppSettings["datapath"]))
            {
                xml = XDocument.Load(ConfigurationManager.AppSettings["datapath"]);
                countries = from tea in xml.Root.Descendants("tee")
                                let country = (string)tea.Element("maa")
                                orderby country
                                select country;
                countries = countries.Distinct();
                cbCountrySelection.ItemsSource = null;
                cbCountrySelection.ItemsSource = countries;

            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Window listWindow = new ListWindow(xml, countries, cbCountrySelection.Text);
            listWindow.Show();
            Close();
        }
    }
}
