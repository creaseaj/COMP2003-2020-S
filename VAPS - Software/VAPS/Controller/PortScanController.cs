using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VAPS.Model;

namespace VAPS.Controller
{
    class PortScanController
    {
        List<List<String>> portOutput;
        string command;
        string args;
        PortScanController portScan;
        public PortScanController()
        {
            portOutput = new List<List<string>>();
            command = "netstat";
            args = "-an";
            portScan = this;
        }
        public void generateData()
        {
            portOutput.Clear();
            String result = cmdController.executeCommand(command, args);
            String[] toList;
            string[] headers = { "Protocol", "Port", "Foreign Address", "State", "Description", "Recommendation"};
            portOutput.Add(headers.ToList());
            foreach (string line in Regex.Split(result, "\r\n"))
            {
                if (Regex.IsMatch(line, @"[a-zA-Z]"))
                {
                    if (line.Contains("  "))
                    {
                        //MessageBox.Show("I have more than one space in me!");
                        string editedLine = line;
                        RegexOptions options = RegexOptions.None;
                        Regex regex = new Regex("[ ]{2,}", options);
                        editedLine = regex.Replace(line, ",");

                        String[] splitArray = editedLine.Split(',').ToArray<String>();
                        portOutput.Add(splitArray.ToList());

                    }
                    else
                    {
                        toList = new String[1];
                        toList[0] = line;
                        portOutput.Add(toList.ToList());
                    }
                }
            }

        }
        public List<List<String>> generateDataTable()
        {
            generateData();
            return portOutput;
        }
        public DataTable generateTable(DataTable dataTable)
        {
            List<List<string>> portsList = portScan.generateDataTable();
            //Delete's initially records 1, 2 as these hold data we don't need
            portsList.RemoveAt(1);
            portsList.RemoveAt(1);
            for (int i = 0; i < portsList[0].Count; i++)
            {
                dataTable.Columns.Add(new DataColumn(portsList[0][i]));
            }
            //Delete this record after creating columns as it holds their names, and we no longer need this
            portsList.RemoveAt(0);
            //Go through remaining records and remove the blank value they contain
            foreach (var record in portsList)
            {
                record.RemoveAt(0);
            }
            foreach (var entry in portsList)
            {
                var nextRow = dataTable.NewRow();
                String[] splitProtocol = entry[1].Split(':');
                if (splitProtocol[1] != "")
                {
                    nextRow.SetField(0, entry[0]);
                    nextRow.SetField(1, splitProtocol[1]);
                    nextRow.SetField(2, entry[2]);
                    nextRow.SetField(3, entry[3]);
                    nextRow.SetField(4, "Unknown port");
                    nextRow.SetField(5, "Could be closed");
                    nextRow = checkPort(nextRow);
                    dataTable.Rows.Add(nextRow);
                }
            }
            return dataTable;
        }
        public DataRow checkPort(DataRow nextRow)
        {
            List<List<String>> tempList = new List<List<String>>();
            tempList = Port.Instance.getList();
            foreach (var port in tempList)
            {
                if (nextRow[1].ToString() == port[0])
                {
                    nextRow[4] = port[1];
                    nextRow[5] = port[2];
                }
            }
            return nextRow;
        }
        public int[] getValues(DataTable table)
        {
            int keepOpen = 0;
            int couldClose = 0;
            int shouldClose = 0;
            
            foreach (DataRow row in table.Rows)
            {
                if (row[5].ToString() == "Keep open")
                {
                    keepOpen++;
                }
                if (row[5].ToString() == "Could be closed")
                {
                    couldClose++;
                }
                if (row[5].ToString() == "Should be closed")
                {
                    shouldClose++;
                }
            }
            int[] results = new int[] { keepOpen, couldClose, shouldClose };
            return results;
        }
        public void runPortScan(DataGrid PortScannerDataGrid, TextBlock[] PortScannerBlocks)
        {
            DataTable PortScannerTable = new DataTable();
            PortScannerTable = generateTable(PortScannerTable);
            PortScannerDataGrid.ItemsSource = PortScannerTable.DefaultView;
            PortScannerDataGrid.Visibility = Visibility.Visible;
            int[] results = getValues(PortScannerTable);
            PortScannerBlocks[0].Text = results[0].ToString();
            PortScannerBlocks[1].Text = results[1].ToString();
            PortScannerBlocks[2].Text = results[2].ToString();
        }
    }
}