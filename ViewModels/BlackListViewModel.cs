using easysave.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using easysave.Models;

namespace easysave.ViewModels
{
    public class BlacklistModelView
    {
        public void CallAddBlacklist(string processName) {
            BlacklistModel blacklist = new BlacklistModel();
            blacklist.AddProcessName(processName);
        }

        public void CallRemoveBlacklist(string processName)
        {
            BlacklistModel blacklist = new BlacklistModel();
            blacklist.RemoveProcessName(processName);
        }

        public List<string> GetBlacklist()
        {
            BlacklistModel blacklist = new BlacklistModel();
            List<string> list = blacklist.getAllProcesses();
            return list;
        }

        public bool CallStartProcess()
        {
            BlacklistModel blacklist = new BlacklistModel();
            bool result = blacklist.StartProcessMonitor();
            return result;
        }
    }


}
