using easysave.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace easysave.Models
{
    internal class LoggerHandlerModel
    {

        public LoggerHandler loggerHandler;


        public LoggerHandlerModel(LoggerHandler loggerHandler)
        {
            this.loggerHandler=loggerHandler;
        }

        public bool createLoggerFileIfNotExists()
        {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%",Environment.UserName);
            if (File.Exists(path+"logs.json")) return false;
            System.IO.Directory.CreateDirectory(@path);
            var file = File.Create(@path+"logs.json");
            file.Close();
            return true;
        }

        public void writeLog()
        {
            List<LoggerHandler> loggerHandlers = getAllLogs();
            loggerHandlers.Add(loggerHandler);
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%", Environment.UserName);
            string jsonString = JsonConvert.SerializeObject(loggerHandlers, Newtonsoft.Json.Formatting.Indented);
            using (var streamWriter = new StreamWriter(path+"logs.json"))
            {
                streamWriter.Write(jsonString);
            }
        }

        public List<LoggerHandler> getAllLogs()
        {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%", Environment.UserName);
            List<LoggerHandler> loggerHandlers = new List<LoggerHandler>();
            createLoggerFileIfNotExists();
            using (StreamReader r = new StreamReader(path+"logs.json"))
            {
                string json = r.ReadToEnd();
                if (json != null && json != "")
                {
                    loggerHandlers = JsonConvert.DeserializeObject<List<LoggerHandler>>(json);
                }
                return loggerHandlers;
            }
        }
    }
}
