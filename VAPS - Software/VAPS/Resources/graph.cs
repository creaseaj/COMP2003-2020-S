using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace VAPS.Resources
{
    class graph
    {
        private List<String> ColumnsName = new List<string>();      // List of column names
        private List<int> ColumnsVal = new List<int>();             // List of column values
        private List<Brush> ColumnsColour = new List<Brush>();      // List of column colours
        public graph()
        {

        }
        public graph(List<string> columnsName, List<int> columnsVal)
        {
            ColumnsName = columnsName;
            ColumnsVal = columnsVal;
        }
        public void drawGraph(bool animate, Canvas canvas, Brush colour)
        {
            //int colWidth = 40;
            int spacing = 20;
            int x = 0;
            int y = 0;
            double multiplier = 0.9;
            // Draw Values
            int min = ColumnsVal[0], max = 0;
                // Find highest and lowest values
            foreach(int val in ColumnsVal)
            {
                if (val > max) { max = val; }
                if (val < min) { min = val; }
            }
            // Draw highest and lowest value
            TextBlock maxTxt = new TextBlock();
            maxTxt.Text = max.ToString();
            maxTxt.Foreground = colour;
            canvas.Children.Add(maxTxt);
            Canvas.SetLeft(maxTxt, 0);
            Canvas.SetTop(maxTxt, 0);

            TextBlock minTxt = new TextBlock();
            minTxt.Text = "0";
            minTxt.Foreground = colour;
            canvas.Children.Add(minTxt);
            Canvas.SetLeft(minTxt, 0);
            Canvas.SetTop(minTxt, (canvas.ActualHeight *multiplier) - MeasureString(minTxt).Height);
            x = MeasureString(maxTxt).Width > MeasureString(minTxt).Width ? Convert.ToInt32(MeasureString(maxTxt).Width) : Convert.ToInt32(MeasureString(minTxt).Width);
            x += spacing;
            x += spacing;


            // Draw graph labels & values
            for (int i = 0; i < ColumnsName.Count(); i++)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = ColumnsName[i];
                textBlock.Foreground = colour;
                canvas.Children.Add(textBlock);
                Canvas.SetLeft(textBlock, x);
                double speed = animate ? 0.5 : 0;
                drawColumn(Convert.ToInt32(canvas.ActualHeight * multiplier), x, Convert.ToInt32(Convert.ToDouble(ColumnsVal[i]) / Convert.ToDouble(max) * canvas.ActualHeight * multiplier), Convert.ToInt32(MeasureString(textBlock).Width),ColumnsColour[i] == null ? colour : ColumnsColour[i],speed,canvas);
                //Rectangle rect = new Rectangle { Height = (Convert.ToDouble(ColumnsVal[i])/ Convert.ToDouble(max)) * canvas.ActualHeight * multiplier, Width = MeasureString(textBlock).Width };
                //rect.Fill = ColumnsColour[i] == null ? colour : ColumnsColour[i];
                //canvas.Children.Add(rect);
                //Canvas.SetTop(rect, (canvas.ActualHeight * multiplier) - rect.Height);
                //Canvas.SetLeft(rect, x);
                x += Convert.ToInt32(MeasureString(textBlock).Width);
                x += spacing;
              
                if (i == 0) { y = Convert.ToInt32(canvas.ActualHeight * multiplier); }
                Canvas.SetTop(textBlock, y);

            }

            x = MeasureString(maxTxt).Width > MeasureString(minTxt).Width ? Convert.ToInt32(MeasureString(maxTxt).Width) : Convert.ToInt32(MeasureString(minTxt).Width);
            x += spacing;

            // draw axis lines
            Line yLine = new Line();
            yLine.X1 = x;
            yLine.X2 = x;
            yLine.Y1 = 0;
            yLine.Y2 = (canvas.ActualHeight * multiplier) + 2;
            yLine.StrokeThickness = 4;
            yLine.Stroke = Brushes.Black;
            canvas.Children.Add(yLine);

            Line xLine = new Line();
            xLine.X1 = x - 2;
            xLine.X2 = canvas.ActualWidth;
            xLine.Y1 = canvas.ActualHeight * multiplier;
            xLine.Y2 = canvas.ActualHeight * multiplier;
            xLine.StrokeThickness = 4;
            xLine.Stroke = Brushes.Black;
            canvas.Children.Add(xLine);
        }
        public void addColumn(List<string> columnsName, List<int> columnsVal, List<Brush> columnsColour)
        {
            ColumnsName.AddRange(columnsName);
            ColumnsVal.AddRange(columnsVal);
            ColumnsColour.AddRange(columnsColour);
        }
        public void addColumn(string columnName, int columnVal, Brush columnColour)
        {
            ColumnsName.Add(columnName);
            ColumnsVal.Add(columnVal);
            ColumnsColour.Add(columnColour);
        }
        private async Task drawColumn(double top, double left, double height, double width, Brush colour, double time, Canvas canvas)
        {
            if (time != 0)
            {

                int framespeed = 60;
                double frames = time * framespeed;
                double rectHeight = Convert.ToDouble(height) / Convert.ToDouble(frames);
                Rectangle rect = new Rectangle { Height = rectHeight, Width = width };
                rect.Fill = colour;
                canvas.Children.Add(rect);
                Canvas.SetTop(rect, top);
                Canvas.SetLeft(rect, left);
                for (int i = 0; i < frames; i++)
                {
                    await Task.Delay(1000 / framespeed);
                    rect.Height = i * rectHeight;
                    Canvas.SetTop(rect, top - i * rectHeight);

                }
            }
            else
            {
                Rectangle rect = new Rectangle { Height = height, Width = width };
                rect.Fill = colour;
                canvas.Children.Add(rect);
                Canvas.SetTop(rect, top - height);
                Canvas.SetLeft(rect, left);
            }

        }
        private Size MeasureString(TextBlock text)
        {
            var formattedText = new FormattedText(
                text.Text,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(text.FontFamily, text.FontStyle, text.FontWeight, text.FontStretch),
                text.FontSize,
                Brushes.Black,
                new NumberSubstitution(),
                1);

            return new Size(formattedText.Width, formattedText.Height);
        }
    }
}
