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

namespace VAPS.View
{
    /// <summary>
    /// Interaction logic for PortScan.xaml
    /// </summary>
    public partial class PortScan : Window
    {
        Controller.PortScanController scanController;
        public PortScan()
        {
            InitializeComponent();
            scanController = new Controller.PortScanController();
        }

        private void viewNetwork(object sender, RoutedEventArgs e)
        {
            scanController.results();
        }
    }
}
