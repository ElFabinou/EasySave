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
        }

        public void initPause()
        {
            RegisteredSaveModel registeredSaveModel = new RegisteredSaveModel(registeredSaveWork);
            registeredSaveModel.Pause();
        }

        public ReturnHandler initSlotCreation()
        {
            RegisteredSaveModel registeredSaveModel = new RegisteredSaveModel(registeredSaveWork);
            return registeredSaveModel.addRegisteredSaveWork();
        }

        public List<RegisteredSaveWork> initSlotSelection()
        {
            RegisteredSaveModel registeredSaveModel = new RegisteredSaveModel(registeredSaveWork);
            return registeredSaveModel.getAllRegisteredSaveWork();
        }

        public ReturnHandler initSlotDeletion()
        {
            RegisteredSaveModel registeredSaveModel = new RegisteredSaveModel(registeredSaveWork);
            return registeredSaveModel.deleteRegisteredWork();
        }

        public async Task<ReturnHandler> initRegisteredSaveWork()
        {
            RegisteredSaveModel registeredSaveModel = new RegisteredSaveModel(registeredSaveWork);
            return await registeredSaveModel.copyFilesToTarget();
        }

        public void notifyViewPercentage(Loader loader, LoadingViewGUI? loadingViewGUI = null)
        {
            SaveWorkView saveWorkView = new SaveWorkView();
            saveWorkView.sendPercentage(loader);
            loadingViewGUI = loader.loadingViewGUI;
            if(loadingViewGUI!= null)
            {
                loadingViewGUI.Dispatcher.Invoke(() =>
                {
                    loadingViewGUI.setPercentage(loader);
                });
            }
        }

        public void initPause(RegisteredSaveModel _registeredSaveModel)
        {
            RegisteredSaveModel registeredSaveModel = _registeredSaveModel;
            registeredSaveModel.Pause();
        }

        public void initStop(RegisteredSaveModel _registeredSaveModel)
        {
            RegisteredSaveModel registeredSaveModel = _registeredSaveModel;
            registeredSaveModel.Stop();
        }

        public void blacklistInterrupt(Loader loader)
        {
            LoadingViewGUI loadingViewGUI = loader.loadingViewGUI;
            loadingViewGUI.Dispatcher.Invoke(() =>
            {
                loadingViewGUI.displayInterruptBlacklist();
            });
        }

    }
}