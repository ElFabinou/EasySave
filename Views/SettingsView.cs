using easysave.ViewModels;
using System.Configuration;
using static easysave.Objects.LanguageHandler;

namespace easysave.Views
{
    internal class SettingsView : MainView
    {

        public SettingsView(mode selectedMode) {
            this.language = Instance.rm;
            this.selectedMode = selectedMode;
        }

        public void initSettings()
        {
            if (selectedMode == mode.Console)
            {
                string? choice = "";
                while (choice != "1" && choice != "2" && choice != "3")
                {
                    Console.WriteLine("[1] "+language.GetString("settings_choice_language"));
                    Console.WriteLine("[2] "+language.GetString("setting_choice_change_extension"));
                    Console.WriteLine("[3] " + language.GetString("settings_choice_back"));

                    choice = Console.ReadLine();
                }if (choice == "1")
                 {
                     initLanguageSelection();
                 }
                 else if (choice == "2")
                 {
                    initExtensionSelection();
                 }
                 else if (choice == "3")
                 {
                     mainMenu();
                 }
            }
        }

        public void initLanguageSelection()
        {
            Array allLanguages = Enum.GetNames(typeof(Language));
            for (int i = 0; i<allLanguages.Length; i++)
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

        public void initExtensionSelection()
        {
            Console.WriteLine(language.GetString("setting_ask_extension"));
            List<string> extensions = new List<string>();
            extensions.Add("json");
            extensions.Add("xml");
            Console.WriteLine("[1] .json" + (ConfigurationManager.AppSettings["configExtension"]!.ToString() == "json" ? " ["+language.GetString("extension_current")+"]" : ""));
            Console.WriteLine("[2] .xml" + (ConfigurationManager.AppSettings["configExtension"]!.ToString() == "xml" ? " ["+language.GetString("extension_current")+"]" : ""));

            string s = "";
            s = Console.ReadLine();
            while (s == "" || s == null)
            {
                Console.WriteLine(language.GetString("setting_ask_extension"));
                s = Console.ReadLine();
            }
            LoggerHandlerViewModel loggerHandlerViewModel = new LoggerHandlerViewModel();
            switch (s)
            {
                case "1":
                    loggerHandlerViewModel.setLoggerExtension("json").Print();
                    break;
                case "2":
                    loggerHandlerViewModel.setLoggerExtension("xml").Print();
                    break;
            }
            mainMenu();
        }
    }
}