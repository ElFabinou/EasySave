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
    public class PrioExtensionViewModel
    {
        public void CallAddPrioExtension(string extension) {
            PrioExtensionModel cryptosoftExtensionModel = new PrioExtensionModel();
            cryptosoftExtensionModel.AddPrioExtension(extension);
        }

        public void CallRemovePrioExtension(string extension)
        {
            PrioExtensionModel cryptosoftExtensionModel = new PrioExtensionModel();
            cryptosoftExtensionModel.RemovePrioExtension(extension);
        }

        public List<string> GetExtensionList()
        {
            PrioExtensionModel cryptosoftExtensionModel = new PrioExtensionModel();
            List<string> list = cryptosoftExtensionModel.getExtensionList();
            return list;
        }

        public bool CheckFileExtension(string extension)
        {
            PrioExtensionModel cryptosoftExtensionModel = new PrioExtensionModel();
            bool result = cryptosoftExtensionModel.CheckFileExtension(extension);
            return result;
        }
    }
}
