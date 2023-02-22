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

namespace easysave.Views
{
    public partial class MainWindow : Window
    {
        public ResourceManager language;
        private string ipAdress = "127.0.0.1";
        private int port = 12345;

        public MainWindow()
        {
            InitializeComponent();
            this.language = Instance.rm;
            translateAllItems();


            Thread socketServerThread = new Thread(() => {
                Socket server = Initialize();
                server = AcceptConnexion(server);

                EcouteRéseau(server);
            });
            socketServerThread.Start();

            
;
        }

        public Socket Initialize()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress iPAddress = IPAddress.Parse(ipAdress);

            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, port);
            socket.Bind(iPEndPoint);

            socket.Listen(1);
            Console.WriteLine($"Serveur en écoute sur {iPEndPoint}");

            return socket;
        }

        public Socket AcceptConnexion(Socket socketServ)
        {
            Socket socketClient = socketServ.Accept();
            Console.WriteLine($"Nouvelle connexion : {socketClient.RemoteEndPoint.ToString()}");

            return socketClient;
        }


        public void EnvoyerMessage(Socket serv, string message)
        {
            string messageReponse = message;
            byte[] bufferReponse = Encoding.ASCII.GetBytes(messageReponse);
            serv.Send(bufferReponse);
        }


        public void EcouteRéseau(Socket serv)
        {
            int bytesRead;
            while (true)
            {
                byte[] buffer = new byte[8192];
                bytesRead = serv.Receive(buffer);
                if (bytesRead > 0)
                {
                    string messageRead = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.WriteLine(messageRead);
                    EnvoyerMessage(serv, messageRead);
                }
            }

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