using System;
using System.Collections.Generic;
using System.Data;
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
        ARPController ARP;
        PortScanController PortScan;
        public CoreWindow()
        {
            InitializeComponent();
            var mainForm = this;
            ARP = new ARPController();
            PortScan = new PortScanController();
            


            //The visibilities are used in development, this code is likely to be removed and the items set to hidden in release
            arpGrid.Visibility = Visibility.Hidden;
        }

        private void btnARP_Click(object sender, RoutedEventArgs e)
        {
            DataTable ARPTable = new DataTable();
            List<List<string>> arpList = ARP.getARPRaw();
            for(int i = 0; i < arpList[0].Count; i++) {
                ARPTable.Columns.Add(new DataColumn(arpList[0][i]));            
            }

            for(int i = 1; i < arpList.Count; i++)
            {
                var newRow = ARPTable.NewRow();
                for(int j = 0; j < arpList[i].Count; j++)
                {
                    newRow[arpList[0][j]] = arpList[i][j];
                }
                ARPTable.Rows.Add(newRow);
            }
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
            List<List<string>> portsList = PortScan.generateDataTable();
            //Delete's initially records 1, 2 as these hold data we don't need
            portsList.RemoveAt(1);
            portsList.RemoveAt(1);
            for (int i = 0; i < portsList[0].Count; i++)
            {
                PortScannerTable.Columns.Add(new DataColumn(portsList[0][i]));
            }
            //Delete this record after creating columns as it holds their names, and we no longer need this
            portsList.RemoveAt(0);
            //Go through remaining records and remove the blank value they contain
            foreach (var record in portsList)
            {
                record.RemoveAt(0);
            }
            foreach (var entry in portsList)
            {
                var nextRow = PortScannerTable.NewRow();
                nextRow.SetField(0, entry[0]);
                nextRow.SetField(1, entry[1]);
                nextRow.SetField(2, entry[2]);
                nextRow.SetField(3, entry[3]);
                PortScannerTable.Rows.Add(nextRow);
            }
            PortScannerDataGrid.ItemsSource = PortScannerTable.DefaultView;
            PortScannerDataGrid.Visibility = Visibility.Visible;
            
        }

        private void tabCon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
