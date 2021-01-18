using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
            args = "-a";
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
        public string results()
        {
            executeCommand(command, args);
            return portOutput.ToString();
        }

        private void refreshList()
        {
            portOutput.Clear();
            String result = executeCommand(command, args);
        }
    }
}
