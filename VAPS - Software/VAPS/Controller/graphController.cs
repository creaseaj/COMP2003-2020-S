using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using VAPS.Resources;
namespace VAPS.Controller
{
    class graphController
    {
        graph graph = new graph();
        Canvas canvas;
        public void testGraph(Canvas canvas1)
        {
            canvas = canvas1;
            Random rnd = new Random();
            canvas.Children.Clear();
            graph = new graph();
            graph.addColumn("Red", rnd.Next(0,100), Brushes.DarkRed);
            graph.addColumn("Orange", rnd.Next(0, 100), Brushes.DarkOrange);
            graph.addColumn("Yellow", rnd.Next(0, 100), Brushes.Goldenrod);
            graph.addColumn("Green", rnd.Next(0, 100), Brushes.DarkGreen);
            graph.addColumn("Blue", rnd.Next(0, 100), Brushes.DarkBlue);
            graph.addColumn("Purple", rnd.Next(0, 100), Brushes.Purple);
            graph.drawGraph(true, canvas, Brushes.Black);
        }

        public void buttonPressed(MouseButtonEventArgs mouseButtonEventArgs)
        {
            if(mouseButtonEventArgs.OriginalSource is Rectangle)
            {
                Rectangle clickedRect = (Rectangle)mouseButtonEventArgs.OriginalSource;
                graph.itemClicked(clickedRect, canvas);
            }
        }
    }
}
