using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using easysave.Objects;

namespace easysave.Models
{
    internal class BasicSaveModel
    {

        private BasicSaveWork basicSaveWork;

        public BasicSaveModel(BasicSaveWork basicSaveWork)
        {
            this.basicSaveWork = basicSaveWork;
        }

        public string copyFilesToTarget()
        {
            try
            {
                string sourcePath = basicSaveWork.getSourcePath();
                string targetPath = basicSaveWork.getTargetPath();
                string test;
              

                // Create target directory if it doesn't exist
                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }

                // Copy files from source to target directory
                foreach (string file in Directory.GetFiles(sourcePath))
                {
                    File.Copy(file, Path.Combine(targetPath, Path.GetFileName(file)));
                }
                return "Les fichiers ont bien été copiés.";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
