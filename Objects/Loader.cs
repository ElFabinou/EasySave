using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easysave.Objects
{
    public class Loader
    {

        public double percentage;

        public void setPercentage(double totalFile, double currentFile) {
            percentage = (currentFile/totalFile)*100; 
        }
        
        public double getPercentage() { return percentage; }

        public string percentageToChar()
        {
            string loaderString = "";
            for (int i = 0; i < (int)Math.Round(getPercentage(), 0); i++)
            {
                loaderString += "#";
            }
            return loaderString;
        }

    }
}
