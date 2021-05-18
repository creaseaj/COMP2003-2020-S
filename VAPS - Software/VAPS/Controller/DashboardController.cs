using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace VAPS.Controller
{
    class DashboardController
    {
        //Method for updating the dashboard total low, medium, and high risks
        public Label[] updateDashboard(Label[] dashboardLabels, TextBlock[] arpResults, TextBlock[] portScanResults)
        {
            //Declarations
            int high, medium, low;
            int[] arpScores = generateARPScores(arpResults);
            int[] portScores = generatePortScores(portScanResults);

            //Generate a total for the dashboard
            high = portScores[2] + arpScores[2];
            medium = portScores[1] + arpScores[1];
            low = portScores[0] + arpScores[0];

            //Set the dashboard labels with the new totals
            dashboardLabels[0].Content = high.ToString();
            dashboardLabels[1].Content = medium.ToString();
            dashboardLabels[2].Content = low.ToString();

            //Return the Label array with the new results, which will update the dashboard totals
            return dashboardLabels;
        }

        //Method for updating the dashboard risk information. It gives different outputs depending if the ARP and Ports have values or not.
        public TextBlock[] updateDashboardInformation(TextBlock[] dashboardInformation, TextBlock[] arpResults, TextBlock[] portScanResults)
        {
            //Declarations
            int[] arpScores = generateARPScores(arpResults);
            int[] portScores = generatePortScores(portScanResults);

            //Set text for low-level risk information
            if ((arpScores[0] == 0) && (portScores[0] == 0))
            {
                dashboardInformation[2].Text = "There are no low-level risks found through either ARP or Port Scanning tools.";
            }
            else if ((arpScores[0] != 0) && (portScores[0] == 0))
            {
                dashboardInformation[2].Text = ("The ARP scan has found " + arpScores[0] + " low-level risks. These are known devices, registered with VAPTS and are therefore likely to be safe.\n\nPort scanning has 0 low-level risks. Either it has not ran or the device does not have any low-level risk ports open.");
            }
            else if ((arpScores[0] == 0) && (portScores[0] != 0))
            {
                dashboardInformation[2].Text = ("The ARP scan has 0 low-level risks. Either it has not ran, or there aren't any known devices found.\n\nPort scanning has found " + portScores[0] + " low-level ports open. These should be safe to keep open.");
            }
            else if ((arpScores[0] != 0) && (portScores[0] != 0))
            {
                dashboardInformation[2].Text = ("The ARP scan has found " + arpScores[0] + " low-level risks. These are known devices, registered with VAPTS and are therefore likely to be safe.\n\nPort scanning has found " + portScores[0] + " low-level ports open. These should be safe to keep open.");
            }

            //Set text for medium level risk information
            if ((arpScores[1] == 0) && (portScores[1] == 0))
            {
                dashboardInformation[1].Text = "There are no medium level risks found through either ARP or Port Scanning tools.";
            }
            else if ((arpScores[1] != 0) && (portScores[1] == 0))
            {
                dashboardInformation[2].Text = ("The ARP scan has found " + arpScores[1] + " medium level risks. These are unknown devices, not registered with VAPTS but the manufacturer can be identified. Check out these devices and register them to ensure they are not malicious.\n\nPort scanning has 0 medium level risks. Either it has not ran or the device does not have any medium risk ports open.");
            }
            else if ((arpScores[1] == 0) && (portScores[1] != 0))
            {
                dashboardInformation[1].Text = ("The ARP scan has 0 medium level risks. Either it has not ran, or there aren't any unknown devices found.\n\nPort scanning has found " + portScores[1] + " medium level risk ports open. Identify what service is using these and, if not necessary, consider closing.");
            }
            else if ((arpScores[1] != 0) && (portScores[1] != 0))
            {
                dashboardInformation[1].Text = ("The ARP scan has found " + arpScores[1] + " medium level risks. These are unknown devices, not registered with VAPTS but the manufacturer can be identified. Check out these devices and register them to ensure they are not malicious.\n\nPort scanning has found " + portScores[1] + " medium level risk ports open. Identify what service is using these and, if not necessary, consider closing.");
            }

            //Set text for high level risk information
            if ((arpScores[2] == 0) && (portScores[2] == 0))
            {
                dashboardInformation[0].Text = "There are no high level risks found through either ARP or Port Scanning tools.";
            }
            else if ((arpScores[2] != 0) && (portScores[2] == 0))
            {
                dashboardInformation[0].Text = ("The ARP scan has found " + arpScores[2] + " high level risks. These are unknown devices, not is the manufacturer known. These devices can potentially be malicious so check the device out and consider restricting network access.\n\nPort scanning has 0 high level risks. Either it has not ran or the device does not have any high risk ports open.");
            }
            else if ((arpScores[2] == 0) && (portScores[2] != 0))
            {
                dashboardInformation[0].Text = ("The ARP scan has 0 high level risks. Either it has not ran, or there aren't any high risk devices found.\n\nPort scanning has found " + portScores[2] + " high level risk ports open. These ports have either known vulnerabilities or are open ports which are not needed. Consider closing them ASAP.");
            }
            else if ((arpScores[2] != 0) && (portScores[2] != 0))
            {
                dashboardInformation[0].Text = ("The ARP scan has found " + arpScores[2] + " high level risks. These are unknown devices, not is the manufacturer known. These devices can potentially be malicious so check the device out and consider restricting network access.\n\nPort scanning has found " + portScores[2] + " high level risk ports open. These ports have either known vulnerabilities or are open ports which are not needed. Consider closing them ASAP.");
            }

            //Return the updated dashboard information array, which will update the dashboard
            return dashboardInformation;
        }

        //Method for generating the total ARP risks
        private int[] generateARPScores(TextBlock[] arpResults)
        {
            //Check that there is a score, if it's empty then set it to 0
            foreach (TextBlock result in arpResults)
            {
                if (result.Text.ToString() == "")
                {
                    result.Text = "0";
                }
            }
            //Create an int array of the ARP low, medium, high level risks
            int[] arpScores = new int[] { 0, 0, 0 };
            arpScores[0] = Convert.ToInt32(arpResults[0].Text.ToString());
            arpScores[1] = Convert.ToInt32(arpResults[1].Text.ToString());
            arpScores[2] = Convert.ToInt32(arpResults[2].Text.ToString());

            return arpScores;
        }
        private int[] generatePortScores(TextBlock[] portScanResults)
        {
            //Check that there is a score, if it's empty then set it to 0
            foreach (TextBlock result in portScanResults)
            {
                if (result.Text.ToString() == "")
                {
                    result.Text = "0";
                }
            }

            //Create an int array of the ports low, medium, high level risks
            int[] portScores = new int[] { 0, 0, 0 };
            portScores[0] = Convert.ToInt32(portScanResults[0].Text.ToString());
            portScores[1] = Convert.ToInt32(portScanResults[1].Text.ToString());
            portScores[2] = Convert.ToInt32(portScanResults[2].Text.ToString());

            return portScores;
        }
    }
}
