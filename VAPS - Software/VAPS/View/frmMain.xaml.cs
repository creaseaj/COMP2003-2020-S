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
            //Port.Instance.fileInput();

            //The visibilities are used in development, this code is likely to be removed and the items set to hidden in release
            arpGrid.Visibility = Visibility.Hidden;
        }

        private void btnARP_Click(object sender, RoutedEventArgs e)
        {
            
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
                //nmapOut.Text = "You must have permission from the network adminstrator to perform this action";
            }

        }
        private async Task nmapScanningProgress()
        {
           
            // Task runs in the background which is the nmap function
            Task<DataTable> nmapTable = Task.Run(() =>
            {
                return nController.fingerPrint(nController.GetLocalIPAddress(),nController.getSubnetFromIP(nController.GetLocalIPAddress()));
            });
            int counter = 0;
            // Runs while nmap command is still running, shows scanning dots
            while (!nmapTable.IsCompleted)
            {
                await Task.Delay(500);
                switch(counter % 3)
                {
                    case 0:
                        nmapInstall.Content = "Scanning.";
                    break;
                    case 1:
                        nmapInstall.Content = "Scanning..";
                    break;
                    case 2:
                        nmapInstall.Content = "Scanning...";
                    break;
                }
                counter++;

            }
                    
            nmapOutGrid.ItemsSource = nmapTable.Result.DefaultView;
            nmapOutGrid.Visibility = Visibility.Visible;
            nmapInstall.Content = "Run Nmap Scan";
        }

        private void ipsubShow_Click(object sender, RoutedEventArgs e)
        {
            string ipaddress = nController.GetLocalIPAddress();
            //nmapOut.Text = (ipaddress);

            /// nController.Subnet();

            //nmapOut.Text += nController.getSubnetFromIP(ipaddress);



        }

        private void nmapInstall_Click(object sender, RoutedEventArgs e)
        {
            //nmapOut.Text = new nmapController().scanLocal();
        }
    }
}
