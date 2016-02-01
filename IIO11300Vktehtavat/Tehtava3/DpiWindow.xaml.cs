using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Tehtava3
{
    /// <summary>
    /// Interaction logic for DpiWindow.xaml
    /// </summary>
    public partial class DpiWindow : Window
    {
        Image image;

        public DpiWindow(Image image)
        {
            InitializeComponent();

            this.image = image;
            this.Title = image.Name;
            spDpiForm.DataContext = image;
        }

        // Tallentaa tehdyt muutokset Image-olioon ja sulkee ikkunan
        private void SaveClick(object sender, RoutedEventArgs e)
        {
            int dpiX = -1, dpiY = -1;

            Int32.TryParse(tbDpiX.Text, out dpiX);
            Int32.TryParse(tbDpiY.Text, out dpiY);

            if (dpiX >= 0 && dpiY >= 0)
            {
                image.DpiX = dpiX;
                image.DpiY = dpiY;

                if (cbUnit.Text == "1 (määrätty)") image.Unit = true;
                else image.Unit = false;

                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Virhe DPI-arvoissa", "Virhe", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Sulkee ikkunan tallentamatta muutoksia
        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        // Estää tekstin liittämisen tekstikenttään
        private void OnPreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste) e.Handled = true;
        }

        // Sallii tekstikenttään ainoastaan numeroita
        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }
    }
}
