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
            dataHandler.Load();
            dgCustomers.DataContext = dataHandler.customerTable.DefaultView;
        }

        private void CreateCustomerClicked(object sender, RoutedEventArgs e)
        {
            btnCreateCustomer.Visibility = Visibility.Collapsed;
            spNewCustomerInfo.Visibility = Visibility.Visible;
        }

        private void CreateCustomerSaveClicked(object sender, RoutedEventArgs e)
        {
            if(txtFirstname.Text != "" && txtLastname.Text != "" && txtAddress.Text != "" && txtZip.Text != "" && txtCity.Text != "")
            {
                if(!dataHandler.SaveCustomer(txtFirstname.Text, txtLastname.Text, txtAddress.Text, txtZip.Text, txtCity.Text))
                {
                    MessageBox.Show("Asiakkaan lisääminen epäonnistui", "Virhe", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                dgCustomers.DataContext = null;
                dgCustomers.DataContext = dataHandler.customerTable.DefaultView;

                spNewCustomerInfo.Visibility = Visibility.Collapsed;
                btnCreateCustomer.Visibility = Visibility.Visible;
            }
        }

        private void CreateCustomerCancelClicked(object sender, RoutedEventArgs e)
        {
            spNewCustomerInfo.Visibility = Visibility.Collapsed;
            btnCreateCustomer.Visibility = Visibility.Visible;
        }

        private void DeleteCustomerClicked(object sender, RoutedEventArgs e)
        {
            if (dgCustomers.SelectedIndex < 0)
            {
                MessageBox.Show("Asiakasta ei ole valittu", "Virhe", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBoxResult result = MessageBox.Show("Poistetaanko asiakas?", "Asiakkaan poisto", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                DataRowView row = (DataRowView)dgCustomers.SelectedItems[0];
                if(!dataHandler.DeleteCustomer(row["ID"].ToString()))
                {
                    MessageBox.Show("Asiakkaan poistaminen epäonnistui", "Virhe", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                dgCustomers.DataContext = null;
                dgCustomers.DataContext = dataHandler.customerTable.DefaultView;
            }
        }
    }
}
