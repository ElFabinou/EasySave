using easysave.Objects;
using easysave.ViewModels;

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

        public void mainMenu()
        {
            if (this.selectedMode == mode.Console)
            {
                Console.WriteLine("  ______                 _____                    _____ _      _____ \r\n |  ____|               / ____|                  / ____| |    |_   _|\r\n | |__   __ _ ___ _   _| (___   __ ___   _____  | |    | |      | |  \r\n |  __| / _` / __| | | |\\___ \\ / _` \\ \\ / / _ \\ | |    | |      | |  \r\n | |___| (_| \\__ \\ |_| |____) | (_| |\\ V /  __/ | |____| |____ _| |_ \r\n |______\\__,_|___/\\__, |_____/ \\__,_| \\_/ \\___|  \\_____|______|_____|\r\n                   __/ |                                             \r\n                  |___/                                              \r\n\r\n");
                Console.WriteLine("\n");
                string? choice = "";
                int attempt = 0;
                while (choice != "1" && choice != "2" && choice != "3" && choice != "4")
                {
                    Console.WriteLine("[1] Sauvegarde basique\t");
                    Console.WriteLine("[2] Travail de sauvegarde\t");
                    Console.WriteLine("[3] Slots de travail de sauvegarde\t");
                    Console.WriteLine("[x] Paramètres\t");
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
                        initSlotModification();
                        break;
                }
            }
            else
            {
                Console.WriteLine("Erreur, impossible de charger l'interface.");
            }
        }

        private void initSlotModification()
        {
            if (this.selectedMode == mode.Console)
            {
                RegisteredSaveWork registeredSaveWork = new RegisteredSaveWork();
                string? name = "";
                while (name == "" || name == null)
                {
                    Console.WriteLine("Veuillez saisir le nom du travail de sauvegarde (notez que le nom du dossier qui sera créé contiendra automatiquement la date et heure de la sauvegarde) : ");
                    name = Console.ReadLine();
                }
                string? sourcePath = "";
                while (sourcePath == "" || sourcePath == null || !registeredSaveWork.pathExists(sourcePath))
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
                registeredSaveWork.setSaveName(name);
                registeredSaveWork.setSourcePath(sourcePath);
                registeredSaveWork.setTargetPath(targetPath);
                RegisteredSaveViewModel viewModel = new RegisteredSaveViewModel(registeredSaveWork);
                Console.WriteLine(viewModel.initSlotCreation());

            }
        }

        public void initSlotSelection()
        {
            if (this.selectedMode == mode.Console)
            {
                RegisteredSaveViewModel registeredSaveViewModel = new RegisteredSaveViewModel();
                List<RegisteredSaveWork> registeredSaveViewModelList = registeredSaveViewModel.initSlotSelection();
                Console.WriteLine("Choissez un slot de sauvegarde à éditer : ");
                int i = 1;
                foreach (RegisteredSaveWork registeredSaveWork in registeredSaveViewModelList)
                {
                    Console.WriteLine("["+i+"] "+registeredSaveWork.getSaveName());
                    i++;
                }
                int s = 0;
                while (s > registeredSaveViewModelList.Count || s < 1)
                {
                    try {
                        s = Convert.ToInt32(Console.Read());
                    }catch(Exception e)
                    {
                        Console.WriteLine("Vous n'avez pas saisi un nombre.");
                    }
                }

            }
        }

        public void initBasicSaveWork()
        {
            if (this.selectedMode == mode.Console)
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
    
    }
}