using System;
using System.Collections;
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
        UsernameSearchController usernameSearch;

        PasswordTesterController PasswordTesting;
        Image[] passwordImages;

        nmapController nController;

        public CoreWindow()
        {
            InitializeComponent();
            var mainForm = this;
            ARP = new ARPController();
            PortScan = new PortScanController();
            usernameSearch = new UsernameSearchController();
            PasswordTesting = new PasswordTesterController();
            Port.Instance.fileInput();
            Device.Instance.fileInput();
            usernameSearch.fileInput();

            nController = new nmapController();
            //Port.Instance.fileInput();

            //The visibilities are used in development, this code is likely to be removed and the items set to hidden in release
            arpGrid.Visibility = Visibility.Hidden;
            txtARPDeviceName.IsEnabled = false;
            btnARPAddName.IsEnabled = false;
            txtARPDeviceName.Visibility = Visibility.Hidden;
            btnARPAddName.Visibility = Visibility.Hidden;

            passwordImages = new Image[] { imgLength, imgLower, imgNumber, imgPassword, imgSpecial, imgUpper };
        }

        private void btnARP_Click(object sender, RoutedEventArgs e)
        {
            

            DataTable ARPTable = new DataTable();
            ARPTable = ARP.formatTable(ARPTable);
            arpGrid.ItemsSource = ARPTable.DefaultView;
            arpGrid.Visibility = Visibility.Visible;
            txtARPDeviceName.Visibility = Visibility.Visible;
            btnARPAddName.Visibility = Visibility.Visible;
            btnARPAddName.IsEnabled = false;
            int[] ARPResults = ARP.getResults(ARPTable);
            blockARPKnown.Text = ARPResults[1].ToString();
            blockARPRegistered.Text = ARPResults[0].ToString();
            blockARPUnknown.Text = ARPResults[2].ToString();

        }
      

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
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
            /*         This causes the ARP tab to run incredibly slow
            if (tabARP != null && tabARP.IsSelected)
            {
                DataTable ARPTable = new DataTable();
                ARPTable = ARP.formatTable(ARPTable);
                arpGrid.ItemsSource = ARPTable.DefaultView;
                arpGrid.Visibility = Visibility.Visible;
                txtARPDeviceName.Visibility = Visibility.Visible;
                btnARPAddName.Visibility = Visibility.Visible;
                btnARPAddName.IsEnabled = false;
                int[] ARPResults = ARP.getResults(ARPTable);
                blockARPKnown.Text = ARPResults[1].ToString();
                blockARPRegistered.Text = ARPResults[0].ToString();
                blockARPUnknown.Text = ARPResults[2].ToString();
            }
            */
            if (tabPortScanner != null && tabPortScanner.IsSelected)
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
        }

        private void arpGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtARPDeviceName.IsEnabled = true;
            btnARPAddName.IsEnabled = true;
            txtARPDeviceName.Text = "";
        }

        private void btnARPAddName_Click(object sender, RoutedEventArgs e)
        {
            DataRowView rowView = arpGrid.SelectedItem as DataRowView;
            try
            {
                
                string macAddress = rowView.Row[1].ToString();
                ARP.registerDevice(macAddress, txtARPDeviceName.Text);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("You must select a device first.");
            }
            DataTable ARPTable = new DataTable();
            ARPTable = ARP.formatTable(ARPTable);
            arpGrid.ItemsSource = ARPTable.DefaultView;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (pwdPasswordInput.Password.Length != 0)
            {
                txtTimeToCrack.Text = "Time to crack: " + PasswordTesting.timeToCrack(pwdPasswordInput.Password);
                passwordImages = PasswordTesting.passwordGuidance(pwdPasswordInput.Password, passwordImages);
                txtBlockCleartext.Text = pwdPasswordInput.Password;
            }
            else
            {
                txtTimeToCrack.Text = "";
            }
        }
        


        private void btnShowClear_Click(object sender, RoutedEventArgs e)
        {
            if (txtBlockCleartext.Visibility == Visibility.Hidden)
            {
                txtBlockCleartext.Visibility = Visibility.Visible;
                pwdPasswordInput.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFABADB3"));
            }
            else if (txtBlockCleartext.Visibility == Visibility.Visible)
            {
                txtBlockCleartext.Visibility = Visibility.Hidden;
                pwdPasswordInput.Foreground = new SolidColorBrush(Colors.Black);
            }

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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnUsernameSearch_Click(object sender, RoutedEventArgs e)
        {
            _ = searchUsername();

        }
        private async Task searchUsername()
        {
            /*
            //DataTable UsernameTable = new DataTable();
            Task<DataTable> UsernameTable = Task.Run(() =>
            {
                return usernameSearch.generateResults(txtUsername.Text);
            });
            int counter = 0;
            // Runs while nmap command is still running, shows scanning dots
            while (!UsernameTable.IsCompleted)
            {
                await Task.Delay(500);
                switch (counter % 3)
                {
                    case 0:
                        btnUsernameSearch.Content = ("Searching.");
                        break;
                    case 1:
                        btnUsernameSearch.Content = ("Searching..");
                        break;
                    case 2:
                        btnUsernameSearch.Content = ("Searching...");
                        break;
                }
                counter++;
            }
            */

            DataTable UsernameTable = new DataTable();
            UsernameTable = usernameSearch.generateResults(txtUsername.Text);
            btnUsernameSearch.Content = ("Search");
            dtGrdUsernames.ItemsSource = UsernameTable.DefaultView;
            //dtGrdUsernames.ItemsSource = UsernameTable.Result.DefaultView;
        }
    }
}
