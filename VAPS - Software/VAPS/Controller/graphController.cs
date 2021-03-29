using System;
using System.Collections.Generic;
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
            canvas.Children.Clear();
            graph = new graph();
            graph.addColumn("Registered", 15, Brushes.Green);
            graph.addColumn("Known", 10, Brushes.Orange);
            graph.addColumn("Unknown", 30, Brushes.Red);
            graph.addColumn("Other", 40, null);
            graph.drawGraph(true, canvas, Brushes.Black);
        }
    }
}
