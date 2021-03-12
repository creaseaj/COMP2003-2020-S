using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
using VAPS.Model;

namespace VAPS.View
{
    /// <summary>
    /// Interaction logic for CoreWindow.xaml
    /// </summary>
    public partial class CoreWindow : Window
    {
        ARPController ARP;
        PortScanController PortScan;
        nmapController nController;
        public CoreWindow()
        {
            InitializeComponent();
            var mainForm = this;
            ARP = new ARPController();
            PortScan = new PortScanController();
            nController = new nmapController();
            Port.Instance.fileInput();

            //The visibilities are used in development, this code is likely to be removed and the items set to hidden in release
            arpGrid.Visibility = Visibility.Hidden;
        }

        private void btnARP_Click(object sender, RoutedEventArgs e)
        {
            DataTable ARPTable = new DataTable();
            ARPTable = ARP.generateTable(ARPTable);
            arpGrid.ItemsSource = ARPTable.DefaultView;
            arpGrid.Visibility = Visibility.Visible;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
        private void btnARPClk(object sender, RoutedEventArgs e)
        {

        }
        private void btnPortScanner_Click(object sender, RoutedEventArgs e)
        {

            DataTable PortScannerTable = new DataTable();
            PortScannerTable = PortScan.generateTable(PortScannerTable);
            PortScannerDataGrid.ItemsSource = PortScannerTable.DefaultView;
            PortScannerDataGrid.Visibility = Visibility.Visible;
            int[] results = PortScan.getValues(PortScannerTable);
            txtBlockOpenNum.Text = results[0].ToString();
            txtBlockCouldNum.Text = results[1].ToString();
            txtBlockShouldNum.Text = results[2].ToString();



        }

        private void tabCon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }



        private void scnNtwrkClk(object sender, RoutedEventArgs e)
        {
            if (authChkBx.IsChecked.Value)
            {
                nmapScanningProgress();

            }
            else
            {
                nmapOut.Text = "You must have permission from the network adminstrator to perform this action";
            }

        }
        private async Task nmapScanningProgress()
        {
            // Task runs in the background which is the nmap function
            Task<string> getNmap = Task.Run(() =>
            {
                return new nmapController().fingerPrint("192.168.0.0","255.255.255.0");
            });
            nmapOut.Text = "initialTest";
            // Runs while nmap command is still running, shows scanning dots
            for (int i = 0; i < 100; i++)
            {
                await Task.Delay(500);
                nmapOut.Text = i.ToString();
                switch (i % 3)
                {
                    case 0:
                        nmapOut.Text = "Scanning.";
                        break;
                    case 1:
                        nmapOut.Text = "Scanning..";
                        break;
                    case 2:
                        nmapOut.Text = "Scanning...";
                        break;
                }

                // Checks if nmap command is still running
                if(getNmap.IsCompleted)
                {
                    break;
                }

            }
            nmapOut.Text = getNmap.Result;
        }

        private void ipsubShow_Click(object sender, RoutedEventArgs e)
        {
            string ipaddress = nController.GetLocalIPAddress();
            nmapOut.Text = (ipaddress);
            
            /// nController.Subnet();

            string test;
            test = "192.168";

            if (ipaddress.StartsWith(test))
            {
                nmapOut.Text = nmapOut.Text + ("\n255.255.255.0");
            }
            test = "172.";

            if (ipaddress.StartsWith(test))
            {
                nmapOut.Text = nmapOut.Text + ("\n255.255.0.0");
            }
            test = "10.";

            if (ipaddress.StartsWith(test))
            {
                nmapOut.Text = nmapOut.Text + ("\n255.0.0.0");
            }

        }

        private void nmapInstall_Click(object sender, RoutedEventArgs e)
        {
            nmapOut.Text = new nmapController().scanLocal();
        }
    }
}
