using easysave.Objects;
using Newtonsoft.Json;
using System.Configuration;

namespace easysave.Models
{
    internal class LoggerHandlerModel
    {

        public LoggerHandler loggerHandler;


        public LoggerHandlerModel(LoggerHandler loggerHandler)
        {
            this.loggerHandler=loggerHandler;
        }

        public bool createDailyLoggerFileIfNotExists()
        {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%",Environment.UserName);
            if (File.Exists(path+"logs.json")) return false;
            System.IO.Directory.CreateDirectory(@path);
            var file = File.Create(@path+"logs.json");
            file.Close();
            return true;
        }

        public void updateDailyLog()
        {
            List<RegisteredSaveWork> registeredSaveWorks = getAllLogs();
            registeredSaveWorks.Add(loggerHandler.getSaveWork());
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%", Environment.UserName);
            string jsonString = JsonConvert.SerializeObject(registeredSaveWorks, Newtonsoft.Json.Formatting.Indented);
            using (var streamWriter = new StreamWriter(path+"logs.json"))
            {
                streamWriter.Write(jsonString);
            }
        }

        public List<RegisteredSaveWork> getAllLogs()
        {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%", Environment.UserName);
            List<RegisteredSaveWork> registeredSaveWorks = new List<RegisteredSaveWork>();
            createDailyLoggerFileIfNotExists();
            using (StreamReader r = new StreamReader(path+"logs.json"))
            {
                string json = r.ReadToEnd();
                if (json != null && json != "")
                {
                    registeredSaveWorks = JsonConvert.DeserializeObject<List<RegisteredSaveWork>>(json);
                    registeredSaveWorks.RemoveAll(x => x.getSaveName() == this.loggerHandler.getSaveWork().getSaveName());
                }
                return registeredSaveWorks;
            }
        }
    }
}
