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

namespace easysave.Views
{
    public partial class MainWindow : Window
    {
        public ResourceManager language;
        private Socket serverSocket;
        private Thread serverThread;

        public MainWindow()
        {
            InitializeComponent();
            this.language = Instance.rm;
            translateAllItems();
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, 12345));
            serverThread = new Thread(new ThreadStart(Listen));
            serverThread.Start();
        }

        private void Listen()
        {
            serverSocket.Listen(5);
            while (true)
            {
                Socket clientSocket = serverSocket.Accept();
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
                clientThread.Start(clientSocket);
            }
        }

        private void HandleClient(object client)
        {
            Socket clientSocket = (Socket)client;
            byte[] buffer = new byte[1024];
            int bytesReceived = 0;
            while (true)
            {
                try
                {
                    bytesReceived = clientSocket.Receive(buffer);
                    if (bytesReceived == 0)
                    {
                        break;
                    }
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
                    //Dispatcher.Invoke(() => messagesList.Items.Add(message));
                    Dispatcher.Invoke(() => Console.WriteLine(message));
                }
                catch
                {
                    break;
                }
            }
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }


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