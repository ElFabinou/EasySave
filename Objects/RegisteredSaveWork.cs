using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easysave.Objects
{
    public class RegisteredSaveWork : BasicSaveWork
    {

        public void setDateTime(DateTime dateTime)
        {
            this.dateTime = dateTime;
        }

        public DateTime getDateTime()
        {
            return dateTime;
        }
        public void setDate(DateTime dateTime)
        {
            this.dateTime = dateTime;
        }

        public void setSaveName(string saveName)
        {
            this.saveName = saveName;
        }

        public string getSaveName()
        {
            return saveName;
        }

    }
}
