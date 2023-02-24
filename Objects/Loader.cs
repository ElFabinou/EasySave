using easysave.Models;
using easysave.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace easysave.Objects
{
    public class Loader : INotifyPropertyChanged
    {
        public static List<Loader> loaders = new List<Loader>();
        public RegisteredSaveModel registeredSaveModel;
        public double percentage { get; set; }
        public FileInfo fileInfo { get; set; }
        public DirectoryInfo directoryInfo { get; set; }
        public bool isFile { get; set; }

        public LoadingViewGUI loadingViewGUI { get; set; }

        public Loader()
        {
            loadingViewGUI = new LoadingViewGUI(this);
            loadingViewGUI.Show();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public RegisteredSaveModel getSaveModel()
        {
            return registeredSaveModel;
        }

        public void setSaveModel(RegisteredSaveModel saveModel)
        {
            this.registeredSaveModel = saveModel;
        }

        public void setIsFile(bool isFile)
        {
            this.isFile = isFile;
        }

        public bool getIsFile()
        {
            return isFile;
        }

        public void setPercentage(double totalFile, double currentFile) {
            percentage = (currentFile/totalFile)*100;
            NotifyPropertyChanged();
        }
        
        public double getPercentage() { return percentage; }

        public string percentageToChar()
        {
            string loaderString = "";
            int i = 0;
            for (i = 0; i < (int)Math.Round(getPercentage(), 0); i++)
            {
                loaderString += "█";
            }
            return loaderString;
        }

        public void setFile(FileInfo fileInfo)
        {
            this.fileInfo = fileInfo;
        }

        public FileInfo getFile() { 
            return fileInfo; 
        }

        public void setFolder(DirectoryInfo directoryInfo)
        {
            this.directoryInfo = directoryInfo;
        }

        public DirectoryInfo getFolder()
        {
            return directoryInfo;
        }

    }
}
