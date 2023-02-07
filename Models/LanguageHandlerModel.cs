using easysave.Objects;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace easysave.Models
{
    internal class LanguageHandlerModel
    {
        public LanguageHandlerModel(LanguageHandler.Language language) {
            this.language = language;
        }

        public LanguageHandler.Language language;

        public ReturnHandler changeLanguage()
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            settings["language"].Value = language.ToString();
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            LanguageHandler languageHandler = LanguageHandler.Instance;
            languageHandler.setLanguage(language);
            return new ReturnHandler(languageHandler.rm.GetString(""), ReturnHandler.ReturnTypeEnum.Success);
        }
    }
}
