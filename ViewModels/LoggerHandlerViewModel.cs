using easysave.Models;
using easysave.Objects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static easysave.Objects.LanguageHandler;

namespace easysave.ViewModels
{
    internal class LoggerHandlerViewModel
    {

        public LanguageHandler.Language language;

        public ReturnHandler setLoggerExtension(string extension)
        {
            LoggerHandlerModel loggerHandlerModel = new LoggerHandlerModel(new LoggerHandler());
            return loggerHandlerModel.setLoggerExtension(extension);
        }
    }
}