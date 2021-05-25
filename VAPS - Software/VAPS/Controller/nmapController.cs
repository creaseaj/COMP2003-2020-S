using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace VAPS.Controller
{
    class nmapController
    {
        public DataTable fingerPrint(string ip, string subnet){
            // find range of ip addresses using the subnet and ip address
            string[] ipRange = getIPRange(ipToBinary(ip), ipToBinary(subnet));
            string output = cmdController.executeCommand("nmap", " -O " +  ipRange[0] + "-" + ipRange[1].Substring(ipRange[1].Length - 3, 3));
            return generateTable(output);
        }
        private string[] getIPRange(string ip, string subnet)
        {
            StringBuilder stringOut = new StringBuilder();
            string[] ipRange = new string[2];
            for(int i = 0; i < ip.Length; i++)
            {
                stringOut.Append(ip[i] == '1' && subnet[i] == '1' ? "1" : subnet[i] =='1' ? "0" : "");
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
        private DataTable generateTable(string fingerPrintOut)
        {
            DataTable tabOut = new DataTable();
            
            List<string> fingerPrintList = Regex.Split(fingerPrintOut,"\r\n\r\n").ToList();
            fingerPrintList.Remove(fingerPrintList[fingerPrintList.Count - 1]); // removes last item from list
            string[] columnNames = { "IP Address", "OS Version", "Open Ports", "Vulnerability Indicator" };
            foreach(string name in columnNames)
            {
                tabOut.Columns.Add(new DataColumn(name));
            }
            foreach (string fingerPrint in fingerPrintList)
            {
                if(Regex.Split(fingerPrint,@"\n").Length != 1)
                {
                    var newRow = tabOut.NewRow();
                    newRow[0] = Regex.Match(fingerPrint, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}").Value;
                    newRow[3] = Regex.Match(fingerPrint, @"(?<=OS details: ).+").Success ? getCVECount(Regex.Match(fingerPrint, @"(?<=OS details: )([^\d+ - \d+]|\s)+").Value + Regex.Match(fingerPrint, @"(?<=OS details: ).+ \d+ - (\d+)").Groups[1]) : "No OS Detected";
                    newRow[1] = Regex.Match(fingerPrint, @"Running: \w+(\s+|\w+)+(?:\n)").Success ? Regex.Match(fingerPrint, @"Running: \w+(\s+|\w+)+(?:\n)").Value.Substring(9) : "Not Detected";
                    newRow[2] = (1000 - Int32.Parse(Regex.Match(fingerPrint, @"(?<=.)\d+(?=.+ports)").Value)).ToString();
                    tabOut.Rows.Add(newRow);
                }
                
            }
            
            return tabOut;
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
                
                for(int i = 0; i < 8 - nextSec.Length ; i++)
                {
                    binaryIP += "0";
                }
                binaryIP += nextSec;
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
        public string getSubnetFromIP(string ipIn)
        {
            string test;
            test = "192.168";
            string stringOut = "";
            if (ipIn.StartsWith(test))
            {
                stringOut = ("\n255.255.255.0");
            }
            test = "172.";

            if (ipIn.StartsWith(test))
            {
                stringOut =  ("\n255.255.0.0");
            }
            test = "10.";

            if (ipIn.StartsWith(test))
            {
                stringOut =  ("\n255.0.0.0");
            }

            return stringOut;
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

        // CVE web scaper
        private string getCVECount(string searchString)
        {
            webScraper scrape = new webScraper();
            String webPage = scrape.webScrape("https://nvd.nist.gov/vuln/search/results?form_type=Basic&results_type=overview&query=" + searchString);
            string cveNum = Regex.Match(webPage, @"(?!<strong data-testid=vuln - matching - records - count>)\d+(,\d+)*(?=<\/strong>)").Value;
            return cveNum;
        }

        public async Task nmapScanningProgress(Button nmapInstall, DataGrid nmapOutGrid)
        {

            // Task runs in the background which is the nmap function
            Task<DataTable> nmapTable = Task.Run(() =>
            {
                return fingerPrint(GetLocalIPAddress(), getSubnetFromIP(GetLocalIPAddress()));
            });
            int counter = 0;
            // Runs while nmap command is still running, shows scanning dots
            while (!nmapTable.IsCompleted)
            {
                await Task.Delay(500);
                switch (counter % 3)
                {
                    case 0:
                        nmapInstall.Content = "Scanning.";
                        break;
                    case 1:
                        nmapInstall.Content = "Scanning..";
                        break;
                    case 2:
                        nmapInstall.Content = "Scanning...";
                        break;
                }
                counter++;
            }
            nmapOutGrid.ItemsSource = nmapTable.Result.DefaultView;
            nmapOutGrid.Visibility = Visibility.Visible;
            nmapInstall.Content = "Run Nmap Scan";
        }



    }
}
