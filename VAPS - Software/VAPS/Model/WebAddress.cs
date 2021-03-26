using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VAPS.Model
{
    sealed class WebAddress
    {
        private List<String> webAddresses;

        WebAddress()
        {
            webAddresses = new List<string>();
        }
        private static readonly object padlock = new object();
        private static WebAddress instance = null;

        public static WebAddress Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new WebAddress();
                    }
                    return instance;
                }
            }
        }
        public List<String> getWebAddresses()
        {
            return instance.webAddresses;
        }
        public void addWebAddresses(List<String> newAddresses)
        {
            instance.webAddresses = newAddresses;
        }
    }
}
