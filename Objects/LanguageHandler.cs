using System;
using System.Configuration;
using System.Reflection;
using System.Resources;

namespace easysave.Objects
{
    internal sealed class LanguageHandler
    {
        private static readonly LanguageHandler instance = new LanguageHandler();

        private LanguageHandler() { }

        public static LanguageHandler Instance
        {
            get { return instance; }
        }

        public ResourceManager rm = new ResourceManager("easysave.Ressources."  +ConfigurationManager.AppSettings["language"]!.ToString(), Assembly.GetExecutingAssembly());

        public enum Language
        {
            fr_FR,
            en_GB
        }
        public Language language = (Language)Enum.Parse(typeof(Language), ConfigurationManager.AppSettings["language"]!.ToString());

        public Language getLanguage()
        {
            return language;
        }

        public void setLanguage(Language language)
        {
            this.language = language;
            rm = new ResourceManager("easysave.Ressources." + this.language.ToString(), Assembly.GetExecutingAssembly());
        }
    }
}