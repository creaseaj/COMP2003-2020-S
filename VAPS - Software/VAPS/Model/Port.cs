using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VAPS.Model
{
    sealed class Port
    {
        private List<List<String>> portDefinitions;
        Port()
        {
            portDefinitions = new List<List<string>>();
        }   
        private static readonly object padlock = new object();
        private static Port instance = null;
        
        
        public static Port Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Port();
                    }
                    return instance;
                }
            }
        }
        public void fileInput()
        {
            string filepath = ("Resources/Ports List.csv");
            var fileReader = new StreamReader(File.OpenRead(filepath));
            fileReader.ReadLine();
            while (!fileReader.EndOfStream)
            {
                var rawInput = fileReader.ReadLine();
                string[] values = rawInput.Split(',');
                List<String> tempList = new List<string>();
                tempList.Add(values[0]);
                tempList.Add(values[1]);
                tempList.Add(values[2]);
                portDefinitions.Add(tempList);
            }
            fileReader.Close();
        }
        public List<List<String>> getList()
        {
            return portDefinitions;
        }
    }
}
