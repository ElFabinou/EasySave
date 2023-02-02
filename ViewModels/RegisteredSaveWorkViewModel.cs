using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using easysave.Models;
using easysave.Objects;

namespace easysave.ViewModels
{
    public class RegisteredSaveViewModel
    {
        public RegisteredSaveWork registeredSaveWork;

        public RegisteredSaveViewModel(RegisteredSaveWork? registeredSaveWork = null)
        {
                this.registeredSaveWork = registeredSaveWork;
        }

        /* public string initRegisteredSaveWork()
        {
            RegisteredSaveModel model = new RegisteredSaveModel(registeredSaveWork);
            return model.copyFilesToTarget();
        } */

        public string initSlotCreation()
        {
            RegisteredSaveModel registeredSaveModel = new RegisteredSaveModel(registeredSaveWork);
            registeredSaveModel.writeInJson();
            return "ok";
        }

        public List<RegisteredSaveWork> initSlotSelection()
        {
            RegisteredSaveModel registeredSaveModel = new RegisteredSaveModel();
            return registeredSaveModel.getAllRegisteredSaveWork();
        }
    }
}
