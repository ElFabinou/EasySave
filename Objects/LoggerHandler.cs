using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easysave.Objects
{
    public class LoggerHandler
    {

        private RegisteredSaveWork saveWork;

        public LoggerHandler(RegisteredSaveWork saveWork)
        {
            this.saveWork=saveWork;
        }

        public RegisteredSaveWork getSaveWork() { return saveWork; }
        public void setSaveWork(RegisteredSaveWork registeredSaveWork)
        {
            this.saveWork = registeredSaveWork;
        }
    }
}
