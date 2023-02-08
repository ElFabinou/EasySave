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



    }
}
