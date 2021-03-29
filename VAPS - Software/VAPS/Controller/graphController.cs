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
            graph.addColumn("Test1", 15, Brushes.Red);
            graph.addColumn("Long Test 1", 10, null);
            graph.addColumn("Second Test", 30, Brushes.Aqua);
            graph.addColumn("Test 3", 40, Brushes.DarkTurquoise);
            graph.drawGraph(false, canvas, Brushes.Black);
        }
    }
}
