using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easysave.Objects
{
    public class RegisteredSaveWork : BasicSaveWork
    {
        public enum Type
        {
            Complet,
            Differentiel
        };

        public Type type;

        public void setSaveName(string saveName)
        {
            this.saveName = saveName;
        }

        public string getSaveName()
        {
            return saveName;
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
