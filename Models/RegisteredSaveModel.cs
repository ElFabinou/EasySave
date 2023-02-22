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
using System.Threading;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace easysave.Models
{
    public class RegisteredSaveModel
    {
        public RegisteredSaveWork? registeredSaveWork;

        private int doneFiles = 0;

        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        public RegisteredSaveModel(RegisteredSaveWork? registeredSaveWork = null)
        {
            this.registeredSaveWork = registeredSaveWork;
        }

        private bool isPaused = false;
        private bool isStopped = false;

        public void Pause()
        {
            if(isPaused)
            {
                isPaused = false;
                lock (this)
                {
                    Monitor.PulseAll(this);
                }
                return;
            }
            isPaused = true;
        }

        public void Stop()
        {
            if (isStopped)
            {
                isStopped = true;
                return;
            }
            isStopped = true;
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
            BlacklistModelView blacklist = new BlacklistModelView();
            bool resultBl = blacklist.CallStartProcess();
            if (!resultBl) {
                return new ReturnHandler("Error : Blacklist", ReturnHandler.ReturnTypeEnum.Error);
            }
            try
            {
                this.language = Instance.rm;
                DirectoryInfo root = new DirectoryInfo(registeredSaveWork!.getSourcePath());
                var fileCount = System.IO.Directory.GetDirectories(registeredSaveWork.getSourcePath(), "*", SearchOption.AllDirectories).Count() + System.IO.Directory.GetFiles(registeredSaveWork.getSourcePath(), "*.*", SearchOption.AllDirectories).Count(); ;
                Loader loader = null;
                if (System.Threading.Thread.CurrentThread.GetApartmentState() != System.Threading.ApartmentState.STA)
                {
                    System.Threading.Thread thread = new System.Threading.Thread(() =>
                    {
                        // Remove "Loader" type from this line
                        loader = new Loader();
                        loader.setSaveModel(this);
                        System.Windows.Threading.Dispatcher.Run();
                    });
                    thread.SetApartmentState(System.Threading.ApartmentState.STA);
                    thread.Start();
                    System.Threading.Thread.Sleep(500);
                }
                else
                {
                    loader = new Loader();
                    loader.setSaveModel(this);
                    System.Windows.Threading.Dispatcher.Run();
                }

                // Now the "loader" variable is defined in both branches of the if statement
                loader.setPercentage(fileCount, 0);
                this.doneFiles = 0;
                DirectoryCopy(registeredSaveWork.getSourcePath(), registeredSaveWork.getTargetPath()+"\\"+registeredSaveWork.getSaveName(), true, registeredSaveWork.getType(), fileCount, loader);
                callLogger(100, 0, 0, 0, registeredSaveWork.getSaveName(), 0, StateLog.State.END, registeredSaveWork.getSourcePath(), registeredSaveWork.getTargetPath());
                return new ReturnHandler(language.GetString("copy-file"), ReturnHandler.ReturnTypeEnum.Success);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error : "+e.ToString(), ReturnHandler.ReturnTypeEnum.Error);
                return new ReturnHandler("Error : "+e.ToString(), ReturnHandler.ReturnTypeEnum.Error);
            }
        }

        public async void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs, RegisteredSaveWork.Type type, int totalFile, Loader loader)
        {
            BlacklistModelView blacklist = new BlacklistModelView();
            PrioExtensionViewModel prioExtensionViewModel = new PrioExtensionViewModel();
            try
            {
                lock (this)
                {
                    while (isPaused)
                    {
                        Monitor.Wait(this);
                    }
                }
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
                bool resultBl = blacklist.CallStartProcess();
                if (!resultBl)
                {
                    Pause();
                    registeredSaveViewModel.blacklistInterrupt(loader);
                }
                if (!Directory.Exists(destDirName))
                {
                    if (isStopped) { _semaphore.Release(); return; }
                    _semaphore.Wait();
                    Directory.CreateDirectory(destDirName);
                    loader.setPercentage(totalFile, doneFiles);
                    loader.setIsFile(false);
                    loader.setFolder(dir);
                    loader.setSaveModel(this);
                    registeredSaveViewModel.notifyViewPercentage(loader);
                    callLogger(loader.getPercentage(), 0, totalFile, doneFiles, registeredSaveWork.getSaveName(), 0, StateLog.State.ACTIVE, sourceDirName, destDirName);
                    _semaphore.Release();
                }
                // Get the files in the directory and copy them to the new location.
                FileInfo[] files = dir.GetFiles();
                orderFilesBy(files);
                foreach (FileInfo file in files)
                {
                    if (isStopped) { _semaphore.Release(); return; }
                    resultBl = blacklist.CallStartProcess();
                    if (!resultBl)
                    {
                        Pause();
                        registeredSaveViewModel.blacklistInterrupt(loader);
                    }
                    ++doneFiles;
                    string temppath = Path.Combine(destDirName, file.Name);
                    string sourcepath = Path.Combine(sourceDirName, file.Name);
                    FileInfo destFile = new FileInfo(temppath);
                    if ((int)type == 1)
                    {
                        if (!destFile.Exists || file.LastWriteTime > destFile.LastWriteTime)
                        {
                            double totalTime = DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                            if (file.Length > 100000000)
                            {
                                _semaphore.Wait();
                            }
                                try
                            {
                                using (var fileStream = new FileStream(sourcepath, FileMode.Open, FileAccess.Read))
                                {
                                    using (var streamReader = new BinaryReader(fileStream))
                                    {
                                        using (var fileN = new FileStream(temppath, FileMode.Create, FileAccess.Write))
                                        {
                                            using (var streamWriter = new BinaryWriter(fileN))
                                            {
                                                while (!isStopped && streamReader.BaseStream.Position < streamReader.BaseStream.Length)
                                                {
                                                    if (isStopped) { _semaphore.Release(); return; }
                                                    byte[] buffer = streamReader.ReadBytes(1024);
                                                    streamWriter.Write(buffer);
                                                    loader.setPercentage(totalFile, doneFiles);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                // Handle any exceptions here
                            }
                            if (isStopped) { _semaphore.Release(); return; }
                            _semaphore.Release();
                            if (!isStopped)
                            {
                                if (file.Length > 100000000)
                                {
                                    _semaphore.Wait();
                                }
                                resultBl = blacklist.CallStartProcess();
                                if (!resultBl)
                                {
                                    Pause();
                                    registeredSaveViewModel.blacklistInterrupt(loader);
                                }
                                totalTime = DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds - totalTime;
                                loader.setFile(file);
                                loader.setIsFile(true);
                                loader.setSaveModel(this);
                                registeredSaveViewModel.notifyViewPercentage(loader);
                                callLogger(loader.getPercentage(), file.Length, totalFile, doneFiles, registeredSaveWork.getSaveName(), totalTime, StateLog.State.ACTIVE, sourcepath, temppath);
                                _semaphore.Release();
                            }
                        }
                    }
                    else
                    {
                        if (isStopped) { _semaphore.Release(); return; }
                        _semaphore.Wait();
                        double totalTime = DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                        resultBl = blacklist.CallStartProcess();
                        if (!resultBl)
                        {
                            Pause();
                            registeredSaveViewModel.blacklistInterrupt(loader);
                        }
                        try
                        {
                            using (var fileStream = new FileStream(sourcepath, FileMode.Open, FileAccess.Read))
                            {
                                using (var streamReader = new BinaryReader(fileStream))
                                {
                                    using (var fileN = new FileStream(temppath, FileMode.Create, FileAccess.Write))
                                    {
                                        using (var streamWriter = new BinaryWriter(fileN))
                                        {
                                            while (!isStopped && streamReader.BaseStream.Position < streamReader.BaseStream.Length)
                                            {
                                                if (isStopped) { _semaphore.Release(); return;  }
                                                byte[] buffer = streamReader.ReadBytes(1024);
                                                streamWriter.Write(buffer);
                                                loader.setPercentage(totalFile, doneFiles);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // Handle any exceptions here
                        }
                        _semaphore.Release();
                        if (isStopped) { _semaphore.Release(); return; }
                        if (!isStopped)
                        {
                            _semaphore.Wait();
                            resultBl = blacklist.CallStartProcess();
                            if (!resultBl)
                            {
                                Pause();
                                registeredSaveViewModel.blacklistInterrupt(loader);
                            }
                            totalTime = DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds - totalTime;
                            loader.setFile(file);
                            loader.setIsFile(true);
                            loader.setSaveModel(this);
                            registeredSaveViewModel.notifyViewPercentage(loader);
                            callLogger(loader.getPercentage(), file.Length, totalFile, doneFiles, registeredSaveWork.getSaveName(), totalTime, StateLog.State.ACTIVE, sourcepath, temppath);
                            _semaphore.Release();
                        }
                    }
                    lock (this)
                    {
                        while (isPaused)
                        {
                            Monitor.Wait(this);
                        }
                    }
                }
                // If copying subdirectories, copy them and their contents to new location.
                if (copySubDirs)
                {
                    foreach (DirectoryInfo subdir in dirs)
                    {
                        if (isStopped) { _semaphore.Release(); return; }
                        _semaphore.Wait();
                        resultBl = blacklist.CallStartProcess();
                        if (!resultBl)
                        {
                            Pause();
                        }
                        ++doneFiles;
                        string temppath = Path.Combine(destDirName, subdir.Name);
                        string sourcepath = Path.Combine(sourceDirName, subdir.Name);
                        loader.setPercentage(totalFile, doneFiles);
                        loader.setIsFile(false);
                        loader.setFolder(subdir);
                        loader.setSaveModel(this);
                        registeredSaveViewModel.notifyViewPercentage(loader);
                        callLogger(loader.getPercentage(), 0, totalFile, doneFiles, registeredSaveWork.getSaveName(), 0, StateLog.State.ACTIVE, sourcepath, temppath);
                        lock (this)
                        {
                            while (isPaused)
                            {
                                Monitor.Wait(this);
                            }
                        }
                        _semaphore.Release();
                        DirectoryCopy(subdir.FullName, temppath, copySubDirs, type, totalFile, loader);
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

        public FileInfo[] orderFilesBy(FileInfo[] files)
        {
            // Extensions à trier en premier
            PrioExtensionViewModel prioExtensionViewModel = new PrioExtensionViewModel();
            List<string> preferredExtensions = prioExtensionViewModel.GetExtensionList();
            // Trier les fichiers en fonction de leur extension (extensions préférées en premier)
            Array.Sort(files, (a, b) =>
            {
                // Récupérer les extensions des deux fichiers
                string extA = a.Extension.ToLower();
                string extB = b.Extension.ToLower();

                // Si les deux fichiers ont la même extension, les trier par ordre alphabétique
                if (extA == extB)
                {
                    return string.Compare(a.Name, b.Name);
                }

                // Sinon, trier les fichiers avec les extensions préférées en premier
                if (preferredExtensions.Contains(extA))
                {
                    return -1;
                }
                else if (preferredExtensions.Contains(extB))
                {
                    return 1;
                }
                else
                {
                    return string.Compare(a.Name, b.Name);
                }
            });
            return files;
        }
    }
}