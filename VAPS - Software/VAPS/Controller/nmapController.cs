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
            // find range of ip addresses using the subnet and ip address
            string[] ipRange = getIPRange(ipToBinary(ip), ipToBinary(subnet));
            string output = cmdController.executeCommand("nmap",  ipRange[0] + "-" + ipRange[1].Substring(ipRange[1].Length - 3, 3));
            return output;
        }
        private string[] getIPRange(string ip, string subnet)
        {
            StringBuilder stringOut = new StringBuilder();
            string[] ipRange = new string[2];
            for(int i = 0; i < ip.Length; i++)
            {
                if (ip[i] == '1' & subnet[i] == '1'){
                    stringOut.Append("1");
                }
                else if(subnet[i] == '1')
                {
                    stringOut.Append("0");
                }
            }
            ipRange[0] = stringOut.ToString();
            ipRange[1] = stringOut.ToString();
            for(int i = 0; i < 32 - stringOut.Length; i++)
            {
                ipRange[0] += "0";
                ipRange[1] += "1";
            }
            ipRange[0] = binaryToIP(ipRange[0]);
            ipRange[1] = binaryToIP(ipRange[1]);
            return ipRange;
        }
        private string invertBinary(string ipIn)
        {
            StringBuilder stringOut = new StringBuilder();
            foreach(char item in ipIn)
            {
                switch (item)
                {
                    case '0':
                        stringOut.Append("1");
                        break;
                    case '1':
                        stringOut.Append("0");
                        break;
                }
            }
            return stringOut.ToString();
        }
        private string binaryToIP(string binIn)
        {
            string toConvert;
            int decimalOctet;
            StringBuilder ipOut = new StringBuilder();
            for(int i = 0; i < 4; i++)
            {
                decimalOctet = 0;
                toConvert = binIn.Substring(i * 8,8);
                for(int j = 0; j < toConvert.Length; j++)
                {
                    if(toConvert[j] == '1')
                    {
                        decimalOctet += Convert.ToInt32(Math.Pow(2,8 - j - 1));
                    }
                }
                ipOut.Append(decimalOctet.ToString());
                if(i < 3)
                {
                    ipOut.Append(".");
                }
            }
            return ipOut.ToString();
        }
        private string ipToBinary(string ipIn)
        {
            string[] ipSplit = Regex.Split(ipIn, @"\.");
            string binaryIP = "";
            foreach(string octet in ipSplit)
            {
                string nextSec = "";
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
        
        public string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

    }
}
