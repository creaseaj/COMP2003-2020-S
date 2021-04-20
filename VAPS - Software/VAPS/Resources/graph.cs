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
            Double CanvasHeight = canvas.ActualHeight;
            //int colWidth = 40;
            int spacing = 20;
            int x = 0;
            int y = 0;
            double multiplier = 0.9;
            double graphHeight = CanvasHeight * multiplier;
            // Draw Values
            int min = ColumnsVal[0], max = 0;
                // Find highest and lowest values
            foreach(int val in ColumnsVal)
            {
                if (val > max) { max = roundUpTen(val); }
                if (val < min) { min = val; }
            }
            TextBlock maxTxt = new TextBlock();
            maxTxt.Text = max.ToString();
            maxTxt.Foreground = colour;
            double maxWidth = MeasureString(maxTxt).Width;
            maxWidth = 40;
            // Draw Axis Labels
            int axisSpacing = findAxisSpacing(max);
            int newMax = 0;
            while(newMax < max)
            {
                newMax += axisSpacing;
            }
            max = newMax;
            for(int i = 0; i <= max ; i+=axisSpacing)
            {
                TextBlock txtBlk = new TextBlock();
                txtBlk.Text = (max - i).ToString();
                txtBlk.Foreground = colour;
                canvas.Children.Add(txtBlk);

                if(i == 0)
                {
                    maxWidth = MeasureString(txtBlk).Width;
                }

                Canvas.SetLeft(txtBlk, maxWidth - MeasureString(txtBlk).Width);
                Canvas.SetTop(txtBlk, ((double)i / (double)max) * graphHeight - MeasureString(txtBlk).Height / 2 + spacing);

                Line axisLine = new Line();
                axisLine.X1 = maxWidth + 5;
                axisLine.X2 = canvas.ActualWidth;
                axisLine.Y1 = ((double)i / (double)max) * graphHeight + spacing + 2;
                axisLine.Y2 = ((double)i / (double)max) * graphHeight + spacing + 2;
                axisLine.StrokeThickness = 2;
                axisLine.Stroke = Brushes.Black;
                canvas.Children.Add(axisLine);
            }
           
            //canvas.Children.Add(maxTxt);
            //Canvas.SetLeft(maxTxt, 0);
            //Canvas.SetTop(maxTxt, 0);

            TextBlock minTxt = new TextBlock();
            minTxt.Text = "0";
            minTxt.Foreground = colour;
            //canvas.Children.Add(minTxt);
            //Canvas.SetLeft(minTxt, 0);
            //Canvas.SetTop(minTxt, (canvas.ActualHeight *multiplier) - MeasureString(minTxt).Height);
            x = Convert.ToInt32(maxWidth);
            x += spacing;

            // Draw graph labels & values
            for (int i = 0; i < ColumnsName.Count(); i++)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = ColumnsName[i];
                textBlock.Foreground = colour;
                canvas.Children.Add(textBlock);
                Canvas.SetLeft(textBlock, x);
                double speed = animate ? 0.2 : 0;
                int columnMaxHeight = Convert.ToInt32(canvas.ActualHeight * multiplier);
                int currentColumnHeight = Convert.ToInt32((Convert.ToDouble(ColumnsVal[i]) / Convert.ToDouble(max)) * columnMaxHeight);
                int columnWidth = Convert.ToInt32(MeasureString(textBlock).Width);
                Brush columnColour = ColumnsColour[i] == null ? colour : ColumnsColour[i];
                drawColumn(columnMaxHeight + spacing, x, currentColumnHeight, columnWidth , columnColour,speed,canvas);
                x += Convert.ToInt32(MeasureString(textBlock).Width);
                x += spacing;
              
                if (i == 0) { y = Convert.ToInt32(canvas.ActualHeight * multiplier); }
                Canvas.SetTop(textBlock, y + spacing);

            }

            x = Convert.ToInt32(maxWidth) + 5;
            

            // draw axis lines
            Line yLine = new Line();
            yLine.X1 = x;
            yLine.X2 = x;
            yLine.Y1 = spacing + 1;
            yLine.Y2 = (canvas.ActualHeight * multiplier) + 3 + spacing;
            yLine.StrokeThickness = 4;
            yLine.Stroke = Brushes.Black;
            canvas.Children.Add(yLine);

            Line xLine = new Line();
            xLine.X1 = x - 2;
            xLine.X2 = canvas.ActualWidth;
            xLine.Y1 = canvas.ActualHeight * multiplier + spacing;
            xLine.Y2 = canvas.ActualHeight * multiplier + spacing;
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
        private int findAxisSpacing(int maxValue)
        {
            float[] spaceValues = {0.1f,0.2f, 0.5f };
            int bestSpacing = 1;
            int unrounded = maxValue / 15;
            int counter = 0;
            while (bestSpacing < unrounded)
            {
                bestSpacing = Convert.ToInt32(Math.Pow(10,counter / 3) * spaceValues[counter % 3]);
                counter++;
            }

            return bestSpacing;
        }
        private int roundUpTen(int numIn)
        {
            if(numIn % 10 == 0)
            {
                return numIn;
            }
            else
            {
                return (10 - numIn % 10) + numIn;
            }
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
                for (int i = 0; i < frames - 1; i++)
                    {
                    await Task.Delay(1000 / framespeed);
                    rect.Height += rectHeight;
                    Canvas.SetTop(rect, top - rect.Height);

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
