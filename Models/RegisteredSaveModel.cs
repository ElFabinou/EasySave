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

        public bool createConfigFileIfNotExists() {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%", Environment.UserName);
            if (File.Exists(path+"saveWorks.json")) return false;
            System.IO.Directory.CreateDirectory(@path);
            var file = File.Create(@path+"saveWorks.json");
            file.Close();
            return true;
        }

        public ReturnHandler addRegisteredSaveWork()
        {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString();
            createConfigFileIfNotExists();
            if (getRegisteredWork(registeredSaveWork.getSaveName()) != null) { return new ReturnHandler("Un travail de sauvegarde porte déjà ce nom. Action annulée.", ReturnHandler.ReturnTypeEnum.Error); }
            List<RegisteredSaveWork> registeredSaveWorksList = getAllRegisteredSaveWork();
            if(registeredSaveWorksList.Count >= 5) { return new ReturnHandler("Limite de travail de sauvegarde atteinte (5/5). Veuillez en supprimer un pour pouvoir en créer un. Action annulée.", ReturnHandler.ReturnTypeEnum.Error);}
            registeredSaveWorksList.Add(registeredSaveWork);
            string jsonString = JsonConvert.SerializeObject(registeredSaveWorksList, Newtonsoft.Json.Formatting.Indented);
            using (var streamWriter = new StreamWriter(path+"saveWorks.json"))
            {
                streamWriter.Write(jsonString);
            }
            return new ReturnHandler("Le travail de sauvegarde a bien été créé et enregistré !", ReturnHandler.ReturnTypeEnum.Success);

        }

        public ReturnHandler deleteRegisteredWork()
        {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString();
            createConfigFileIfNotExists();
            List<RegisteredSaveWork> registeredSaveWorksList = getAllRegisteredSaveWork();
            registeredSaveWorksList.RemoveAll(tempRegisteredSaveWork => tempRegisteredSaveWork.getSaveName() == registeredSaveWork.getSaveName());
            string jsonString = JsonConvert.SerializeObject(registeredSaveWorksList, Newtonsoft.Json.Formatting.Indented);
            using (var streamWriter = new StreamWriter(path+"saveWorks.json"))
            {
                streamWriter.Write(jsonString);
            }
            return new ReturnHandler("Le travail de sauvegarde a bien été supprimé !", ReturnHandler.ReturnTypeEnum.Success);
        }

        public List<RegisteredSaveWork> getAllRegisteredSaveWork() {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%", Environment.UserName);
            List<RegisteredSaveWork> registeredSaveWorksList = new List<RegisteredSaveWork>();
            createConfigFileIfNotExists();
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

        public ReturnHandler copyFilesToTarget()
        {
            try
            {
                DirectoryCopy(registeredSaveWork.getSourcePath(), registeredSaveWork.getTargetPath()+"\\"+registeredSaveWork.getSaveName(), true, registeredSaveWork.getType());
                return new ReturnHandler("Les fichiers ont bien été copiés !", ReturnHandler.ReturnTypeEnum.Success);
            }
            catch (Exception e)
            {
                return new ReturnHandler("Un erreur est survenue : "+e.ToString(), ReturnHandler.ReturnTypeEnum.Error);
            }
        }

        public void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs, RegisteredSaveWork.Type type)
        {
            try
            {
                // Get the subdirectories for the specified directory.
                DirectoryInfo dir = new DirectoryInfo(sourceDirName);

                if (!dir.Exists)
                {
                    throw new DirectoryNotFoundException(
                        "Source directory does not exist or could not be found: "
                        + sourceDirName);
                }

                DirectoryInfo[] dirs = dir.GetDirectories();
                // If the destination directory doesn't exist, create it.
                if (!Directory.Exists(destDirName))
                {
                    Directory.CreateDirectory(destDirName);
                }

                // Get the files in the directory and copy them to the new location.
                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo file in files)
                {
                    string temppath = Path.Combine(destDirName, file.Name);
                    FileInfo destFile = new FileInfo(temppath);
                    if ((int)type == 1)
                    {
                        if (!destFile.Exists || file.LastWriteTime > destFile.LastWriteTime)
                        {
                            file.CopyTo(temppath, true);
                        }
                    }
                    else
                    {
                        file.CopyTo(temppath, true);
                    }
                }

                // If copying subdirectories, copy them and their contents to new location.
                if (copySubDirs)
                {
                    foreach (DirectoryInfo subdir in dirs)
                    {
                        string temppath = Path.Combine(destDirName, subdir.Name);
                        DirectoryCopy(subdir.FullName, temppath, copySubDirs, type);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}