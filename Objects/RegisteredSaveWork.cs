using System;
using System.IO;

namespace easysave.Objects
{
    public class RegisteredSaveWork
    {

        public Type type { get; set; }
        public string sourcePath { get; set; }
        public string targetPath { get; set; }
        public string saveName { get; set; }

    public enum Type
        {
            Complet,
            Differentiel
        };

        public string getSaveName()
        {
            return saveName;
        }

        public void setSaveName(string saveName)
        {
            this.saveName = saveName;
        }

        public Type getType()
        {
            return type;
        }

        public void setType(Type type) {
            this.type = type;
        }

        public void setSourcePath(string sourcePath)
        {
            if (pathExists(sourcePath))
            {
                this.sourcePath = sourcePath;
            }
        }

        public void setTargetPath(string targetPath)
        {

            this.targetPath = targetPath;
        }

        public string getSourcePath()
        {
            return sourcePath;
        }

        public string getTargetPath()
        {
            return targetPath;
        }

        public bool pathExists(string path)
        {
            return Directory.Exists(path);
        }

    }
}
