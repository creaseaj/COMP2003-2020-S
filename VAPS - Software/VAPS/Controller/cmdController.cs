using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace VAPS.Controller
{
    static class cmdController
    {
        static public string executeCommand(String command, String args)
        {

            String stringOut = "";
            ProcessStartInfo startInfo = new ProcessStartInfo();
            try
            {
                startInfo.CreateNoWindow = true;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.FileName = command;
                startInfo.Arguments = args;
                Process process = Process.Start(startInfo);
                stringOut = process.StandardOutput.ReadToEnd();
                return stringOut;
            }
            catch(Exception e)
            {
                return e.ToString();
            }
            
        }
    }
}
