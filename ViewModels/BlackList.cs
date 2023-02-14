using easysave.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace easysave.ViewModels
{
    public class MainViewModel
    {
        private readonly Blacklist _blacklist;

        public MainViewModel()
        {
            _blacklist = new Blacklist();
            _blacklist.AddProcessName("Calculator");
            // Ajoutez les autres noms de processus à surveiller ici...
        }

        public void StartProcessMonitor()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var runningProcesses = Process.GetProcesses();
                    foreach (var process in runningProcesses)
                    {
                        if (_blacklist.ContainsProcess(process.ProcessName))
                        {
                            // Le processus est dans la liste noire, vous pouvez effectuer l'action appropriée ici...
                            Console.WriteLine("BlackList detecté");
                            break;
                        }
                    }
                    Console.WriteLine("New thread");
                    Thread.Sleep(5000); // Attendez 5 secondes avant de vérifier à nouveau les processus en cours d'exécution
                }
            });
        }
    }


}
