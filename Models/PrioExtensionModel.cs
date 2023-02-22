using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easysave.Models
{
    public class PrioExtensionModel
    {

        public List<string> _prioExtensions;

        public PrioExtensionModel()
        {   
            _prioExtensions = new List<string>();
        }

        public PrioExtensionModel prioExtensionModel;

        public bool CheckFileExtension(string prioExtension)
        {
            return ContainsExtension(prioExtension);
        }

        public void AddPrioExtension(string prioExtension)
        {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%", Environment.UserName);
            createConfigFileIfNotExists();
            List<string> prioExtensions = getExtensionList();
            prioExtensions.Add(prioExtension);
            string jsonString = JsonConvert.SerializeObject(prioExtensions, Newtonsoft.Json.Formatting.Indented);
            using (var streamWriter = new StreamWriter(path + "prioExtensions.json"))
            {
                streamWriter.Write(jsonString);
            }
        }

        public void RemovePrioExtension(string prioExtension)
        {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%", Environment.UserName);
            createConfigFileIfNotExists();
            List<string> processes = getExtensionList();
            processes.Remove(prioExtension);
            string jsonString = JsonConvert.SerializeObject(processes, Newtonsoft.Json.Formatting.Indented);
            using (var streamWriter = new StreamWriter(path + "prioExtensions.json"))
            {
                streamWriter.Write(jsonString);
            }
        }

        public List<string> getExtensionList()
        {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%", Environment.UserName);
            List<string>? prioExtensions = new List<string>();
            createConfigFileIfNotExists();
            using (StreamReader r = new StreamReader(path + "prioExtensions.json"))
            {
                string json = r.ReadToEnd();
                if (json != null && json != "")
                {
                    prioExtensions = JsonConvert.DeserializeObject<List<string>>(json);
                }
                return prioExtensions;
            }
        }

        public bool ContainsExtension(string prioExtension)
        {
            return getExtensionList().Contains(prioExtension);
        }

        public List<string> LoadExtensionsFromFile()
        {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%", Environment.UserName);
            if (File.Exists(path + "prioExtensions.json"))
            {
                using (StreamReader r = new StreamReader(path + "prioExtensions.json"))
                {
                    string json = r.ReadToEnd();
                    return JsonConvert.DeserializeObject<List<string>>(json);
                }
            }
            else
            {
                createConfigFileIfNotExists();
                return new List<string> { };
            }
        }

        public void SaveExtensionsToFile(string[] prioExtensions)
        {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%", Environment.UserName);
            string json = JsonConvert.SerializeObject(prioExtensions);
            File.WriteAllText(path + "prioExtensions.json", json);
        }

        public bool createConfigFileIfNotExists()
        {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%", Environment.UserName);
            if (File.Exists(path + "prioExtensions.json")) return false;
            System.IO.Directory.CreateDirectory(@path);
            var file = File.Create(@path + "prioExtensions.json");
            file.Close();
            return true;
        }
    }
}
