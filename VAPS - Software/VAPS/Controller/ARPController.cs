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
    class ARPController
    {
        List<List<String>> arpList;
        public ARPController()
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
            foreach (String line in Regex.Split(unParsedList, "\r\n"))
            {
                if (Regex.IsMatch(line, @"^ *\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"))
                {
                    String[] toArpList = Regex.Split(line, @" +");
                    String[] newArp = new string[3];
                    Array.Copy(toArpList,1,newArp,0,3);
                    arpList.Add(newArp.ToList());
                }
            }
        }
        public String getArpList()
        {
            updateArpList();
            List<List<String>> arpOut = new List<List<string>>();
            string[] headers = {"IP Address","Physical Address","Type"};
            arpOut.Add(headers.ToList());
            arpOut.AddRange(arpList);
            int[] lengths = new int[arpList[0].Count];
            // finding longest item in each column
            for(int i = 0; i < arpList.Count; i++)
            {
                for(int j = 0; j < arpList[i].Count; j++)
                {
                    int size = arpList[i][j].Length;
                    lengths[j] = size > lengths[j] ? size : lengths[j];
                }
            }
            StringBuilder stringOut = new StringBuilder();
            //String stringOut = "";
            foreach (List<String> line in arpOut)
            {
                for(int i = 0; i < line.Count ; i ++)
                {
                    stringOut.Append(line[i] );
                    for(int j = 0; j < lengths[i] -line[i].Length; j++){stringOut.Append(' ');}
                    stringOut.Append(" |");
                }
                stringOut.Append("\n");
            }
            return stringOut.ToString();
        }
    }
}
