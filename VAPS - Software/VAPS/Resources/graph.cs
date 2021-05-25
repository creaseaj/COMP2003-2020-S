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

            x = Convert.ToInt32(maxWidth);
            x += spacing;

            // Draw graph labels & values
            drawColumns(colour, canvas, animate, spacing, spacing, spacing + Convert.ToInt32(canvas.ActualHeight * multiplier), x, canvas.Width,max);

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

            //Rectangle test = new Rectangle { Height = 50, Width = 50 };                    //  box test
            //test.Name = "Left";
            //test.Fill = Brushes.Green;
            //canvas.Children.Add(test);

            //Rectangle right = new Rectangle { Height = 50, Width = 50 };
            //right.Name = "Right";
            //right.Fill = Brushes.Red;
            //canvas.Children.Add(right);
            //Canvas.SetLeft(right, 50);
            //test.MouseLeftButtonDown = rotateColumns();
        }
        public void itemClicked(Rectangle objectClicked, Canvas canvas)
        {
            if(objectClicked.Name == "Left")
            {
                rotateLeft(canvas);
            }
            else if(objectClicked.Name == "Right")
            {
                rotateRight(canvas);
            }
        }
        private void rotateRight(Canvas canvas)
        {
            for(int i = 1; i < ColumnsName.Count; i++)
            {
                rotateGraphs(canvas);
            }
            canvas.Children.Clear();
            drawGraph(false, canvas, Brushes.Black);
        }
        private void rotateLeft(Canvas canvas)
        {
            rotateGraphs(canvas);
            canvas.Children.Clear();
            drawGraph(false, canvas, Brushes.Black);
        }
        private void rotateGraphs( Canvas canvas)
        {
            string spacerName = "";
            int spacerVal = 0 ;
            int movement = -1;
            Brush spacerColour = Brushes.Black;
            for(int i = 0; i < ColumnsName.Count; i++)
            {
                
                 if (i + movement < 0)
                    {
                    spacerName = ColumnsName[ColumnsName.Count + (i + movement)];
                    spacerVal = ColumnsVal[ColumnsName.Count + (i + movement)];
                    spacerColour = ColumnsColour[ColumnsName.Count + (i + movement)];

                    ColumnsName[ColumnsName.Count + (i + movement)] = ColumnsName[i];
                    ColumnsVal[ColumnsName.Count + (i + movement)] = ColumnsVal[i];
                    ColumnsColour[ColumnsName.Count + (i + movement)] = ColumnsColour[i];
                }
                else if (i == ColumnsName.Count - 1)
                {
                    
                        ColumnsName[i + movement] = spacerName;
                        ColumnsVal[i + movement] = spacerVal;
                        ColumnsColour[i + movement] = spacerColour;
                }
                else
                {
                    ColumnsName[i + movement] = ColumnsName[i];
                    ColumnsVal[i + movement] = ColumnsVal[i];
                    ColumnsColour[i + movement] = ColumnsColour[i];
                }
            }
        }
        private void drawColumns(Brush colour, Canvas canvas, Boolean animate , double spacing, double top, double bottom, double left, double right, double highestVal )
        { 
            for (int i = 0; i < ColumnsName.Count(); i++)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = ColumnsName[i];
                textBlock.Foreground = colour;
                canvas.Children.Add(textBlock);
                Canvas.SetLeft(textBlock, left);
                double speed = animate ? 0.2 : 0;
                double columnMaxHeight = bottom - top;
                double currentColumnHeight = ColumnsVal[i] / highestVal * columnMaxHeight;
                int columnWidth = Convert.ToInt32(MeasureString(textBlock).Width);
                Brush columnColour = ColumnsColour[i] == null ? colour : ColumnsColour[i];
                drawColumn(bottom, left, currentColumnHeight, columnWidth, columnColour, speed, canvas);
                left += Convert.ToInt32(MeasureString(textBlock).Width);
                left += spacing;

                //if (i == 0) { y = Convert.ToInt32(canvas.ActualHeight * multiplier); }
                Canvas.SetTop(textBlock, bottom);

            }
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
