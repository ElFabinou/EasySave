using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easysave.Objects
{
    public class LoggerHandler
    {

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
    }
}
