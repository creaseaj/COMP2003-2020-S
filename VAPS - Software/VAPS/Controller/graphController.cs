using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using VAPS.Resources;
namespace VAPS.Controller
{
    class graphController
    {
        graph graph = new graph();
        public void testGraph(Canvas canvas)
        {
            Random rnd = new Random();
            canvas.Children.Clear();
            graph = new graph();
            int max = 100;
            graph.addColumn("Red", rnd.Next(0,max), Brushes.DarkRed);
            graph.addColumn("Orange", rnd.Next(0, max), Brushes.DarkOrange);
            graph.addColumn("Yellow", rnd.Next(0, max), Brushes.Goldenrod);
            graph.addColumn("Green", rnd.Next(0, max), Brushes.DarkGreen);
            graph.addColumn("Blue", rnd.Next(0, max), Brushes.DarkBlue);
            graph.addColumn("Purple", rnd.Next(0, max), Brushes.Purple);
            graph.drawGraph(true, canvas, Brushes.Black);
        }
        public void createPortGraph(Canvas canvas, PortScanController prtScn, DataTable prtScanTable)
        {
            canvas.Children.Clear();
            graph = new graph();
            int[] ports = prtScn.getValues(prtScanTable);
            //List<string> safety = new List<string>();
            graph.addColumn("Safe",ports[0],Brushes.Green);
            graph.addColumn("Medium",ports[1],Brushes.Orange);
            graph.addColumn("Dangerous",ports[2],Brushes.Red);
            //List<Brush> brushes = new List<Brush>();
            // graph.addColumn(safety, arpPorts, brushes);
            graph.drawGraph(false, canvas, Brushes.Black);

        }
        public void createARPGraph(Canvas canvas, ARPController ARPScn, DataTable arpScanTable)
        {
            canvas.Children.Clear();
            graph = new graph();
            int[] arp = ARPScn.getResults(arpScanTable);
            //List<string> safety = new List<string>();
            graph.addColumn("Safe", arp[0], Brushes.Green);
            graph.addColumn("Medium", arp[1], Brushes.Orange);
            graph.addColumn("Dangerous", arp[2], Brushes.Red);
            //List<Brush> brushes = new List<Brush>();
            // graph.addColumn(safety, arpPorts, brushes);
            graph.drawGraph(false, canvas, Brushes.Black);

        }

    }
}
