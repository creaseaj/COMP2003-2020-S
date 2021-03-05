using System;
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
//using VAPS.Resources;
namespace VAPS.View
{
    /// <summary>
    /// Interaction logic for CoreWindow.xaml
    /// </summary>
    public partial class CoreWindow : Window
    {
        ARPController network;
        PortScanController scanController;
        public CoreWindow()
        {
            InitializeComponent();
            var mainForm = this;
            network = new ARPController();
            scanController = new PortScanController();

            //The visibilities are used in development, this code is likely to be removed and the items set to hidden in release
            arpGrid.Visibility = Visibility.Hidden;
            txtPortOutput.Visibility = Visibility.Hidden;
        }

        private void btnARP_Click(object sender, RoutedEventArgs e)
        {
            DataTable dataTable = new DataTable();
            List<List<string>> arpList = network.getARPRaw();
            for(int i = 0; i < arpList[0].Count; i++) {
                dataTable.Columns.Add(new DataColumn(arpList[0][i]));            
            }
            //ARPWindow.ShowDialog();
            for(int i = 1; i < arpList.Count; i++)
            {
                var newRow = dataTable.NewRow();
                for(int j = 0; j < arpList[i].Count; j++)
                {
                    newRow[arpList[0][j]] = arpList[i][j];
                }
                dataTable.Rows.Add(newRow);
            }
            arpGrid.ItemsSource = dataTable.DefaultView;
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
            txtPortOutput.Text = scanController.results();
            txtPortOutput.Visibility = Visibility.Visible;
        }

        private void tabCon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void scnNtwrkClk(object sender, RoutedEventArgs e)
        {
            nmapOut.Text = new nmapController().scanLocal();
        }
    }
}
