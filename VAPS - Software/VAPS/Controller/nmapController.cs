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

namespace VAPS.Controller
{
    class nmapController
    {
        public DataTable fingerPrint(string ip, string subnet){
            //string preload = "Starting Nmap 7.91 ( https://nmap.org ) at 2021-03-22 20:53 GMT Standard Time\r\nNmap scan report for 192.168.0.1\r\nHost is up (0.0046s latency).\r\nNot shown: 995 closed ports\r\nPORT     STATE    SERVICE\r\n80/tcp   open     http\r\n443/tcp  open     https\r\n5000/tcp open     upnp\r\n8081/tcp filtered blackice-icecap\r\n8082/tcp filtered blackice-alerts\r\nMAC Address: 40:0D:10:46:62:20 (Arris Group)\r\nNo exact OS matches for host (If you know what OS is running on it, see https://nmap.org/submit/ ).\r\nTCP/IP fingerprint:\r\nOS:SCAN(V=7.91%E=4%D=3/22%OT=80%CT=1%CU=32312%PV=Y%DS=1%DC=D%G=Y%M=400D10%T\r\nOS:M=6059042F%P=i686-pc-windows-windows)SEQ(SP=C6%GCD=1%ISR=C4%TI=Z%CI=Z%II\r\nOS:=I%TS=7)SEQ(TI=Z%CI=Z%II=I%TS=8)OPS(O1=M5B4ST11NW4%O2=M5B4ST11NW4%O3=M5B\r\nOS:4NNT11NW4%O4=M5B4ST11NW4%O5=M5B4ST11NW4%O6=M5B4ST11)WIN(W1=3890%W2=3890%\r\nOS:W3=3890%W4=3890%W5=3890%W6=3890)ECN(R=Y%DF=Y%T=40%W=3908%O=M5B4NNSNW4%CC\r\nOS:=N%Q=)T1(R=Y%DF=Y%T=40%S=O%A=S+%F=AS%RD=0%Q=)T2(R=N)T3(R=Y%DF=Y%T=40%W=3\r\nOS:890%S=O%A=S+%F=AS%O=M5B4ST11NW4%RD=0%Q=)T4(R=Y%DF=Y%T=40%W=0%S=A%A=Z%F=R\r\nOS:%O=%RD=0%Q=)T5(R=Y%DF=Y%T=40%W=0%S=Z%A=S+%F=AR%O=%RD=0%Q=)T6(R=Y%DF=Y%T=\r\nOS:40%W=0%S=A%A=Z%F=R%O=%RD=0%Q=)T7(R=Y%DF=Y%T=40%W=0%S=Z%A=S+%F=AR%O=%RD=0\r\nOS:%Q=)U1(R=Y%DF=N%T=40%IPL=164%UN=0%RIPL=G%RID=G%RIPCK=G%RUCK=G%RUD=G)IE(R\r\nOS:=Y%DFI=N%T=40%CD=S)\r\nNetwork Distance: 1 hop\r\n\r\nNmap scan report for 192.168.0.16\r\nHost is up (0.0040s latency).\r\nAll 1000 scanned ports on 192.168.0.16 are filtered\r\nMAC Address: 14:91:82:98:D3:02 (Belkin International)\r\nToo many fingerprints match this host to give specific OS details\r\nNetwork Distance: 1 hop\r\n\r\nNmap scan report for 192.168.0.50\r\nHost is up (0.0057s latency).\r\nAll 1000 scanned ports on 192.168.0.50 are closed\r\nMAC Address: AC:F1:DF:12:48:2C (D-Link International)\r\nToo many fingerprints match this host to give specific OS details\r\nNetwork Distance: 1 hop\r\n\r\nNmap scan report for 192.168.0.51\r\nHost is up (0.0015s latency).\r\nAll 1000 scanned ports on 192.168.0.51 are closed\r\nMAC Address: AC:F1:DF:12:48:2D (D-Link International)\r\nToo many fingerprints match this host to give specific OS details\r\nNetwork Distance: 1 hop\r\n\r\nNmap scan report for 192.168.0.49\r\nHost is up (0.000043s latency).\r\nNot shown: 996 closed ports\r\nPORT     STATE SERVICE\r\n135/tcp  open  msrpc\r\n139/tcp  open  netbios-ssn\r\n445/tcp  open  microsoft-ds\r\n3389/tcp open  ms-wbt-server\r\nDevice type: general purpose\r\nRunning: Microsoft Windows 10\r\nOS CPE: cpe:/o:microsoft:windows_10\r\nOS details: Microsoft Windows 10 1809 - 1909\r\nNetwork Distance: 0 hops\r\n\r\nOS detection performed. Please report any incorrect results at https://nmap.org/submit/ .\r\nNmap done: 256 IP addresses (5 hosts up) scanned in 76.21 seconds\r\n";
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
                //if (ip[i] == '1' & subnet[i] == '1'){
                //    stringOut.Append("1");
                //}
                //else
                //{
                //    stringOut.Append("0");
                //}
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

        

    }
}
