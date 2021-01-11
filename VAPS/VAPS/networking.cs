using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace VAPS
{
    class networking
    {
        List<List<String>> arpList;
        public networking()
        {
            arpList = new List<List<String>>();
        }
        
        private string executeCommand(String command, String args)
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
        private void updateArpList()
        {
            arpList.Clear();
            String unParsedList = executeCommand("arp", "-a");
            foreach(String line in Regex.Split(unParsedList,"\r\n"))
            {
                if(Regex.IsMatch(line, @"^ *\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"))
                {
                    arpList.Add(Regex.Split(line, @" +").ToList());
                }
            }
        }
        public String getArpList()
        {
            updateArpList();
            String stringOut = "";
            foreach(List<String> line in arpList)
            {
                foreach(String item in line)
                {
                    stringOut += item + " ";
                }
                stringOut += "\n";
            }
            return stringOut;
        }
    }
}
