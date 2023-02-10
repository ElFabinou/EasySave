using easysave.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;

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
            if (File.Exists(path+"dailyLogs-"+DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)+".json") && File.Exists(path+"stateLogs.json")) return false;
            if (!File.Exists(path+"dailyLogs-"+DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)+".json")){
                System.IO.Directory.CreateDirectory(@path);
                var file = File.Create(@path+"dailyLogs-"+DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)+".json");
                file.Close();
            }
            if (!File.Exists(path+"stateLogs.json"))
            {
                System.IO.Directory.CreateDirectory(@path);
                var file = File.Create(@path+"stateLogs.json");
                file.Close();
            }
            return true;
        }

        public void updateStateLog()
        {
            List<StateLog> stateLogs = getAllStateLog();
            stateLogs.Add(loggerHandler.getStateLog());
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%", Environment.UserName);
            string jsonString = JsonConvert.SerializeObject(stateLogs, Newtonsoft.Json.Formatting.Indented);
            using (var streamWriter = new StreamWriter(path+"stateLogs.json"))
            {
                streamWriter.Write(jsonString);
            }
        }

        public void updateDailyLog()
        {
            List<DailyLog> dailyLogs = getAllDailyLog();
            dailyLogs.Add(loggerHandler.getDailyLog());
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%", Environment.UserName);
            string jsonString = JsonConvert.SerializeObject(dailyLogs, Newtonsoft.Json.Formatting.Indented);
            using (var streamWriter = new StreamWriter(path+"dailyLogs-"+DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)+".json"))
            {
                streamWriter.Write(jsonString);
            }
        }

        public List<StateLog> getAllStateLog()
        {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%", Environment.UserName);
            List<StateLog>? stateLogs = new List<StateLog>();
            createLoggerFileIfNotExists();
            using (StreamReader r = new StreamReader(path+"stateLogs.json"))
            {
                string json = r.ReadToEnd();
                if (json != null && json != "")
                {
                    stateLogs = JsonConvert.DeserializeObject<List<StateLog>>(json);
                    stateLogs!.RemoveAll(x => x.getSaveName() == this.loggerHandler.getStateLog().getSaveName());
                }
                return stateLogs;
            }
        }

        public List<DailyLog> getAllDailyLog()
        {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%", Environment.UserName);
            List<DailyLog>? stateLogs = new List<DailyLog>();
            createLoggerFileIfNotExists();
            using (StreamReader r = new StreamReader(path+"dailyLogs-"+DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)+".json"))
            {
                string json = r.ReadToEnd();
                if (json != null && json != "")
                {
                    stateLogs = JsonConvert.DeserializeObject<List<DailyLog>>(json);
                }
                return stateLogs;
            }
        }
    }
}
