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
        }

        private void GetCustomersClicked(object sender, RoutedEventArgs e)
        {
            lbCustomers.DataContext = dataHandler.getDataTable();
            lbCustomers.DisplayMemberPath = "Lastname";
        }

        private void CustomerSelected(object sender, SelectionChangedEventArgs e)
        {
            if (!(lbCustomers.SelectedItem is DataRowView)) return;
            spCustomerInfo.DataContext = (lbCustomers.SelectedItem as DataRowView);
        }
    }
}
