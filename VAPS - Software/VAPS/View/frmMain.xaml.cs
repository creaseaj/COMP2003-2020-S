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
using VAPS.Controller;
//using VAPS.Resources;
namespace VAPS.View
{
    /// <summary>
    /// Interaction logic for CoreWindow.xaml
    /// </summary>
    public partial class CoreWindow : Window
    {
        ARPController network;
        public CoreWindow()
        {
            
            InitializeComponent();
            var mainForm = this;
            network = new ARPController();
        }

        private void btnARP_Click(object sender, RoutedEventArgs e)
        {
            localDevices.Text = network.getArpList();

            //ARPWindow.ShowDialog();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        

        private void btnARPClk(object sender, RoutedEventArgs e)
        {

        }

        private void btnPortScan_Click(object sender, RoutedEventArgs e)
        {
            var PortScan = new PortScan();
            PortScan.ShowDialog();
        }
    }
}
