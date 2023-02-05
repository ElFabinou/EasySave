using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easysave.Objects
{
    public class LoggerHandler
    {

        public DateTime dateTime;
        public string type;
        public string content;

        public LoggerHandler(DateTime dateTime, string type, string content)
        {
            this.dateTime=dateTime;
            this.type=type;
            this.content=content;
        }
    }
}
