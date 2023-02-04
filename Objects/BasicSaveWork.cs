using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easysave.Objects
{
    public class BasicSaveWork
    {

        public string sourcePath;
        public string targetPath;
        public string saveName = "Sauvegarde-" + DateTime.Now.ToString();


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
