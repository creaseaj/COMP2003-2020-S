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
            Controller.ARPController runCommand = new ARPController();
            command = "netstat";
            args = "-an";
        }

        public string executeCommand(String command, String args)
        {

            String stringOut = "";
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = command;
            startInfo.Arguments = args;
            Process process = Process.Start(startInfo);
            stringOut = process.StandardOutput.ReadToEnd();
            return stringOut;
        }


        private void refreshList()
        {
            portOutput.Clear();
            String result = executeCommand(command, args);
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
            executeCommand(command, args);
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