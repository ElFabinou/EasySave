using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easysave.Objects
{
    public class LoggerHandler
    {

        private string format = ConfigurationManager.AppSettings["configExtension"]!.ToString();
        private StateLog stateLog;
        private DailyLog dailyLog;

        public LoggerHandler(StateLog? stateLog = null, DailyLog? dailyLog = null)
        {
            this.stateLog=stateLog;
            this.dailyLog=dailyLog;
        }

        public StateLog getStateLog() { return stateLog; }
        public void setStateLog(StateLog stateLog)
        {
            this.stateLog = stateLog;
        }
        public DailyLog getDailyLog() { return dailyLog; }
        public void setDailyLog(DailyLog dailyLog)
        {
            this.dailyLog = dailyLog;
        }

        public string getFormat() { return format; }
        public void setFormat(string format) { this.format = format; }
    }
}
