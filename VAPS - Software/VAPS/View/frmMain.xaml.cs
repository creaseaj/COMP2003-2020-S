using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
        public CoreWindow()
        {
            InitializeComponent();
            var mainForm = this;
            ARP = new ARPController();
            PortScan = new PortScanController();
            Port.Instance.fileInput();
            Device.Instance.fileInput();

            //The visibilities are used in development, this code is likely to be removed and the items set to hidden in release
            arpGrid.Visibility = Visibility.Hidden;
            txtARPDeviceName.IsEnabled = false;
            btnARPAddName.IsEnabled = false;
            txtARPDeviceName.Visibility = Visibility.Hidden;
            btnARPAddName.Visibility = Visibility.Hidden;
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

        private void txtARPDeviceName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
