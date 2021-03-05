using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace VAPS.Controller
{
    class nmapController
    {
        public string fingerPrint(string ip, string subnet){
            string output = cmdController.executeCommand("nmap", " -O " + ip + subnet);
            Console.WriteLine(ipToBinary(ip));
            return output;
        }
        private string ipToBinary(string ipIn)
        {
            string[] ipSplit = Regex.Split(ipIn, @"\.");
            string binaryIP = "";
            foreach(string octet in ipSplit)
            {
                string nextSec = "";
                Console.WriteLine("Converting " + octet);
                nextSec = Convert.ToString(Convert.ToInt32(octet),2);
                binaryIP += nextSec;
                for(int i = 0; i < 8 - nextSec.Length ; i++)
                {
                    binaryIP += "0";
                }
            }
            return binaryIP;
        }
        private bool isInstalled()
        {
            string output = cmdController.executeCommand("nmap","");
            string failString = "System.ComponentModel.Win32Exception";
            if (output.Contains(failString))
            {
                return false;
            }
            return true;
        }
        public string scanLocal()
        {
            if (isInstalled())
            {
                string output = cmdController.executeCommand("nmap", "192.168.0.*");
                return output;
            }
            else
            {
                MessageBox.Show("NMap is not installed. Click the link below to install NMap.");
                return "";
            }
        }

    }
}
