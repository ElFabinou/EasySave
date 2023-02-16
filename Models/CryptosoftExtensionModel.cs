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
    public class CryptosoftExtensionModel
    {

        public List<string> _extensions;

        public CryptosoftExtensionModel()
        {   
            _extensions = new List<string>();
        }

        public BlacklistModel _blacklist;


        public void AddCryptosoftExtension(string extension)
        {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%", Environment.UserName);
            createConfigFileIfNotExists();
            List<string> extensions = getExtensionList();
            extensions.Add(extension);
            string jsonString = JsonConvert.SerializeObject(extensions, Newtonsoft.Json.Formatting.Indented);
            using (var streamWriter = new StreamWriter(path + "extensions.json"))
            {
                streamWriter.Write(jsonString);
            }
        }

        public void RemoveCryptosoftExtension(string extension)
        {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%", Environment.UserName);
            createConfigFileIfNotExists();
            List<string> processes = getExtensionList();
            processes.Remove(extension);
            string jsonString = JsonConvert.SerializeObject(processes, Newtonsoft.Json.Formatting.Indented);
            using (var streamWriter = new StreamWriter(path + "extensions.json"))
            {
                streamWriter.Write(jsonString);
            }
        }

        public List<string> getExtensionList()
        {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%", Environment.UserName);
            List<string>? extensions = new List<string>();
            createConfigFileIfNotExists();
            using (StreamReader r = new StreamReader(path + "extensions.json"))
            {
                string json = r.ReadToEnd();
                if (json != null && json != "")
                {
                    extensions = JsonConvert.DeserializeObject<List<string>>(json);
                }
                return extensions;
            }
        }

        public bool ContainsProcess(string extension)
        {
            return getExtensionList().Contains(extension);
        }

        public List<string> LoadExtensionsFromFile()
        {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%", Environment.UserName);
            if (File.Exists(path + "extensions.json"))
            {
                using (StreamReader r = new StreamReader(path + "extensions.json"))
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

        public void SaveExtensionsToFile(string[] extensions)
        {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%", Environment.UserName);
            string json = JsonConvert.SerializeObject(extensions);
            File.WriteAllText(path + "extensions.json", json);
        }

        public bool createConfigFileIfNotExists()
        {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%", Environment.UserName);
            if (File.Exists(path + "extensions.json")) return false;
            System.IO.Directory.CreateDirectory(@path);
            var file = File.Create(@path + "extensions.json");
            file.Close();
            return true;
        }
    }
}
