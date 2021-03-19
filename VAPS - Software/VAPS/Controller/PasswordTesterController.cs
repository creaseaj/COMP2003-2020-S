using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hexasoft.Zxcvbn;

namespace VAPS.Controller
{
    
    class PasswordTesterController
    {
        ZxcvbnEstimator passwordTester = new ZxcvbnEstimator();

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
    }
}
