using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tehtava3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FileHandler fileHandler;
        List<Image> images;

        public MainWindow()
        {
            InitializeComponent();
            fileHandler = new FileHandler();
        }

        // Antaa käyttäjän valita avattavan kansion Windowsin FolderBrowserDialogin avulla
        private void SelectFolder()
        {
            string path = "";
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            path = fbd.SelectedPath;
            OpenImages(path);
        }

        // Lukee halutun kansion kuvien tiedot FileHandlerin avulla
        private void OpenImages(string path)
        {
            images = new List<Image>();

            List<string> imageFilePaths = Directory.GetFiles(path).Where(item => item.EndsWith(".png")).ToList();

            if(imageFilePaths.Count == 0)
            {
                System.Windows.MessageBox.Show("Kansio ei sisällä PNG-kuvia", "Virhe", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            foreach (string imageFilePath in imageFilePaths)
            {
                Regex regex = new Regex(@"\\(?<filename>[^\\]*)\.png$");
                Match match = regex.Match(imageFilePath);
                string filename = match.Groups["filename"].Value;

                Image image = new Image(filename, imageFilePath);
                fileHandler.readDpi(image);
                images.Add(image);
            }
            dgImages.ItemsSource = images;
        }

        // Avaa DpiWindowin kuvan DPI:n muokkaamiseen
        private void EditDpi(Image image)
        {
            DpiWindow dpiWindow;
            dpiWindow = new DpiWindow(image);
            dpiWindow.Owner = this;

            dpiWindow.Closed += (_, args) =>
            {
                if (dpiWindow.DialogResult == true)
                {
                    dgImages.Items.Refresh();
                    dgImages.UpdateLayout();
                    SaveDpi(image);
                }
            };

            dpiWindow.ShowDialog();
        }

        // Pyytää FileHandleria tallentamaan kuvan
        private void SaveDpi(Image image)
        {
            List<byte> binaryFile = fileHandler.readFile(image);
            if (binaryFile != null)
            {
                if (fileHandler.saveFile(image, binaryFile))
                {
                    System.Windows.MessageBox.Show("Tiedoston tallentaminen onnistui", "DPI tallennettu", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
            System.Windows.MessageBox.Show("Tiedoston tallentaminen epäonnistui", "Virhe", MessageBoxButton.OK, MessageBoxImage.Error);
        }


        private void OpenFolderClicked(object sender, RoutedEventArgs e)
        {
            SelectFolder();
        }

        private void ImageDoubleClicked(object sender, MouseButtonEventArgs e)
        {
            if (!(dgImages.SelectedItem is Image)) return;

            EditDpi(dgImages.SelectedItem as Image);
        }

        private void CloseClicked(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
