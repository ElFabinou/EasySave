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
    public class BlacklistModel
    {

        public List<string> _processNames;

        public BlacklistModel()
        {   
            _processNames = new List<string>();
            //_processNames = LoadBlacklistFromFile();
            //_blacklist = new BlacklistModel();
            //Récuperer le nom des logiciel dans le fichier json
        }

        public BlacklistModel _blacklist;


        public bool StartProcessMonitor()
        {
            //_processNames.Add("Calculator");
            var runningProcesses = Process.GetProcesses();
            foreach (var process in runningProcesses)
            {
                Console.WriteLine(process.ProcessName);
                Console.WriteLine(ContainsProcess(process.ProcessName));
                if (ContainsProcess(process.ProcessName))
                {
                    //Le processus est dans la liste noire, vous pouvez effectuer l'action appropriée ici...
                    Console.WriteLine("Blacklist detecté");
                    return false;
                }
            }
            return true;
        }


        public void AddProcessName(string processName)
        {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%", Environment.UserName);
            createConfigFileIfNotExists();
            List<string> processes = getAllProcesses();
            processes.Add(processName);
            string jsonString = JsonConvert.SerializeObject(processes, Newtonsoft.Json.Formatting.Indented);
            using (var streamWriter = new StreamWriter(path + "blacklist.json"))
            {
                streamWriter.Write(jsonString);
            }
        }

        public void RemoveProcessName(string processName)
        {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%", Environment.UserName);
            createConfigFileIfNotExists();
            List<string> processes = getAllProcesses();
            processes.Remove(processName);
            string jsonString = JsonConvert.SerializeObject(processes, Newtonsoft.Json.Formatting.Indented);
            using (var streamWriter = new StreamWriter(path + "blacklist.json"))
            {
                streamWriter.Write(jsonString);
            }
        }

        public List<string> getAllProcesses()
        {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%", Environment.UserName);
            List<string>? processes = new List<string>();
            createConfigFileIfNotExists();
            using (StreamReader r = new StreamReader(path + "blacklist.json"))
            {
                string json = r.ReadToEnd();
                if (json != null && json != "")
                {
                    processes = JsonConvert.DeserializeObject<List<string>>(json);
                }
                return processes;
            }
        }

        public bool ContainsProcess(string processName)
        {
            return getAllProcesses().Contains(processName);
        }

        public List<string> LoadBlacklistFromFile()
        {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%", Environment.UserName);
            if (File.Exists(path + "blacklist.json"))
            {
                using (StreamReader r = new StreamReader(path + "blacklist.json"))
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

        public void SaveBlacklistToFile(string[] processNames)
        {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%", Environment.UserName);
            string json = JsonConvert.SerializeObject(processNames);
            File.WriteAllText(path + "blacklist.json", json);
        }

        public bool createConfigFileIfNotExists()
        {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%", Environment.UserName);
            if (File.Exists(path + "blacklist.json")) return false;
            System.IO.Directory.CreateDirectory(@path);
            var file = File.Create(@path + "blacklist.json");
            file.Close();
            return true;
        }
    }
}
