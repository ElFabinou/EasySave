using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easysave.Objects
{
    public class DailyLog
    {
        public string saveName;
        public string source;
        public string target;
        public string destPath;
        public long fileSize;
        public DateTime dateTime;
        public long encryptTime;
        public double duration;

        public string getSaveName()
        {
            return saveName;
        }

        public void setSaveName(string saveName)
        {
            this.saveName=saveName;
        }

        public string getSource()
        {
            return source;
        }

        public void setSource(string source)
        {
            this.source=source;
        }

        public string getTarget()
        {
            return target;
        }

        public void setTarget(string target)
        {
            this.target = target;
        }

        public string getDestPath()
        {
            return destPath;
        }

        public void setDestPath(string destPath)
        {
            this.destPath = destPath;
        }

        public long getfileSize()
        {
            return fileSize;
        }

        public void setfileSize(long fileSize)
        {
            this.fileSize = fileSize;
        }

        public DateTime getDateTime()
        {
            return dateTime;
        }


        public void setDateTime(DateTime dateTime)
        {
            this.dateTime = dateTime;
        }

        public long getEncryptTime()
        {
            return encryptTime;
        }

        public void setEncryptTime(long encryptTime)
        {
            this.encryptTime = encryptTime;
        }


        public double getDuration()
        {
            return duration;
        }

        public void setDuration(double duration)
        {
            this.duration = duration;
        }
    }
}
