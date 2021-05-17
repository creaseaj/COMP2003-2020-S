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
        //Declarations
        ARPController ARP;
        PortScanController PortScan;

        graphController graphControl;

        UsernameSearchController usernameSearch;
        PasswordTesterController PasswordTesting;
        Image[] passwordImages;
        TextBlock[] ARPTextBlocks;
        TextBlock[] PortScannerBlocks;
        nmapController NMap;

        public CoreWindow()
        {
            InitializeComponent();

            //Controllers
            ARP = new ARPController();
            PortScan = new PortScanController();

            graphControl = new graphController();
            PasswordTesting = new PasswordTesterController();
            //Port.Instance.fileInput();
            //Device.Instance.fileInput();

            nController = new nmapController();
            //Port.Instance.fileInput();
            graphControl.runUpdates(ARPgraphCanvas, portCanvas, ARP, PortScan, ARP.formatTable(new DataTable()));

            usernameSearch = new UsernameSearchController();
            PasswordTesting = new PasswordTesterController();
            NMap = new nmapController();

            //Initial Fileinput
            Port.Instance.fileInput();
            Device.Instance.fileInput();
            usernameSearch.fileInput();

            //GUI items in arrays, easier to pass into controllers
            passwordImages = new Image[] { imgLength, imgLower, imgNumber, imgPassword, imgSpecial, imgUpper };
            ARPTextBlocks = new TextBlock[] { blockARPKnown, blockARPRegistered, blockARPUnknown };
            PortScannerBlocks = new TextBlock[] { txtBlockOpenNum, txtBlockCouldNum, txtBlockShouldNum };



            //The visibilities are used in development, this code is likely to be removed and the items set to hidden in release
            arpGrid.Visibility = Visibility.Hidden;
            txtARPDeviceName.IsEnabled = false;
            btnARPAddName.IsEnabled = false;
            txtARPDeviceName.Visibility = Visibility.Hidden;
            btnARPAddName.Visibility = Visibility.Hidden;
            dtGrdUsernames.Visibility = Visibility.Hidden;
            txBlockUsernameResult.Visibility = Visibility.Hidden;
        }

        private void btnARP_Click(object sender, RoutedEventArgs e)
        {
            ARP.ARPScan(btnRun1, arpGrid, ARPTextBlocks, txtARPDeviceName, btnARPAddName);
        }
      

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnPortScanner_Click(object sender, RoutedEventArgs e)
        {
            PortScan.runPortScan(PortScannerDataGrid, PortScannerBlocks);
        }

        private void tabCon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabARP != null && tabARP.IsSelected)
            {
                if (arpGrid.Items.Count <= 0)
                {
                    ARP.ARPScan(btnRun1, arpGrid, ARPTextBlocks, txtARPDeviceName, btnARPAddName);
                }
                
            }
            else if (tabPortScanner != null && tabPortScanner.IsSelected)
            {
                PortScan.runPortScan(PortScannerDataGrid, PortScannerBlocks);
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
            ARP.addDeviceName(arpGrid, txtARPDeviceName);
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordTesting.passwordDisplay(pwdPasswordInput, txtTimeToCrack, passwordImages, txtBlockCleartext);
        }

        private void btnShowClear_Click(object sender, RoutedEventArgs e)
        {
            PasswordTesting.displayCleartext(txtBlockCleartext, pwdPasswordInput);
        }

        private void scnNtwrkClk(object sender, RoutedEventArgs e)
        {
            if (authChkBx.IsChecked.Value)
            {
                NMap.nmapScanningProgress(nmapInstall, nmapOutGrid);

            }
            else
            {
                MessageBox.Show("You must have permission from the network adminstrator to perform this action");
            }

        }
        private void ipsubShow_Click(object sender, RoutedEventArgs e)
        {

            string ipaddress = nController.GetLocalIPAddress();
            //nmapOut.Text = (ipaddress);

            /// nController.Subnet();

            //nmapOut.Text += nController.getSubnetFromIP(ipaddress);

        }

        }

        private void btnUsernameSearch_Click(object sender, RoutedEventArgs e)
        {
            usernameSearch.runUsernameSearch(txtUsername.Text, btnUsernameSearch, dtGrdUsernames, txBlockUsernameResult);
        }

    }
}
