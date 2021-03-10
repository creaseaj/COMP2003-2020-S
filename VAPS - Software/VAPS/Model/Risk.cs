using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VAPS.Model
{
    class Risk
    {
        private int numRisks;

        public Risk(int risks)
        {
            numRisks = risks;
        }
        public int getNumRisks()
        {
            return numRisks;
        }
        public void setNumRisks(int inNum)
        {
            numRisks = inNum;
        }
    }
}
