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

        public RegisteredSaveViewModel(RegisteredSaveWork registeredSaveWork)
        {
                this.registeredSaveWork = registeredSaveWork;
        }

        /* public string initRegisteredSaveWork()
        {
            RegisteredSaveModel model = new RegisteredSaveModel(registeredSaveWork);
            return model.copyFilesToTarget();
        } */

        public string initSlotCreationAsync()
        {
            RegisteredSaveModel registeredSaveModel = new RegisteredSaveModel(registeredSaveWork);
            registeredSaveModel.writeInJsonAsync();
            return "ok";
        }
    }
}
