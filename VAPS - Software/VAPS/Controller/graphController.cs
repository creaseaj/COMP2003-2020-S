using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
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
        public async Task runUpdates(Canvas arpCanvas, Canvas portCanvas, ARPController ARPScn, PortScanController PORTScn, DataTable arpScanTable)
        {
            int counter = 0;
            // Task runs in the background which is the nmap function
            while (true)
            {
                //Runs while nmap command is still running, shows scanning dots

                await Task.Delay(1000);
                if (counter % 60 == 10)
                {
                    arpCanvas.Children.Clear();
                    Task<int[]> arp = Task.Run(() =>
                    {
                        return ARPScn.getResults(arpScanTable);
                    });
                    Task<int[]> port = Task.Run(() =>
                    {
                        return PORTScn.getValues(PORTScn.generateTable(new DataTable()));
                    });
                    port.Wait();
                    arp.Wait();
                    arpCanvas.Children.Clear();
                    graph arpGraph = new graph();
                    arpGraph.addColumn("Safe", arp.Result[0], Brushes.Green);
                    arpGraph.addColumn("Medium", arp.Result[1], Brushes.Orange);
                    arpGraph.addColumn("Unsafe", arp.Result[2], Brushes.Red);
                    arpGraph.drawGraph(false, arpCanvas, Brushes.Black);
                    portCanvas.Children.Clear();

                    graph PortGraph = new graph();
                    PortGraph.addColumn("Safe", port.Result[0], Brushes.Green);
                    PortGraph.addColumn("Medium", port.Result[1], Brushes.Orange);
                    PortGraph.addColumn("Unsafe", port.Result[2], Brushes.Red);
                    PortGraph.drawGraph(false, portCanvas, Brushes.Black);
                }
                counter++;
            }

        }
        

    }
}
