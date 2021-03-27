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
using System.Windows;
using System.Windows.Media;

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

            try
            {
                var timeFormatted = TimeSpan.FromDays(offlineFastHash);
                time = timeFormatted.Hours.ToString() + " Hours " + ", Minutes: " + timeFormatted.Minutes.ToString();
            }
            catch (Exception)
            {
                time = "A long time!";
            }

            return time;
        }


        public Image[] passwordGuidance(string password, Image[] icons)
        {

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
        public void passwordDisplay(PasswordBox pwdPasswordInput, TextBlock txtTimeToCrack, Image[] passwordImages, TextBlock txtBlockCleartext)
        {
            if (pwdPasswordInput.Password.Length != 0)
            {
                txtTimeToCrack.Text = "Time to crack: " + timeToCrack(pwdPasswordInput.Password);
                passwordImages = passwordGuidance(pwdPasswordInput.Password, passwordImages);
                txtBlockCleartext.Text = pwdPasswordInput.Password;
            }
            else
            {
                txtTimeToCrack.Text = "";
            }
        }
        public void displayCleartext(TextBlock txtBlockCleartext, PasswordBox pwdPasswordInput)
        {
            if (txtBlockCleartext.Visibility == Visibility.Hidden)
            {
                txtBlockCleartext.Visibility = Visibility.Visible;
                pwdPasswordInput.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFABADB3"));
            }
            else if (txtBlockCleartext.Visibility == Visibility.Visible)
            {
                txtBlockCleartext.Visibility = Visibility.Hidden;
                pwdPasswordInput.Foreground = new SolidColorBrush(Colors.Black);
            }

        }
    }
}
