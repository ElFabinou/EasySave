using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using easysave.Models;
using easysave.Objects;
using easysave.Views;

namespace easysave.ViewModels
{
    public class RegisteredSaveViewModel
    {
        public RegisteredSaveWork? registeredSaveWork;
        public RegisteredSaveModel? registeredSaveModel;

        public RegisteredSaveViewModel(RegisteredSaveWork? registeredSaveWork = null)
        {
                this.registeredSaveWork = registeredSaveWork;
                this.registeredSaveModel = new RegisteredSaveModel(registeredSaveWork);
        }

        public void initPause()
        {
            RegisteredSaveModel registeredSaveModel = this.registeredSaveModel;
            registeredSaveModel.Pause();
        }

        public ReturnHandler initSlotCreation()
        {
            RegisteredSaveModel registeredSaveModel = this.registeredSaveModel;
            return registeredSaveModel.addRegisteredSaveWork();
        }

        public List<RegisteredSaveWork> initSlotSelection()
        {
            RegisteredSaveModel registeredSaveModel = this.registeredSaveModel;
            return registeredSaveModel.getAllRegisteredSaveWork();
        }

        public ReturnHandler initSlotDeletion()
        {
            RegisteredSaveModel registeredSaveModel = this.registeredSaveModel;
            return registeredSaveModel.deleteRegisteredWork();
        }

        public async Task<ReturnHandler> initRegisteredSaveWork()
        {
            RegisteredSaveModel registeredSaveModel = this.registeredSaveModel;
            return await registeredSaveModel.copyFilesToTarget();
        }

        public void notifyViewPercentage(Loader loader, LoadingViewGUI? loadingViewGUI = null)
        {
            SaveWorkView saveWorkView = new SaveWorkView();
            saveWorkView.sendPercentage(loader);
            if(loadingViewGUI!= null)
            {
                loadingViewGUI.Dispatcher.Invoke(() =>
                {
                    loadingViewGUI.setPercentage(loader);
                });
            }
        }
    }
}