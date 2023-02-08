namespace easysave.Objects
{
    public class RegisteredSaveWork : BasicSaveWork
    {

        public Type type;

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
