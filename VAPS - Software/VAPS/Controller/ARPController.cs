using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace VAPS.Controller
{
    class ARPController
    {
        List<List<String>> arpList;
        public ARPController()
        {
            arpList = new List<List<String>>();
        }

   
        private void updateArpList()
        {
            arpList.Clear();
            String unParsedList = cmdController.executeCommand("arp", "-a");
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
        private string getMACVendor(String macAddress){
            webScraper scrape = new webScraper();
            String webPage = scrape.webScrape("https://www.adminsub.net/mac-address-finder/" + macAddress);
            String[] webPageLines = Regex.Split(webPage,@"\n");
            String vendor = Regex.Match(webPageLines[94],@"\?q=(\w|\s)+").Value;
            try
            {
                return(vendor.Substring(3));
            }
            catch( System.ArgumentOutOfRangeException)
            {
                return macAddress;
            }
            
            
        }
        private List<List<string>> getVendorsList(List<List<string>> arpList)
        {
            List<List<String>> arpOut = new List<List<string>>();
            string[] headers = { "IP Address", "MAC Address", "Status" };
            arpOut.Add(headers.ToList());
            foreach (List<String> item in arpList)
            {
                List<String> newList = new List<string>();
                for (int i = 0; i < item.Count; i++)
                {
                    if (i == 1 & item[2] != "static")
                    {
                        newList.Add(getMACVendor(item[i].Substring(0, 8)) + item[i].Substring(8));
                    }
                    else { newList.Add(item[i]); }
                }
                arpOut.Add(newList);
            }
            return arpOut;
        }
        public string getArpList()
        {
            updateArpList();
            // creating new arp list with custom mac addresses
            List<List<string>> arpOut = getVendorsList(arpList);
            int[] lengths = new int[arpOut[0].Count];
            // finding longest item in each column
            for(int i = 0; i < arpOut.Count; i++)
            {
                for(int j = 0; j < arpOut[i].Count; j++)
                {
                    int size = arpOut[i][j].Length;
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
        public List<List<string>> getARPRaw()
        {
            updateArpList();
            return getVendorsList(arpList);
        }

        
    }
}
