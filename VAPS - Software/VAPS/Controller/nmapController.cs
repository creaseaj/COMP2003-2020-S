using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VAPS.Controller
{
    class nmapController
    {
        public bool isInstalled()
        {
            string output = cmdController.executeCommand("nmap","");
            string failString = "System.ComponentModel.Win32Exception";
            if (output.Contains(failString))
            {
                return false;
            }
            return true;
        }

    }
}
