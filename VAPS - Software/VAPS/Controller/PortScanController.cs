using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace VAPS.Controller
{
    class PortScanController
    {
        List<List<String>> portOutput;
        string command;
        string args;
        public PortScanController()
        {
            portOutput = new List<List<string>>();
            command = "netstat";
            args = "-an";
        }
        public void generateData()
        {
            portOutput.Clear();
            String result = cmdController.executeCommand(command, args);
            String[] toList;
            string[] headers = { "Protocol", "Local Address", "Foreign Address", "State" };
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

                        //toList = new String[1];
                        //toList[0] = editedLine;
                        //portOutput.Add(toList.ToList());

                    }
                    else
                    {
                        toList = new String[1];
                        toList[0] = line;
                        portOutput.Add(toList.ToList());
                    }
                    // String[] toList = Regex.Split(line, @" +");
                    // String[] newPortScan = new string[2];
                    //Array.Copy(toList, 1, newPortScan, 0, 2);
                    //portOutput.Add(newPortScan.ToList());

                    //String[] toList = Regex.Split(line, @"[ ]{ 2,}");

                    //string[] toList = new string[1];
                    

                }
            }
            /*string[] stringSeperator = new string[] { "\r\n" };
            string[] lines = result.Split(stringSeperator, StringSplitOptions.None);
            foreach (string individualResult in lines)
            {
                portOutput.Add(individualResult);
            }
            for (int i = 0; i < 5; i++)
            {
                portOutput.RemoveAt(0);
            }
            */
        }
        public List<List<String>> generateDataTable()
        {
            generateData();
            return portOutput;


        }
        /*
        public string results()
        {
            generateData();
            cmdController.executeCommand(command, args);
            StringBuilder output = new StringBuilder();
            foreach (string line in portOutput)
            {
                output.Append(line);
                output.Append("\n");
            }
            return output.ToString();
        }
        */
        /*
        public List<string> dataPortScan()
        {
            portOutput.Clear();
            String result = cmdController.executeCommand(command, args);
            string[] stringSeperator = new string[] { "\r\n" };
            string[] lines = result.Split(stringSeperator, StringSplitOptions.None);
            foreach (string individualResult in lines)
            {

                //individualResult.Replace("", "VOID");
                portOutput.Add(individualResult);
            }
            for (int i = 0; i < 5; i++)
            {
                portOutput.RemoveAt(0);
            }
            return portOutput;
        }
        */

    }
}