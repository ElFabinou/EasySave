using easysave.Objects;
using System.Configuration;
using System.Xml;
using System.Text.Json;
using Newtonsoft.Json;

namespace easysave.Models
{
    public class RegisteredSaveModel
    {
        public RegisteredSaveWork registeredSaveWork;

        public RegisteredSaveModel(RegisteredSaveWork registeredSaveWork)
        {
            this.registeredSaveWork = registeredSaveWork;
        }

        public void createConfigFileIfNotExists() {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString();
            if (File.Exists(path)) return;
            var file = System.IO.File.Create(@path+"saveWorks.json");
            file.Close();
        }

        public void writeInJsonAsync()
        {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString();
            createConfigFileIfNotExists();
            string jsonString = JsonConvert.SerializeObject(registeredSaveWork, Newtonsoft.Json.Formatting.Indented);
            var myFile = File.Create(path+"saveWorks.json");

            using (var streamWriter = new StreamWriter(path+"saveWorks.json", append:true))
            {
                streamWriter.Write(content+jsonString);
            }

        }
    }
}