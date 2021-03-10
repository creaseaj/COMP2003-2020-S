using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VAPS.Model
{
    sealed class Device
    {
        private List<List<String>> registeredDevices;

        Device()
        {
            registeredDevices = new List<List<string>>();
        }
        private static readonly object padlock = new object();
        private static Device instance = null;
        string filepath = ("Resources/RegisteredDevices.csv");


        public static Device Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Device();
                    }
                    return instance;
                }
            }
        }
        public void fileInput()
        {
            if (File.Exists(filepath))
            {
                var fileReader = new StreamReader(File.OpenRead(filepath));
                while (!fileReader.EndOfStream)
                {
                    var rawInput = fileReader.ReadLine();
                    String[] values = rawInput.Split(',');
                    List<String> tempList = new List<string>();
                    tempList.Add(values[0]);
                    tempList.Add(values[1]);
                    registeredDevices.Add(tempList);
                }
                fileReader.Close();
            }
            else
            {
                File.CreateText(filepath);
            }
        }
        public List<List<String>> getList()
        {
            return registeredDevices;
        }
        public void setList(List<List<String>> input)
        {
            this.registeredDevices = input;
        }
        public void fileOutput()
        {
            var fileWriter = new StreamWriter(File.OpenWrite(filepath));
            foreach (var item in this.registeredDevices)
            {
                string output = (item[0].ToString() + "," + item[1].ToString());
                fileWriter.WriteLine(output);
            }
            fileWriter.Close();
        }
    }
}
