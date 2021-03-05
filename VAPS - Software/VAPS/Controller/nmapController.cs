using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace VAPS.Controller
{
    class nmapController
    {
        public string fingerPrint(string subnet){
            string output = "testFail";
            output = cmdController.executeCommand("nmap", "192.168.0.0/24");
            return output;
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
