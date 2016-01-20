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
using System.Configuration;
using System.Data;
using System.Collections;

namespace Tehtava5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataHandler dataHandler;

        public MainWindow()
        {
            InitializeComponent();
            dataHandler = new DataHandler();
            Status("Ready");
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if(sender.Equals(btnLoadToDataTable))
            {
                DataTable attendanceTable = dataHandler.getDataTable();
                if(attendanceTable == null)
                {
                    Status("Tietokantavirhe!", 255, 0, 0);
                }
                else
                {
                    dgAttendances.DataContext = null;
                    dgAttendances.DataContext = attendanceTable;
                }
            }
            else if (sender.Equals(btnLoadToDataView) && txtSearch.Text != "")
            {
                DataView attendanceView = dataHandler.getDataView(txtSearch.Text);
                if (attendanceView == null)
                {
                    Status("Tietokantavirhe!", 255, 0, 0);
                }
                else
                {
                    dgAttendances.DataContext = null;
                    dgAttendances.DataContext = attendanceView;
                }
                
            }
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public void Status(string teksti)
        {
            txtStatus.Text = teksti;
            txtStatus.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        }

        public void Status(string teksti, byte r, byte g, byte b)
        {
            txtStatus.Text = teksti;
            txtStatus.Foreground = new SolidColorBrush(Color.FromRgb(r, g, b));
        }
    }
}
