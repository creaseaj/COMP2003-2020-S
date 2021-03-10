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
            string filepath = ("Resources/RegisteredDevices.csv");
            var nextLine = "";
            //var fileReader = new StreamReader(File.AppendText(filepath));
        }
        public void checkFileExists()
        {

        }
        public List<List<String>> getList()
        {
            return registeredDevices;
        }
    }
}
