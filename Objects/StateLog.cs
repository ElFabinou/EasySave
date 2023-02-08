using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easysave.Objects
{
    public class StateLog
    {
        public enum State
        {
            END,
            ACTIVE,
        }

        public string saveName;
        public string state = State.END.ToString();
        public string? fileName;
        public long totalFileSize = 0;
        public int totalFilesToCopy = 0;
        public int remainingFiles = 0;
        public double progression = 0;

        public void setFileName(string fileName)
        {
            this.fileName = fileName;
        }

        public string? getFileName() { return fileName; }

        public void setTotalFileSize(long totalFileSize)
        {
            this.totalFileSize = totalFileSize;
        }

        public long getTotalFileSize() { return totalFileSize; }

        public void setTotalFilesToCopy(int totalFilesToCopy)
        {
            this.totalFilesToCopy = totalFilesToCopy;
        }
        public int getTotalFilesToCopy() { return totalFilesToCopy; }

        public void setRemainingFiles(int remainingFiles)
        {
            this.remainingFiles = remainingFiles;
        }

        public int getRemainingFiles() { return remainingFiles; }

        public void setProgression(double progression)
        {
            this.progression = progression;
        }

        public string getState() { return state; }

        public void setState(State state)
        {
            this.state = state.ToString();
        }

        public double getProgression()
        {
            return progression;
        }

        public string getSaveName()
        {
            return saveName;
        }

        public void setSaveName(string saveName)
        {
            this.saveName=saveName;
        }
    }
}
