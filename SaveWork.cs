using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easysave
{
    internal class SaveWork
    {
        private string? sourcePath;
        private string? targetPath;
        private DateTime dateTime = DateTime.Now;
        private string saveName = "Sauvegarde-"+DateTime.Now.ToString();

        public void setSourcePath(string sourcePath)
        {
            if (pathExists(sourcePath))
            {
                this.sourcePath = sourcePath;
            }
        }

        public void setTargetPath(string targetPath)
        {
            if (pathExists(targetPath))
            {
                this.targetPath = targetPath;
            }
        }

        public string? getSourcePath()
        {
            return this.sourcePath;
        }

        public string? getTargetPath()
        {
            return this.targetPath;
        }

        public bool pathExists(string path)
        {
            return Directory.Exists(path);
        }
    }
}
