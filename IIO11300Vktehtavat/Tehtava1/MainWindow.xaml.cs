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

namespace IIO13200_15S
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

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void buttonCalculate_Click(object sender, RoutedEventArgs e)
        {
            int width, height, frameWidth;

            if (int.TryParse(txtWindowWidth.Text, out width) && int.TryParse(txtWindowHeight.Text, out height))
            {
                labelArea.Content = width * height;

                if (int.TryParse(txtFrameWidth.Text, out frameWidth))
                {
                    labelFramePerim.Content = 2 * ((height + 2 * frameWidth) + (width + 2 * frameWidth));
                    labelFrameArea.Content = ((height + 2 * frameWidth) * (width + 2 * frameWidth)) - (width * height);
                }
                else
                {
                    labelFramePerim.Content = labelFrameArea.Content = "Virhe!";
                }
            }
            else labelArea.Content = labelFramePerim.Content = labelFrameArea.Content = "Virhe!";
        }
    }
}
