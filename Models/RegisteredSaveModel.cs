using easysave.Objects;
using System.Configuration;
using System.Xml;
using System.Text.Json;
using Newtonsoft.Json;
using System.IO;
using System;

namespace easysave.Models
{
    public class RegisteredSaveModel
    {
        public RegisteredSaveWork registeredSaveWork;

        public RegisteredSaveModel(RegisteredSaveWork? registeredSaveWork = null)
        {
            this.registeredSaveWork = registeredSaveWork;
        }

        public void createConfigFileIfNotExists() {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString();
            if (File.Exists(path+"saveWorks.json")) return;
            var file = File.Create(@path+"saveWorks.json");
            file.Close();
        }

        public void writeInJson()
        {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString();
            createConfigFileIfNotExists();
            List<RegisteredSaveWork> registeredSaveWorksList = getAllRegisteredSaveWork();
            registeredSaveWorksList.Add(registeredSaveWork);
            string jsonString = JsonConvert.SerializeObject(registeredSaveWorksList, Newtonsoft.Json.Formatting.Indented);
            using (var streamWriter = new StreamWriter(path+"saveWorks.json"))
            {
                streamWriter.Write(jsonString);
            }

        }

        public List<RegisteredSaveWork> getAllRegisteredSaveWork() {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString();
            List<RegisteredSaveWork> registeredSaveWorksList = new List<RegisteredSaveWork>();
            using (StreamReader r = new StreamReader(path+"saveWorks.json"))
            {
                string json = r.ReadToEnd();
                if(json != null && json != "") {
                    registeredSaveWorksList = JsonConvert.DeserializeObject<List<RegisteredSaveWork>>(json);
                }
                return registeredSaveWorksList;
            }
        }

        public RegisteredSaveWork? getRegisteredWork(string name)
        {
            List<RegisteredSaveWork> registeredSaveWorksList = getAllRegisteredSaveWork();
            foreach(RegisteredSaveWork registeredSaveWork in registeredSaveWorksList )
            {
                if(registeredSaveWork.getSaveName() == name)
                {
                    return registeredSaveWork;
                }
            }
            return null;
        }
    }
}