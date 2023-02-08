using System.Resources;
using static easysave.Objects.LanguageHandler;
using System.Configuration;

namespace easysave.Views
{
    internal class MainView
    {
        public enum mode
        {
            Console,
            GUI
        };

        public mode selectedMode;

        public MainView(mode selectedMode = mode.Console)
        {
            this.selectedMode = selectedMode;
        }

        public ResourceManager language;

        public void mainMenu()
        {
            if (this.selectedMode == mode.Console)
            {
                this.language = Instance.rm;
                SaveWorkView saveWorkView = new SaveWorkView(this.selectedMode);
                SettingsView settingsView = new SettingsView(this.selectedMode);
                Console.WriteLine("  ______                 _____                    _____ _      _____ \r\n |  ____|               / ____|                  / ____| |    |_   _|\r\n | |__   __ _ ___ _   _| (___   __ ___   _____  | |    | |      | |  \r\n |  __| / _` / __| | | |\\___ \\ / _` \\ \\ / / _ \\ | |    | |      | |  \r\n | |___| (_| \\__ \\ |_| |____) | (_| |\\ V /  __/ | |____| |____ _| |_ \r\n |______\\__,_|___/\\__, |_____/ \\__,_| \\_/ \\___|  \\_____|______|_____|\r\n                   __/ |                                             \r\n                  |___/                                              ");
                Console.WriteLine("\t\t\t\t\t\t\t\t"+ConfigurationManager.AppSettings["version"]!.ToString());
                Console.WriteLine("\n");
                Console.ForegroundColor= ConsoleColor.Yellow;
                Console.WriteLine(language.GetString("menu_title"));
                Console.ForegroundColor= ConsoleColor.Gray;
                string? choice = "";
                int attempt = 0;
                while (choice != "1" && choice != "2" && choice != "3" && choice != "4" && choice != "5")
                {
                    Console.WriteLine("[1] "+language.GetString("menu_save_all"));
                    Console.WriteLine("[2] "+language.GetString("menu_work_list"));
                    Console.WriteLine("[3] "+language.GetString("menu_new_save_work"));
                    Console.WriteLine("[4] "+language.GetString("menu_settings"));
                    Console.WriteLine("[5] "+language.GetString("menu_exit"));

                    choice = Console.ReadLine();
                    attempt++;
                }

                switch (choice)
                {
                    case "1":
                        saveWorkView.initSequentialSave();
                        break;
                    case "2":
                        saveWorkView.initSlotSelection();
                        break;
                    case "3":
                        saveWorkView.initSlotCreation();
                        break;
                    case "4":
                        settingsView.initSettings();
                        break;
                    case "5":
                        Environment.Exit(0);
                        break;
                }

            }
            else
            {
                Console.WriteLine(language.GetString("menu_error_load"));
            }
        }
    }
}
