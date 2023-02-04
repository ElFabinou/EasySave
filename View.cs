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
                Console.ForegroundColor= ConsoleColor.Yellow;
                Console.WriteLine("Menu principal");
                Console.ForegroundColor= ConsoleColor.Gray;
                string? choice = "";
                int attempt = 0;
                while (choice != "1" && choice != "2" && choice != "3" && choice != "4")
                {
                    Console.WriteLine("[1] Sauvegarde basique\t");
                    Console.WriteLine("[2] Liste des travaux de sauvegarde\t");
                    Console.WriteLine("[3] Créer un travail de sauvegarde\t");
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
                string? type = "";
                while (type == "" || type == null)
                {
                    Console.WriteLine("Veuillez saisir un type de sauvegarde : ");
                    Console.WriteLine("[1] Complet\t");
                    Console.WriteLine("[2] Differentiel\t");
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
                    Console.WriteLine("\tAucun slot de travail trouvé dans les sauvegardes, retour en arrière.");
                    mainMenu(); 
                }
                foreach (RegisteredSaveWork registeredSaveWork in registeredSaveViewModelList)
                {
                    Console.WriteLine("["+i+"] "+registeredSaveWork.getSaveName()+
                        "\n ├── Type : "+registeredSaveWork.getType().ToString() +
                        "\n ├── Source : "+registeredSaveWork.getSourcePath() +
                        "\n └── Cible : "+registeredSaveWork.getTargetPath());
                    i++;
                }
                int s = 0;
                while (s > registeredSaveViewModelList.Count || s < 1)
                {
                    try {
                        Console.WriteLine("Choisissez un travail de sauvegarde valable :");
                        s = Convert.ToInt32(Console.ReadLine());
                    }catch(Exception)
                    {
                        Console.WriteLine("Veuillez saisir un nombre.");
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
                    Console.WriteLine("Interaction avec le slot de sauvegarde : ");
                    Console.WriteLine(registeredSaveWork.getSaveName()+
                           "\n ├── Type : "+registeredSaveWork.getType().ToString() +
                           "\n ├── Source : "+registeredSaveWork.getSourcePath() +
                           "\n └── Cible : "+registeredSaveWork.getTargetPath());
                    Console.WriteLine("[1] Lancer cette sauvegarde\t");
                    Console.WriteLine("[2] Supprimer cette sauvegarde\t");
                    Console.WriteLine("[x] Modifier cette sauvegarde\t");
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
    
    }
}