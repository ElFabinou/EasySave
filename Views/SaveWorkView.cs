using easysave.Objects;
using easysave.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static easysave.Objects.LanguageHandler;

namespace easysave.Views
{
    internal class SaveWorkView : MainView
    {

        public SaveWorkView(mode selectedMode)
        {
            this.language = Instance.rm;
            this.selectedMode = selectedMode;
        }

        public void initSlotCreation()
        {
            if (this.selectedMode == mode.Console)
            {
                RegisteredSaveWork registeredSaveWork = new RegisteredSaveWork();
                string? name = "";
                while (name == "" || name == null)
                {
                    Console.WriteLine($"{Environment.NewLine} [x]" + language.GetString("settings_choice_back"));
                    Console.WriteLine($"{Environment.NewLine}  " + language.GetString($"slotcreation_ask_name"));
                    name = Console.ReadLine();
                    if (name == "x") { mainMenu(); }
                }
                string? sourcePath = "";
                while (sourcePath == "" || sourcePath == null || !registeredSaveWork.pathExists(sourcePath))
                {
                    Console.WriteLine($"{Environment.NewLine} [x]" + language.GetString("settings_choice_back"));
                    Console.WriteLine($"{Environment.NewLine} " + language.GetString("slotcreation_ask_source_path"));
                    sourcePath = Console.ReadLine();
                    if (sourcePath == "x") { mainMenu(); }

                }
                string? targetPath = "";
                while (targetPath == "" || targetPath == null)
                {
                    Console.WriteLine($"{Environment.NewLine} [x]" + language.GetString("settings_choice_back"));
                    Console.WriteLine($"{Environment.NewLine} " + language.GetString("slotcreation_ask_target_path"));
                    targetPath = Console.ReadLine();
                    if (targetPath == "x") { mainMenu(); }

                }
                string? type = "";
                while (type == "" || type == null)
                {
                    Console.WriteLine($"{Environment.NewLine} [x]" + language.GetString("settings_choice_back") + $"{Environment.NewLine} ");
                    Console.WriteLine(language.GetString("slotcreation_ask_save_type"));
                    Console.WriteLine("[1] "+language.GetString("slotcreation_choice_save_type_complete"));
                    Console.WriteLine("[2] "+language.GetString("slotcreation_choice_save_type_differential"));
                    type = Console.ReadLine();
                    if (type == "x") { mainMenu(); }

                }
                registeredSaveWork.setType(RegisteredSaveWork.Type.Complet);
                if (type == "2") registeredSaveWork.setType(RegisteredSaveWork.Type.Differentiel);
                registeredSaveWork.setSaveName(name);
                registeredSaveWork.setSourcePath(sourcePath);
                registeredSaveWork.setTargetPath(targetPath);
                RegisteredSaveViewModel viewModel = new RegisteredSaveViewModel(registeredSaveWork);
                viewModel.initSlotCreation().Print();
                mainMenu();
            }
        }

        public void initSlotSelection()
        {
            if (selectedMode == mode.Console)
            {
                RegisteredSaveViewModel registeredSaveViewModel = new RegisteredSaveViewModel();
                List<RegisteredSaveWork> registeredSaveViewModelList = registeredSaveViewModel.initSlotSelection();
                Console.WriteLine("Choisissez un slot de sauvegarde à éditer : ");
                int i = 1;
                if (registeredSaveViewModelList.Count == 0)
                {
                    Console.WriteLine(language.GetString("slotselection_error_no_slot"));
                    mainMenu();
                }
                foreach (RegisteredSaveWork registeredSaveWork in registeredSaveViewModelList)
                {
                    Console.WriteLine("["+i+"] "+registeredSaveWork.getSaveName()+
                        "\n ├── "+language.GetString("slotselection_info_slot_type")+registeredSaveWork.getType().ToString() +
                        "\n ├── "+language.GetString("slotselection_info_slot_source")+registeredSaveWork.getSourcePath() +
                        "\n └── "+language.GetString("slotselection_info_slot_cible")+registeredSaveWork.getTargetPath());
                    i++;
                }
                int s = 0;
                while (s > registeredSaveViewModelList.Count || s < 1)
                {
                    try
                    {
                        Console.WriteLine(language.GetString("slotselection_ask_slot_valid_number"));
                        s = Convert.ToInt32(Console.ReadLine());
                    }
                    catch (Exception)
                    {
                        Console.WriteLine(language.GetString("slotselection_error_invalid_number"));
                    }
                }
                initSlotInteraction(registeredSaveViewModelList[s-1]);
            }
        }

        public void initSlotInteraction(RegisteredSaveWork registeredSaveWork)
        {
            if (selectedMode == mode.Console)
            {
                string? choice = "";
                while (choice == "" || choice == null || (choice != "1" && choice != "2"))
                {
                    Console.WriteLine(language.GetString("slotinteraction_ask_operation"));
                    Console.WriteLine(registeredSaveWork.getSaveName()+
                        "\n ├── "+language.GetString("slotselection_info_slot_type")+registeredSaveWork.getType().ToString() +
                        "\n ├── "+language.GetString("slotselection_info_slot_source")+registeredSaveWork.getSourcePath() +
                        "\n └── "+language.GetString("slotselection_info_slot_cible")+registeredSaveWork.getTargetPath());
                    Console.WriteLine("[1] "+language.GetString("slotinteraction_choice_run_save_work"));
                    Console.WriteLine("[2] "+language.GetString("slotinteraction_choice_delete_save_work"));
                    choice = Console.ReadLine();
                }
                if (choice == "1")
                {
                    RegisteredSaveViewModel viewModel = new RegisteredSaveViewModel(registeredSaveWork);
                    viewModel.initRegisteredSaveWork().Print();
                    mainMenu();
                }
                if (choice == "2")
                {
                    RegisteredSaveViewModel viewModel = new RegisteredSaveViewModel(registeredSaveWork);
                    viewModel.initSlotDeletion().Print();
                    mainMenu();
                }
            }
        }

    }
}
