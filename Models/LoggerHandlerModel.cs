using easysave.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Resources;
using System.Threading;

namespace easysave.Models
{
    internal class LoggerHandlerModel
    {

        public LoggerHandler loggerHandler;
        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        public LoggerHandlerModel(LoggerHandler loggerHandler)
        {
            this.loggerHandler=loggerHandler;
        }

        public ResourceManager language;

        public bool createLoggerFileIfNotExists()
        {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%",Environment.UserName);
            if (File.Exists(path+"dailyLogs-"+DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)+"."+loggerHandler.getFormat().ToString()) && File.Exists(path+"stateLogs"+"."+loggerHandler.getFormat().ToString())) return false;
            if (!File.Exists(path+"dailyLogs-"+DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)+"."+loggerHandler.getFormat().ToString())){
                System.IO.Directory.CreateDirectory(@path);
                var file = File.Create(@path+"dailyLogs-"+DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)+"."+loggerHandler.getFormat().ToString());
                file.Close();
                if (loggerHandler.getFormat().ToString() == "xml")
                {
                    using (XmlWriter writer = XmlWriter.Create(@path+"dailyLogs-"+DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)+"."+loggerHandler.getFormat().ToString()))
                    {
                        writer.WriteStartDocument();
                        writer.WriteStartElement("root");
                        writer.WriteEndElement();
                        writer.WriteEndDocument();
                    }
                }
            }
            if (!File.Exists(path+"stateLogs"+"."+loggerHandler.getFormat().ToString()))
            {
                System.IO.Directory.CreateDirectory(@path);
                var file = File.Create(path+"stateLogs"+"."+loggerHandler.getFormat().ToString());
                file.Close();
                if (loggerHandler.getFormat().ToString() == "xml")
                {
                    using (XmlWriter writer = XmlWriter.Create(path+"stateLogs"+"."+loggerHandler.getFormat().ToString()))
                    {
                        writer.WriteStartDocument();
                        writer.WriteStartElement("root");
                        writer.WriteEndElement();
                        writer.WriteEndDocument();
                    }
                }
            }
            return true;
        }

        public void updateStateLog()
        {
            _semaphore.Wait();
            List<StateLog> stateLogs = getAllStateLog();
            stateLogs.Add(loggerHandler.getStateLog());
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%", Environment.UserName);
            switch (loggerHandler.getFormat().ToString().ToLower())
            {
                case "json":
                    string jsonString = JsonConvert.SerializeObject(stateLogs, Newtonsoft.Json.Formatting.Indented);
                    using (var streamWriter = new StreamWriter(path + "stateLogs" + "." + loggerHandler.getFormat().ToString()))
                    {
                        streamWriter.Write(jsonString);
                    }
                    break;
                case "xml":
                    XmlSerializer serializer = new XmlSerializer(typeof(List<StateLog>), new XmlRootAttribute("root"));
                    using (StringWriter writer = new StringWriter())
                    {
                        serializer.Serialize(writer, stateLogs);
                        using (var streamWriter = new StreamWriter(path + "stateLogs" + "." + loggerHandler.getFormat().ToString()))
                        {
                            streamWriter.Write(writer.ToString());
                        }
                    }
                    break;
            }
            _semaphore.Release();
        }

        public void updateDailyLog()
        {
            _semaphore.Wait();
            List<DailyLog> dailyLogs = getAllDailyLog();
            dailyLogs.Add(loggerHandler.getDailyLog());
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%", Environment.UserName);
            if (loggerHandler.getFormat().ToString().ToLower() == "xml")
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<DailyLog>), new XmlRootAttribute("root"));
                using (var streamWriter = new StreamWriter(path + "dailyLogs-" + DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) + "." + loggerHandler.getFormat().ToString()))
                {
                    serializer.Serialize(streamWriter, dailyLogs);
                }
            }
            else
            {
                string jsonString = JsonConvert.SerializeObject(dailyLogs, Newtonsoft.Json.Formatting.Indented);
                using (var streamWriter = new StreamWriter(path + "dailyLogs-" + DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) + "." + loggerHandler.getFormat().ToString()))
                {
                    streamWriter.Write(jsonString);
                }
            }
            _semaphore.Release();
        }

        public List<StateLog> getAllStateLog()
        {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%", Environment.UserName);
            List<StateLog>? stateLogs = new List<StateLog>();
            createLoggerFileIfNotExists();
            using (StreamReader r = new StreamReader(path + "stateLogs" + "." + loggerHandler.getFormat().ToString()))
            {
                string json = r.ReadToEnd();
                if (json != null && json != "")
                {
                    switch (loggerHandler.getFormat().ToString().ToLower())
                    {
                        case "json":
                            stateLogs = JsonConvert.DeserializeObject<List<StateLog>>(json);
                            break;
                        case "xml":
                            XmlSerializer serializer = new XmlSerializer(typeof(List<StateLog>), new XmlRootAttribute("root"));
                            using (StringReader reader = new StringReader(json))
                            {
                                stateLogs = (List<StateLog>)serializer.Deserialize(reader);
                            }
                            break;
                    }
                    stateLogs!.RemoveAll(x => x.getSaveName() == this.loggerHandler.getStateLog().getSaveName());
                }
                return stateLogs;
            }
        }

        public List<DailyLog> getAllDailyLog()
        {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%", Environment.UserName);
            List<DailyLog>? dailyLogs = new List<DailyLog>();
            createLoggerFileIfNotExists();
            switch (loggerHandler.getFormat().ToString().ToLower())
            {
                case "json":
                    using (StreamReader r = new StreamReader(path + "dailyLogs-" + DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) + "." + loggerHandler.getFormat().ToString()))
                    {
                        string json = r.ReadToEnd();
                        if (json != null && json != "")
                        {
                            dailyLogs = JsonConvert.DeserializeObject<List<DailyLog>>(json);
                        }
                        return dailyLogs;
                    }
                case "xml":
                    XmlSerializer serializer = new XmlSerializer(typeof(List<DailyLog>), new XmlRootAttribute("root"));
                    using (StreamReader r = new StreamReader(path + "dailyLogs-" + DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) + "." + loggerHandler.getFormat().ToString()))
                    {
                        dailyLogs = (List<DailyLog>)serializer.Deserialize(r);
                        return dailyLogs;
                    }
                default:
                    return dailyLogs;
            }
        }

        public ReturnHandler setLoggerExtension(string loggerExtension)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            settings["configExtension"].Value = loggerExtension;
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            LanguageHandler languageHandler = LanguageHandler.Instance;
            return new ReturnHandler(languageHandler.rm.GetString("config_extension_update"), ReturnHandler.ReturnTypeEnum.Success);
        }
    }
}
