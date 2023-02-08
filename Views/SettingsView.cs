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
                    Console.WriteLine("[2] "+language.GetString("setting_choice_config_path"));
                    Console.WriteLine("[3] " + language.GetString("settings_choice_back"));

                    choice = Console.ReadLine();
                }

                 if (choice == "1")
                 {
                     initLanguageSelection();
                 }
                 else if (choice == "2")
                 {

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
    }
}
