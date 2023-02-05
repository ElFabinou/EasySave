using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using easysave.Models;
using easysave.Objects;

namespace easysave.ViewModels
{
    public class BasicSaveWorkViewModel
    {
        private BasicSaveWork basicSaveWork;

        public BasicSaveWorkViewModel(BasicSaveWork saveWork)
        {
            basicSaveWork = saveWork;
        }

        public string initBasicSaveWork()
        {
            BasicSaveModel model = new BasicSaveModel(basicSaveWork);
            return model.copyFilesToTarget();
        }
    }
}
