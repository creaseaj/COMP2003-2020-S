using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hexasoft.Zxcvbn;
using System.Reflection;
using System.IO;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;

namespace VAPS.Controller
{
    class PasswordTesterController
    {
        ZxcvbnEstimator passwordTester = new ZxcvbnEstimator();
        string checkPath = "VAPS.Resources.Icons.check.png";
        string crossPath = "VAPS.Resources.Icons.cancel.png";
        Regex lower = new Regex(@"[a-z]");
        Regex number = new Regex(@"[0-9]");
        Regex special = new Regex(@"[!@#$%^&*(),.?:{}|<>]");
        Regex upper = new Regex(@"[A-Z]");

        public string timeToCrack(string password)
        {
            var result = passwordTester.EstimateStrength(password);
            double offlineFastHash = result.CrackTimesSeconds.OfflineFastHashing1e10PerSecond;
            //double offlineSlowHash = result.CrackTimesSeconds.OfflineSlowHashing1e4PerSecond;
            //double noThrottling = result.CrackTimesSeconds.OnlineNoThrottling10PerSecond;
            //double throttling = result.CrackTimesSeconds.OnlineThrottling100PerHour;

            //var times = new double[] { offlineFastHash, offlineSlowHash, noThrottling, throttling };

            //double average = Queryable.Average(times.AsQueryable());

            string time = "";

            if (offlineFastHash < 0.1)
            {
                time = "under a second.";
            }
            else
            {
                time = (Math.Round(offlineFastHash, 2)).ToString();
            }     

            return time;
        }


        public Image[] passwordGuidance(string password, Image[] icons)
        {
            /*
            if (Regex.IsMatch(password, "/[^a - z] / g;"))
            {
                icons[1].Source = new BitmapImage(new Uri(@"/VAPTS;Resources/Icons/check.png"));
            }
            else
            {
                icons[1].Source = new BitmapImage(new Uri(@"/VAPS;component/Resources/Icons/cancel.png", UriKind.RelativeOrAbsolute));
            }
            */
            //Check length is greater than 9 characters
            _ = password.Length > 9 ? icons[0].Source = new BitmapImage(new Uri(@"/VAPS;component/Resources/Icons/check.png", UriKind.RelativeOrAbsolute)) : icons[0].Source = new BitmapImage(new Uri(@"/VAPS;component/Resources/Icons/cancel.png", UriKind.RelativeOrAbsolute));

            //Check for lower case
            _ = lower.IsMatch(password) ? icons[1].Source = new BitmapImage(new Uri(@"/VAPS;component/Resources/Icons/check.png", UriKind.RelativeOrAbsolute)) : icons[1].Source = new BitmapImage(new Uri(@"/VAPS;component/Resources/Icons/cancel.png", UriKind.RelativeOrAbsolute));

            //Check it contains a number
            _ = number.IsMatch(password) ? icons[2].Source = new BitmapImage(new Uri(@"/VAPS;component/Resources/Icons/check.png", UriKind.RelativeOrAbsolute)) : icons[2].Source = new BitmapImage(new Uri(@"/VAPS;component/Resources/Icons/cancel.png", UriKind.RelativeOrAbsolute));

            //Check it does not contain the word "password"
            _ = password.ToLower().Contains("password") ? icons[3].Source = new BitmapImage(new Uri(@"/VAPS;component/Resources/Icons/cancel.png", UriKind.RelativeOrAbsolute)) : icons[3].Source = new BitmapImage(new Uri(@"/VAPS;component/Resources/Icons/check.png", UriKind.RelativeOrAbsolute));

            //Check it has a special character
            _ = special.IsMatch(password) ? icons[4].Source = new BitmapImage(new Uri(@"/VAPS;component/Resources/Icons/check.png", UriKind.RelativeOrAbsolute)) : icons[4].Source = new BitmapImage(new Uri(@"/VAPS;component/Resources/Icons/cancel.png", UriKind.RelativeOrAbsolute));
            //Check it has an uppercase character

            _ = upper.IsMatch(password) ? icons[5].Source = new BitmapImage(new Uri(@"/VAPS;component/Resources/Icons/check.png", UriKind.RelativeOrAbsolute)) : icons[5].Source = new BitmapImage(new Uri(@"/VAPS;component/Resources/Icons/cancel.png", UriKind.RelativeOrAbsolute));

            return icons;
        }
    }
}
