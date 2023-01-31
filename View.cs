using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                while (choice != "O" && choice != "N")
                {
                    Console.WriteLine("Créer une nouvelle sauvegarde ? (O/N) " + (attempt > 0 ? "Tapez O pour Oui ou N pour Non." : ""));
                    choice = Console.ReadLine();
                    attempt++;
                }
                if(choice == "O")
                {
                    initSaveWork();
                }
                else
                {
                    Console.WriteLine("Terminé");
                }
            }
            else
            {
                Console.WriteLine("Erreur, impossible de charger l'interface.");
            }
        }
    
        public void initSaveWork()
        {
            SaveWork saveWork = new SaveWork();
            string ?sourcePath = "";
            while(sourcePath == "" || sourcePath == null || !saveWork.pathExists(sourcePath)) {
                Console.WriteLine("Veuillez saisir chemin d'accès existant à sauvegarder : ");
                sourcePath = Console.ReadLine();
            }
            string? targetPath = "";
            while (targetPath == "" || targetPath == null || !saveWork.pathExists(targetPath))
            {
                Console.WriteLine("Veuillez saisir un chemin d'accès existant sur lequel sauvegarder : ");
                targetPath = Console.ReadLine();
            }
            saveWork.setSourcePath(sourcePath);
            saveWork.setTargetPath(targetPath);
        }
    
    }
}
