using easysave.Objects;
using System.Configuration;
using System.Xml;
using System.Text.Json;
using Newtonsoft.Json;
using System.IO;
using System;
using easysave.ViewModels;
using System.Drawing;
using System.Resources;
using static easysave.Objects.LanguageHandler;
using System.Collections.Generic;
using System.Linq;
using easysave.Views;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace easysave.Models
{
    public class RegisteredSaveModel
    {
        public RegisteredSaveWork? registeredSaveWork;

        private int doneFiles = 0;


        public RegisteredSaveModel(RegisteredSaveWork? registeredSaveWork = null)
        {
            this.registeredSaveWork = registeredSaveWork;
        }

        public ResourceManager language;
        public bool createConfigFileIfNotExists()
        {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%", Environment.UserName);
            if (File.Exists(path+"saveWorks.json")) return false;
            System.IO.Directory.CreateDirectory(@path);
            var file = File.Create(@path+"saveWorks.json");
            file.Close();
            return true;
        }

        public ReturnHandler addRegisteredSaveWork()
        {
            this.language = Instance.rm;
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%", Environment.UserName);
            createConfigFileIfNotExists();
            if (getRegisteredWork(registeredSaveWork!.getSaveName()) != null) { return new ReturnHandler("Un travail de sauvegarde porte déjà ce nom. Action annulée.", ReturnHandler.ReturnTypeEnum.Error); }
            List<RegisteredSaveWork> registeredSaveWorksList = getAllRegisteredSaveWork();
            registeredSaveWorksList.Add(registeredSaveWork);
            string jsonString = JsonConvert.SerializeObject(registeredSaveWorksList, Newtonsoft.Json.Formatting.Indented);
            using (var streamWriter = new StreamWriter(path+"saveWorks.json"))
            {
                streamWriter.Write(jsonString);
            }
            return new ReturnHandler(language.GetString("save-work"), ReturnHandler.ReturnTypeEnum.Success);

        }

        public ReturnHandler deleteRegisteredWork()
        {
            this.language = Instance.rm;
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%", Environment.UserName);
            createConfigFileIfNotExists();
            List<RegisteredSaveWork> registeredSaveWorksList = getAllRegisteredSaveWork();
            registeredSaveWorksList.RemoveAll(tempRegisteredSaveWork => tempRegisteredSaveWork.getSaveName() == registeredSaveWork!.getSaveName());
            string jsonString = JsonConvert.SerializeObject(registeredSaveWorksList, Newtonsoft.Json.Formatting.Indented);
            using (var streamWriter = new StreamWriter(path+"saveWorks.json"))
            {
                streamWriter.Write(jsonString);
            }
            return new ReturnHandler(language.GetString("delete-work"), ReturnHandler.ReturnTypeEnum.Success);
        }

        public List<RegisteredSaveWork> getAllRegisteredSaveWork()
        {
            string path = ConfigurationManager.AppSettings["configPath"]!.ToString().Replace("%username%", Environment.UserName);
            List<RegisteredSaveWork>? registeredSaveWorksList = new List<RegisteredSaveWork>();
            createConfigFileIfNotExists();
            using (StreamReader r = new StreamReader(path+"saveWorks.json"))
            {
                string json = r.ReadToEnd();
                if (json != null && json != "")
                {
                    registeredSaveWorksList = JsonConvert.DeserializeObject<List<RegisteredSaveWork>>(json);
                }
                return registeredSaveWorksList;
            }
        }

        public RegisteredSaveWork? getRegisteredWork(string name)
        {
            List<RegisteredSaveWork>? registeredSaveWorksList = getAllRegisteredSaveWork();
            foreach (RegisteredSaveWork? registeredSaveWork in registeredSaveWorksList)
            {
                if (registeredSaveWork.getSaveName() == name)
                {
                    return registeredSaveWork;
                }
            }
            return null;
        }

        public async Task<ReturnHandler> copyFilesToTarget()
        {
            try
            {
                this.language = Instance.rm;
                DirectoryInfo root = new DirectoryInfo(registeredSaveWork!.getSourcePath());
                var fileCount = System.IO.Directory.GetDirectories(registeredSaveWork.getSourcePath(), "*", SearchOption.AllDirectories).Count() + System.IO.Directory.GetFiles(registeredSaveWork.getSourcePath(), "*.*", SearchOption.AllDirectories).Count(); ;
                Loader loader = new Loader();
                LoadingViewGUI loadingViewGUI = null;
                if (System.Threading.Thread.CurrentThread.ApartmentState != System.Threading.ApartmentState.STA)
                {
                    System.Threading.Thread thread = new System.Threading.Thread(() =>
                    {
                        loadingViewGUI = new LoadingViewGUI();
                        loadingViewGUI.Show();
                        System.Windows.Threading.Dispatcher.Run();
                    });
                    thread.SetApartmentState(System.Threading.ApartmentState.STA);
                    thread.Start();
                    System.Threading.Thread.Sleep(500);
                }
                else
                {
                    loadingViewGUI = new LoadingViewGUI();
                    loadingViewGUI.Show();
                    System.Windows.Threading.Dispatcher.Run();
                }
                loader.setPercentage(fileCount, 0);
                this.doneFiles = 0;
                DirectoryCopy(registeredSaveWork.getSourcePath(), registeredSaveWork.getTargetPath()+"\\"+registeredSaveWork.getSaveName(), true, registeredSaveWork.getType(), fileCount, loader, loadingViewGUI);
                callLogger(100, 0, 0, 0, registeredSaveWork.getSaveName(), 0, StateLog.State.END, registeredSaveWork.getSourcePath(), registeredSaveWork.getTargetPath());
                return new ReturnHandler(language.GetString("copy-file"), ReturnHandler.ReturnTypeEnum.Success);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error : "+e.ToString(), ReturnHandler.ReturnTypeEnum.Error);
                return new ReturnHandler("Error : "+e.ToString(), ReturnHandler.ReturnTypeEnum.Error);
            }
        }

        public async void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs, RegisteredSaveWork.Type type, int totalFile, Loader loader, LoadingViewGUI loadingViewGUI)
        {
            try
            {
                RegisteredSaveViewModel registeredSaveViewModel = new RegisteredSaveViewModel();
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
                    loader.setPercentage(totalFile, doneFiles);
                    loader.setIsFile(false);
                    loader.setFolder(dir);
                    registeredSaveViewModel.notifyViewPercentage(loader, loadingViewGUI);
                    callLogger(loader.getPercentage(), 0, totalFile, doneFiles, registeredSaveWork.getSaveName(), 0, StateLog.State.ACTIVE, sourceDirName, destDirName);
                }

                // Get the files in the directory and copy them to the new location.
                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo file in files)
                {
                    ++doneFiles;
                    string temppath = Path.Combine(destDirName, file.Name);
                    string sourcepath = Path.Combine(sourceDirName, file.Name);
                    FileInfo destFile = new FileInfo(temppath);
                    if ((int)type == 1)
                    {
                        if (!destFile.Exists || file.LastWriteTime > destFile.LastWriteTime)
                        {
                            double totalTime = DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                            file.CopyTo(temppath, true);
                            loader.setPercentage(totalFile, doneFiles);
                            totalTime = DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds - totalTime;
                            loader.setFile(file);
                            loader.setIsFile(true);
                            registeredSaveViewModel.notifyViewPercentage(loader, loadingViewGUI);
                            callLogger(loader.getPercentage(), file.Length, totalFile, doneFiles, registeredSaveWork.getSaveName(), totalTime, StateLog.State.ACTIVE, sourcepath, temppath);
                        }
                    }
                    else
                    {
                        double totalTime = DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                        file.CopyTo(temppath, true);
                        loader.setPercentage(totalFile, doneFiles);
                        totalTime = DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds - totalTime;
                        loader.setFile(file);
                        loader.setIsFile(true);
                        registeredSaveViewModel.notifyViewPercentage(loader, loadingViewGUI);
                        callLogger(loader.getPercentage(), file.Length, totalFile, doneFiles, registeredSaveWork.getSaveName(), totalTime, StateLog.State.ACTIVE, sourcepath, temppath);
                    }
                }

                // If copying subdirectories, copy them and their contents to new location.
                if (copySubDirs)
                {
                    foreach (DirectoryInfo subdir in dirs)
                    {
                        ++doneFiles;
                        string temppath = Path.Combine(destDirName, subdir.Name);
                        string sourcepath = Path.Combine(sourceDirName, subdir.Name);
                        loader.setPercentage(totalFile, doneFiles);
                        loader.setIsFile(false);
                        loader.setFolder(subdir);
                        registeredSaveViewModel.notifyViewPercentage(loader, loadingViewGUI);
                        DirectoryCopy(subdir.FullName, temppath, copySubDirs, type, totalFile, loader, loadingViewGUI);
                        callLogger(loader.getPercentage(), 0, totalFile, doneFiles, registeredSaveWork.getSaveName(), 0, StateLog.State.ACTIVE, sourcepath, temppath);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public void callLogger(double progression, long fileSize, int totalFiles, int doneFiles, string saveName, double duration, StateLog.State state, string sourcePath, string destPath)
        {
            StateLog stateLog = new StateLog();
            stateLog!.setProgression(progression);
            stateLog.setTotalFilesToCopy(totalFiles);
            stateLog.setRemainingFiles(totalFiles-doneFiles);
            stateLog.setState(state);
            stateLog.setTotalFileSize(fileSize);
            DailyLog dailyLog = new DailyLog();
            dailyLog.setSaveName(saveName);
            dailyLog.setDuration(0);
            dailyLog.setfileSize(fileSize);
            dailyLog.setSaveName(saveName);
            dailyLog.setDuration(duration);
            dailyLog.setSource(sourcePath);
            dailyLog.setDestPath(destPath);
            LoggerHandler loggerHandler = new LoggerHandler(stateLog, dailyLog);
            LoggerHandlerModel loggerModel = new LoggerHandlerModel(loggerHandler);
            loggerModel.updateStateLog();
            loggerModel.updateDailyLog();
        }
    }
}