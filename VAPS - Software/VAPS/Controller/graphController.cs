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
            Random rnd = new Random();
            canvas.Children.Clear();
            graph = new graph();
            graph.addColumn("Red", rnd.Next(0,100000), Brushes.DarkRed);
            graph.addColumn("Orange", rnd.Next(0, 100000), Brushes.DarkOrange);
            graph.addColumn("Yellow", rnd.Next(0, 100000), Brushes.Goldenrod);
            graph.addColumn("Green", rnd.Next(0, 100000), Brushes.DarkGreen);
            graph.addColumn("Blue", rnd.Next(0, 100000), Brushes.DarkBlue);
            graph.addColumn("Purple", rnd.Next(0, 100000), Brushes.Purple);
            graph.drawGraph(true, canvas, Brushes.Black);
        }
    }
}
