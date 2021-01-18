using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace VAPS.Controller
{
    class webScraper
    {
        String url;
        public webScraper()
        {
            //url = urlIn;
            //webScrape(url);
        }
        public string webScrape(String url)
        {
            var web = new HtmlWeb();
            var doc = web.Load(url);
            return doc.Text;
        }
    }
}
