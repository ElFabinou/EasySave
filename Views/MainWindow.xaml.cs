using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Resources;
using static easysave.Objects.LanguageHandler;
using System.Windows.Shapes;
using System.Resources;
using static easysave.Objects.LanguageHandler;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Diagnostics;
using Newtonsoft.Json;
using easysave.ViewModels;
using System.Diagnostics.Metrics;

namespace easysave.Views
{
    public partial class MainWindow : Window
    {
        public ResourceManager language;
        private const string UniqueName = "EasySave";
        private static Mutex mutex;

        public MainWindow()
        {
            mutex = new Mutex(true, UniqueName, out bool createdNew);//Si un mutex a le même nom = false

            //Verif le true ou false du mutex
            if (!createdNew)//Si le Mutex existe déjà, la valeur de createdNew sera false
            {
                MessageBox.Show("L'application est déjà en cours d'exécution", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                mutex.Dispose();
                Application.Current.Shutdown();
                return;
            }
            InitializeComponent();
            this.language = Instance.rm;
            translateAllItems();

            SocketViewModel socketViewModel = new SocketViewModel();
        }

        //Méthode appelé automatiquement quand l'utilisateur ferme l'application
        protected override void OnClosed(EventArgs e)
        {
            mutex?.Dispose();//Si le mutex n'est pas null, on le libère (pour éviter l'erreur s'il est null)
            base.OnClosed(e);//Call la méthode de la class base. Permet de bien gérer la fermeture
        }

        //public void ConnexionChannel(Socket serv)
        //{
        //    byte[] buffer = new byte[8192];
        //    int bytesRead;
        //    try
        //    {
        //        bytesRead = serv.Receive(buffer);
        //        if (bytesRead > 0)
        //        {
        //            string messageRead = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        //            Console.WriteLine("Client : " + messageRead);
        //            string msgtosend = "Hello";
        //            byte[] bufferReponse = Encoding.ASCII.GetBytes(msgtosend);
        //            serv.Send(bufferReponse);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return;
        //    }
        //}

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void initSaveWork_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Content = new SaveWorkListViewGUI();
        }

        private void initSlotCreation_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Content = new CreateSaveWorkViewGUI();
        }

        private void initSettings_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Content = new SettingsViewGUI();
        }

        private void initLeave_Click(object sender, RoutedEventArgs e)
        {
            //Application.Current.Shutdown();
            Environment.Exit(0);

        }

        private void MainMenu_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void translateAllItems()
        {
            initSaveWork.Content = language.GetString("gui_MainWindow_initSaveWork");
            initSettings.Content = language.GetString("gui_MainWindow_initSettings");
            initLeave.Content = language.GetString("gui_MainWindow_initLeave");
            initSlotCreation.Content = language.GetString("gui_MainWindow_initSlotCreation");
        }
    }
}