using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VAPS.Model;
using VAPS.Resources;
namespace VAPS.Controller
{
    class UsernameSearchController
    {
        public void fileInput()
        {
            //Read in the web addresses to be searched
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
        //Check the username against each address in the file and if it finds a result, add it to the DataTable to return to GUI
        public DataTable generateResults(string username)
        {
            DataTable UsernameTable = new DataTable();
            UsernameTable.Columns.Add("Username exists");
            foreach (var url in WebAddress.Instance.getWebAddresses())
            {
                string fullUrl = ("https://" + url.ToString() + username + "/");
                string result = searchUsername(fullUrl);
                if (result != "Not found.")
                {
                    var newRow = UsernameTable.NewRow();
                    string toSplit = result;
                    string[] split = toSplit.Split('/');
                    newRow[0] = split[2];
                    UsernameTable.Rows.Add(newRow);
                }
            }           
            return UsernameTable;
        }
        //Method for searching the usernames. Takes a url (provided by the file) and uses the HttpWebRequest package to check if the url and username combined returns a results
        public static string searchUsername(String url)
        {
            List<String> matches = new List<string>();
            String returnResult = "";
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                webRequest.Method = "GET";
                webRequest.Timeout = 2000;
                webRequest.UserAgent = ("Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)");
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                if (webResponse.StatusCode == HttpStatusCode.OK)
                {
                    returnResult = url;
                }
                else
                {
                    returnResult = "Not found.";
                }
            }
            catch
            {
                returnResult = "Not found.";
            }
            return returnResult;
        }

        public async Task runUsernameSearch(string username, Button btnUsernameSearch, DataGrid dtGrdUsernames, TextBlock txBlockUsernameResult)
        {
            dtGrdUsernames.Visibility = Visibility.Hidden;
            txBlockUsernameResult.Visibility = Visibility.Hidden;
            // Use threading to allow the user to do other things whilst searching. Search time grows as the websites to search does
            Task<DataTable> UsernameTable = Task.Run(() =>
            {
                Task.Delay(5000).Wait();
                return generateResults(username.ToLower());
            });
            int counter = 0;
            // Show the user that it is searching, but it takes some time
            while (!UsernameTable.IsCompleted)
            {
                await Task.Delay(500);
                switch (counter % 3)
                {
                    case 0:
                        btnUsernameSearch.Content = "Searching.";
                        break;
                    case 1:
                        btnUsernameSearch.Content = "Searching..";
                        break;
                    case 2:
                        btnUsernameSearch.Content = "Searching...";
                        break;
                }
                counter++;
            }
            int numResults = UsernameTable.Result.Rows.Count;
            int numSearched = WebAddress.Instance.numberOfAddresses();
            txBlockUsernameResult.Text = "Number of sites matched: " + numResults + "\nNumber of sites searched: " + numSearched;
            dtGrdUsernames.ItemsSource = UsernameTable.Result.DefaultView;
            dtGrdUsernames.Visibility = Visibility.Visible;
            btnUsernameSearch.Content = "Search";
            txBlockUsernameResult.Visibility = Visibility.Visible;
        }
    }
}
