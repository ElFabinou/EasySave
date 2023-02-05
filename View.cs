using easysave.Objects;
using easysave.ViewModels;
using System;
using System.Resources;
using System.Xml.Serialization;
using static easysave.Objects.LanguageHandler;

namespace easysave
{
    internal class View
    {
        public enum mode
        {
            Console,
            GUI
        };

        public mode selectedMode;

        public View(mode selectedMode = mode.Console) {
            this.selectedMode = selectedMode;
        }

        public ResourceManager language;

        public void mainMenu()
        {
            if (this.selectedMode == mode.Console)
            {
                this.language = Instance.rm;
                Console.WriteLine("  ______                 _____                    _____ _      _____ \r\n |  ____|               / ____|                  / ____| |    |_   _|\r\n | |__   __ _ ___ _   _| (___   __ ___   _____  | |    | |      | |  \r\n |  __| / _` / __| | | |\\___ \\ / _` \\ \\ / / _ \\ | |    | |      | |  \r\n | |___| (_| \\__ \\ |_| |____) | (_| |\\ V /  __/ | |____| |____ _| |_ \r\n |______\\__,_|___/\\__, |_____/ \\__,_| \\_/ \\___|  \\_____|______|_____|\r\n                   __/ |                                             \r\n                  |___/                                              \r\n\r\n");
                Console.WriteLine("\n");
                Console.ForegroundColor= ConsoleColor.Yellow;
                Console.WriteLine(language.GetString("menu_title"));
                Console.ForegroundColor= ConsoleColor.Gray;
                string? choice = "";
                int attempt = 0;
                while (choice != "1" && choice != "2" && choice != "3" && choice != "4")
                {
                    Console.WriteLine("[1] "+language.GetString("menu_basic_save"));
                    Console.WriteLine("[2] "+language.GetString("menu_new_save_work"));
                    Console.WriteLine("[3] "+language.GetString("menu_work_list"));
                    Console.WriteLine("[4] "+language.GetString("menu_settings")); 
                    choice = Console.ReadLine();
                    attempt++;
                }
                switch (choice)
                {
                    case "1":
                        initBasicSaveWork();
                        break;
                    case "2":
                        initSlotSelection();
                        break;
                    case "3":
                        initSlotCreation();
                        break;
                    case "4":
                        initSettings();
                        break;
                }
            }
            else
            {
                Console.WriteLine(language.GetString("menu_error_load"));
            }
        }

        private void initSlotCreation()
        {
            if (this.selectedMode == mode.Console)
            {
                RegisteredSaveWork registeredSaveWork = new RegisteredSaveWork();
                string? name = "";
                while (name == "" || name == null)
                {
                    Console.WriteLine(language.GetString("slotcreation_ask_name"));
                    name = Console.ReadLine();
                }
                string? sourcePath = "";
                while (sourcePath == "" || sourcePath == null || !registeredSaveWork.pathExists(sourcePath))
                {
                    Console.WriteLine(language.GetString("slotcreation_ask_source_path"));
                    sourcePath = Console.ReadLine();
                }
                string? targetPath = "";
                while (targetPath == "" || targetPath == null)
                {
                    Console.WriteLine(language.GetString("slotcreation_ask_target_path"));
                    targetPath = Console.ReadLine();
                }
                string? type = "";
                while (type == "" || type == null)
                {
                    Console.WriteLine(language.GetString("slotcreation_ask_save_type"));
                    Console.WriteLine("[1] "+language.GetString("slotcreation_choice_save_type_complete"));
                    Console.WriteLine("[2] "+language.GetString("slotcreation_choice_save_type_differential"));
                    type = Console.ReadLine();
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
                Console.WriteLine("Choissez un slot de sauvegarde à éditer : ");
                int i = 1;
                if (registeredSaveViewModelList.Count == 0) {
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
                    try {
                        Console.WriteLine(language.GetString("slotselection_ask_slot_valid_number"));
                        s = Convert.ToInt32(Console.ReadLine());
                    }catch(Exception)
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
                if(choice == "1")
                {
                    RegisteredSaveViewModel viewModel = new RegisteredSaveViewModel(registeredSaveWork);
                    viewModel.initRegisteredSaveWork().Print();
                    mainMenu();
                }
                if(choice == "2")
                {
                    RegisteredSaveViewModel viewModel = new RegisteredSaveViewModel(registeredSaveWork);
                    viewModel.initSlotDeletion().Print();
                    mainMenu();
                }
            }
        }

        public void initBasicSaveWork()
        {
            if (selectedMode == mode.Console)
            {
                BasicSaveWork basicSaveWork = new BasicSaveWork();
                string? sourcePath = "";
                while (sourcePath == "" || sourcePath == null || !basicSaveWork.pathExists(sourcePath))
                {
                    Console.WriteLine("Veuillez saisir chemin d'accès existant à sauvegarder : ");
                    sourcePath = Console.ReadLine();
                }
                string? targetPath = "";
                while (targetPath == "" || targetPath == null)
                {
                    Console.WriteLine("Veuillez saisir un chemin d'accès existant sur lequel sauvegarder : ");
                    targetPath = Console.ReadLine();
                }
                basicSaveWork.setSourcePath(sourcePath);
                basicSaveWork.setTargetPath(targetPath);
                BasicSaveWorkViewModel viewModel = new BasicSaveWorkViewModel(basicSaveWork);
                Console.WriteLine(viewModel.initBasicSaveWork());
            }
            Console.WriteLine("Mode non implémenté pour le moment");
        }

        public void initSettings()
        {
            if (selectedMode == mode.Console)
            {
                string? choice = "";
                while (choice != "1" && choice != "2")
                {
                    Console.WriteLine("[1] "+language.GetString("settings_choice_language"));
                    Console.WriteLine("[2] "+language.GetString("setting_choice_config_path"));
                    choice = Console.ReadLine();
                }
                if(choice == "1")
                {
                    initLanguageSelection();
                }
                else if(choice == "2") 
                {

                }
            }
        }

        public void initLanguageSelection()
        {
            Array allLanguages = Enum.GetNames(typeof(Language));
            for (int i = 0; i<allLanguages.Length;i++)
            {
                Console.WriteLine("["+(i+1)+"] "+allLanguages.GetValue(i)!.ToString()+" "+(allLanguages.GetValue(i)!.ToString() == Instance.getLanguage().ToString() ? " ["+language.GetString("language_current")+"]" : ""));
            }
            int s = 0;
            while (s > allLanguages.Length || s < 1)
            {
                try
                {
                    Console.WriteLine(language.GetString("language_ask_choice"));
                    s = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.WriteLine(language.GetString("slotselection_error_invalid_number"));
                }
            }
            LanguageHandlerViewModel languageHandlerViewModel = new LanguageHandlerViewModel();
            languageHandlerViewModel.setLanguage((Language)(s-1));
            languageHandlerViewModel.initLanguageSelection().Print();
            mainMenu();
        }
    }
}