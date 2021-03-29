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
            Canvas.SetTop(minTxt, (canvas.ActualHeight *0.8) - MeasureString(minTxt).Height);
            x = MeasureString(maxTxt).Width > MeasureString(minTxt).Width ? Convert.ToInt32(MeasureString(maxTxt).Width) : Convert.ToInt32(MeasureString(minTxt).Width);
            x += spacing;

            
            // Draw graph labels & values
            for (int i = 0; i < ColumnsName.Count(); i++)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = ColumnsName[i];
                textBlock.Foreground = colour;
                canvas.Children.Add(textBlock);
                Canvas.SetLeft(textBlock, x);

                Rectangle rect = new Rectangle { Height = (Convert.ToDouble(ColumnsVal[i])/ Convert.ToDouble(max)) * canvas.ActualHeight * 0.8, Width = MeasureString(textBlock).Width };
                rect.Fill = ColumnsColour[i] == null ? colour : ColumnsColour[i];
                canvas.Children.Add(rect);
                Canvas.SetTop(rect, (canvas.ActualHeight * 0.8) - rect.Height);
                Canvas.SetLeft(rect, x);
                x += Convert.ToInt32(MeasureString(textBlock).Width);
                x += spacing;
                if (i == 0) { y = Convert.ToInt32(canvas.ActualHeight - MeasureString(textBlock).Height); }
                Canvas.SetTop(textBlock, y);

            }

            x = MeasureString(maxTxt).Width > MeasureString(minTxt).Width ? Convert.ToInt32(MeasureString(maxTxt).Width) : Convert.ToInt32(MeasureString(minTxt).Width);
            x += spacing;

            // draw axis lines
            Line yLine = new Line();
            yLine.X1 = x;
            yLine.X2 = x;
            yLine.Y1 = 0;
            yLine.Y2 = (canvas.ActualHeight * 0.8) + 2;
            yLine.StrokeThickness = 4;
            yLine.Stroke = Brushes.Black;
            canvas.Children.Add(yLine);

            Line xLine = new Line();
            xLine.X1 = x - 2;
            xLine.X2 = canvas.ActualWidth;
            xLine.Y1 = canvas.ActualHeight * 0.8;
            xLine.Y2 = canvas.ActualHeight * 0.8;
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
