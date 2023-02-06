using easysave.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easysave.Objects
{
    public class ReturnHandler
    {
        public string String;
        public enum ReturnTypeEnum
        {
            Success = ConsoleColor.Green,
            Error = ConsoleColor.Red,
        }

        public ReturnTypeEnum ReturnType;

        public ReturnHandler(string String, ReturnTypeEnum returnType) {
            this.String = String;
            this.ReturnType = returnType;
            LoggerHandler loggerHandler = new LoggerHandler(DateTime.Now, returnType.ToString(), String);
            LoggerHandlerModel loggerModel = new LoggerHandlerModel(loggerHandler);
            loggerModel.writeLog();

        }

        public void Print()
        {
            Console.Clear();
            Console.ForegroundColor = (ConsoleColor)ReturnType;
            Console.WriteLine(String);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
