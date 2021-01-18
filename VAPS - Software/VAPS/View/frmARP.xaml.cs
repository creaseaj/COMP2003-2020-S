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
using VAPS.Controller;


namespace VAPS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ARP : Window
    {
        ARPController network;
        public ARP()
        {
            InitializeComponent();
            network = new ARPController();
        }

        private void viewNetwork(object sender, RoutedEventArgs e)
        {
            localDevices.Text = network.getArpList();
            
         }
    }
}
