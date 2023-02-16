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
    public class CryptosoftExtensionModelView
    {
        public void CallAddBlacklist(string extension) {
            CryptosoftExtensionModel cryptosoftExtensionModel = new CryptosoftExtensionModel();
            cryptosoftExtensionModel.AddCryptosoftExtension(extension);
        }

        public void CallRemoveBlacklist(string extension)
        {
            CryptosoftExtensionModel cryptosoftExtensionModel = new CryptosoftExtensionModel();
            cryptosoftExtensionModel.RemoveCryptosoftExtension(extension);
        }

        public List<string> GetBlacklist()
        {
            CryptosoftExtensionModel cryptosoftExtensionModel = new CryptosoftExtensionModel();
            List<string> list = cryptosoftExtensionModel.getExtensionList();
            return list;
        }

        public bool CallStartProcess()
        {
            CryptosoftExtensionModel cryptosoftExtensionModel = new CryptosoftExtensionModel();
            //bool result = cryptosoftExtensionModel.StartCheckCryptosoft();
            return result;
        }
    }


}
