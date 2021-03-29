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
            graph.addColumn("Test1", 15);
            graph.addColumn("Long Test 1", 10);
            graph.addColumn("Second Test", 30);
            graph.addColumn("Test 3", 40);
            graph.drawGraph(false, canvas, Brushes.Black);
        }
    }
}
