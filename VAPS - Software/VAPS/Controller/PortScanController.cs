using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VAPS.Controller
{
    class PortScanController
    {
        List<String> portOutput;
        string command;
        string args;
        public PortScanController()
        {
            portOutput = new List<string>();
            command = "netstat";
            args = "-an";
        }

       


        private void refreshList()
        {
            portOutput.Clear();
            String result = cmdController.executeCommand(command, args);
            string[] stringSeperator = new string[] { "\r\n" };
            string[] lines = result.Split(stringSeperator, StringSplitOptions.None);
            foreach (string individualResult in lines)
            {
                portOutput.Add(individualResult);
            }
            for (int i = 0; i < 5; i++)
            {
                portOutput.RemoveAt(0);
            }
            //int[] lengths = new int[portOutput[0].Count];
            //for (int i = 0; i < portOutput.Count; i++)
            //{
            //    for (int j = 0; j < portOutput.Count; j++)
            //    {
            //        int size = portOutput[i][j].Length;
            //        lengths[j] = size > lengths[j] ? size : lengths[j];
            //    }
            //}
        }
        public string results()
        {
            refreshList();
            cmdController.executeCommand(command, args);
            StringBuilder output = new StringBuilder();
            foreach (string line in portOutput)
            {
                output.Append(line);
                output.Append("\n");
            }
            return output.ToString();
        }

        
    }
}