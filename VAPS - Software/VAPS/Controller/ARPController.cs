﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Data;
using VAPS.Model;

namespace VAPS.Controller
{
    class ARPController
    {
        List<List<String>> arpList;
        ARPController ARP;
        Device device;
        List<String> vendors;
        public ARPController()
        {
            arpList = new List<List<String>>();
            vendors = new List<String>();
            ARP = this;
            device = Device.Instance;
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
                    String[] newArp = new string[4];
                    Array.Copy(toArpList,1,newArp,0,3);
                    newArp[3] = ("Unknown device.");
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
                string test = vendor.Substring(3);
                vendors.Add(test);
                return test;
                //return(vendor.Substring(3));
            }
            catch( System.ArgumentOutOfRangeException)
            {
                return macAddress;
            }
        }
        private List<List<string>> getVendorsList(List<List<string>> arpList)
        {
            List<List<String>> arpOut = new List<List<string>>();
            string[] headers = { "IP Address", "MAC Address", "Status", "Friendly Name"};
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
        public DataTable initialTable(DataTable ARPTable)
        {
            List<List<string>> arpList = ARP.getARPRaw();
            for (int i = 0; i < arpList[0].Count; i++)
            {
                ARPTable.Columns.Add(new DataColumn(arpList[0][i]));
            }

            for (int i = 1; i < arpList.Count; i++)
            {
                var newRow = ARPTable.NewRow();
                for (int j = 0; j < arpList[i].Count; j++)
                {
                    newRow[arpList[0][j]] = arpList[i][j];
                }
                newRow = checkDevice(newRow);
                ARPTable.Rows.Add(newRow);
            }
            for (int i = ARPTable.Rows.Count - 1; i >= 0; i--)
            {
                DataRow row = ARPTable.Rows[i];
                if (row[2].ToString() == "static")
                {
                    row.Delete();
                }
            }
            ARPTable.AcceptChanges();

            return ARPTable;
        }
        public DataTable formatTable(DataTable ARPTable)
        {
            ARPTable = initialTable(ARPTable);
            foreach (DataRow entry in ARPTable.Rows)
            {
                if (entry[0].ToString().Contains("192.168.0"))
                {
                    String[] split = entry[0].ToString().Split('.');
                    entry[0] = "localhost." + split[3];
                }
                /*   Remove broadcast address from table as we don't need it
                else if (entry[0].ToString() == "255.255.255.255")
                {
                    ARPTable.Rows.Remove(entry);
                }
                */
                else if (entry[0].ToString() == "255.255.255.255")
                {
                    entry[0] = "Broadcast";
                }
                else if (entry[0].ToString() == "239.255.255.250")
                {
                    entry[0] = "Local Multicast";
                }
                else if (entry[0].ToString().Contains("224.0.0"))
                {
                    entry[0] = "System";
                }
                device.fileOutput();
                //entry[3] = addRegisteredNames(entry[1].ToString());
            }



            return ARPTable;
        }
        public DataRow checkDevice(DataRow nextRow)
        {
            List<List<String>> tempList = new List<List<string>>();
            tempList = Device.Instance.getList();
            foreach (var device in tempList)
            {
                if (nextRow[1].ToString() == device[0])
                {
                    nextRow[3] = device[1];
                }
            }
            return nextRow;
        }
        public void registerDevice(string MACAddress, string deviceName)
        {
            List<List<String>> temp = new List<List<String>>();
            List<String> input = new List<String>();
            input.Add(MACAddress);
            input.Add(deviceName);
            temp = device.getList();
            temp.Add(input);
            device.setList(temp);
        }
        public int[] getResults(DataTable table)
        {
            int registered = 0;
            int known = 0;
            int unknown = 0;

            List<String> noDuplicates = vendors.Distinct().ToList();

            foreach (DataRow row in table.Rows)
            {
                foreach (string vendor in noDuplicates)
                {
                    if (row[1].ToString().Contains(vendor))
                    {
                        known++;
                    }
                }
                if (row[3].ToString() == "Unknown device.")
                {
                    unknown++;
                }
                else
                {
                    registered++;
                }
            }
            int[] results = new int[] { registered, known, unknown };
            return results;
        }
        
    }
}
