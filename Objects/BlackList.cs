using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easysave.Objects
{
    public class Blacklist
    {
        private readonly List<string> _processNames;

        public Blacklist()
        {
            _processNames = new List<string>();
        }

        public void AddProcessName(string processName)
        {
            _processNames.Add(processName);
        }

        public bool ContainsProcess(string processName)
        {
            bool result = _processNames.Contains(processName);
            return result;
        }
    }
}
