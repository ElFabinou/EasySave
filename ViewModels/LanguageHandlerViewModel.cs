using easysave.Models;
using easysave.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static easysave.Objects.LanguageHandler;

namespace easysave.ViewModels
{
    internal class LanguageHandlerViewModel
    {
        public LanguageHandler.Language language; 

        public ReturnHandler initLanguageSelection()
        {
            LanguageHandlerModel model = new LanguageHandlerModel(language);
            return model.changeLanguage();
        }

        public void setLanguage(Language language)
        {
            this.language = language;
        }


    }
}
