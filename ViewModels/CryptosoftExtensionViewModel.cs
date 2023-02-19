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
using System.Windows.Markup;

namespace easysave.ViewModels
{
    public class CryptosoftExtensionViewModel
    {
        public void CallAddCryptosoftExtension(string extension) {
            CryptosoftExtensionModel cryptosoftExtensionModel = new CryptosoftExtensionModel();
            cryptosoftExtensionModel.AddCryptosoftExtension(extension);
        }

        public void CallRemoveCryptosoftExtension(string extension)
        {
            CryptosoftExtensionModel cryptosoftExtensionModel = new CryptosoftExtensionModel();
            cryptosoftExtensionModel.RemoveCryptosoftExtension(extension);
        }

        public List<string> GetExtensionList()
        {
            CryptosoftExtensionModel cryptosoftExtensionModel = new CryptosoftExtensionModel();
            List<string> list = cryptosoftExtensionModel.getExtensionList();
            return list;
        }

        public bool CheckFileExtension(string extension)
        {
            CryptosoftExtensionModel cryptosoftExtensionModel = new CryptosoftExtensionModel();
            bool result = cryptosoftExtensionModel.CheckFileExtension(extension);
            return result;
        }
    }


}
