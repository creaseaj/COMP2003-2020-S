using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VAPS.Model;

namespace VAPS.Controller
{
    class UsernameSearchController
    {
        public void fileInput()
        {
            string filepath = ("Resources/WebAddresses.txt");
            var fileReader = new StreamReader(File.OpenRead(filepath));
            List<String> newAddresses = new List<string>();
            while (!fileReader.EndOfStream)
            {
                newAddresses.Add(fileReader.ReadLine());
            }
            WebAddress.Instance.addWebAddresses(newAddresses);
            fileReader.Close();
        }
    }
}
