using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace easysave.Models
{
    internal class LoggerModel
    {
        public void createLoggerFile(string fileName) {
            string s = ConfigurationManager.AppSettings["logPath"]!.ToString();
            System.IO.File.Create(@s+"saveWorks.json");
        }

    }
}
