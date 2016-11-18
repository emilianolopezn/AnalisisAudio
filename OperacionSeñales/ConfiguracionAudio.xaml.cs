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

namespace OperacionSeñales
{
    /// <summary>
    /// Interaction logic for ConfiguracionAudio.xaml
    /// </summary>
    public partial class ConfiguracionAudio : UserControl
    {
        public ConfiguracionAudio()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                txtRuta.Text = openFileDialog.FileName;
        }

        private void txtRuta_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
