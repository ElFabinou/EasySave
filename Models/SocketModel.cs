using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using easysave.Objects;
using easysave.ViewModels;
using easysave.Views;

namespace easysave.Models
{
    public class SocketModel
    {
        private string ipAdress = "127.0.0.1";
        private int port = 12345;

        //public SocketModel() {


        //    Thread socketServerThread = new Thread(() =>
        //    {
        //        Socket server = Initialize();
        //        server = AcceptConnexion(server);
        //        EcouteReseau(server);
        //    });
        //    socketServerThread.Start();

        //}

        private static SocketModel instance;
        private static readonly object lockObject = new object();
        private Thread socketServerThread;
        private Socket server;
        public SocketModel()
        {
            socketServerThread = new Thread(() =>
            {
                server = Initialize();
                server = AcceptConnexion(server);
                EcouteReseau(server);
            });
            socketServerThread.Start();
        }

        public static SocketModel GetInstance()
        {
            lock (lockObject)
            {
                if (instance == null)
                {
                    instance = new SocketModel();
                }
                return instance;
            }
        }

        public Socket GetServer()
        {
            if (socketServerThread.IsAlive)
            {
                return server;
            }
            else
            {
                socketServerThread = new Thread(() =>
                {
                    server = Initialize();
                    server = AcceptConnexion(server);
                    EcouteReseau(server);
                });
                socketServerThread.Start();
                return server;
            }
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


        public void EcouteReseau(Socket serv)
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
                    if (messageRead.StartsWith("/start "))
                    {
                        var saveWorkName = messageRead.Split(' ').Skip(1).FirstOrDefault();
                        RegisteredSaveViewModel registeredSaveViewModel = new RegisteredSaveViewModel();
                        List<RegisteredSaveWork> registeredSaveViewModelList = registeredSaveViewModel.initSlotSelection();
                        foreach (RegisteredSaveWork registeredSaveWork in registeredSaveViewModelList)
                        {
                            if(registeredSaveWork.getSaveName() == saveWorkName)
                            {
                                registeredSaveViewModel.setSaveWork(registeredSaveWork);
                                registeredSaveViewModel.initRegisteredSaveWork();
                            }
                        }
                    }else if(messageRead.StartsWith("/pause "))
                    {
                        var saveWorkName = messageRead.Split(' ').Skip(1).FirstOrDefault();
                        RegisteredSaveViewModel registeredSaveViewModel = new RegisteredSaveViewModel();
                        List<RegisteredSaveWork> registeredSaveViewModelList = registeredSaveViewModel.initSlotSelection();
                        foreach (RegisteredSaveWork registeredSaveWork in registeredSaveViewModelList)
                        {
                            if (registeredSaveWork.getSaveName() == saveWorkName)
                            {
                                foreach(Loader loader in Loader.loaders)
                                {
                                    if(loader.getSaveModel().registeredSaveWork.saveName == registeredSaveWork.saveName)
                                    {
                                        loader.getSaveModel().Pause();
                                    }
                                }
                                EnvoyerMessage(serv, "Ce save work n'est pas en cours");
                            }
                        }
                        EnvoyerMessage(serv, "Ce save work n'existe pas");
                    }else if (messageRead.StartsWith("/stop "))
                    {
                        var saveWorkName = messageRead.Split(' ').Skip(1).FirstOrDefault();
                        RegisteredSaveViewModel registeredSaveViewModel = new RegisteredSaveViewModel();
                        List<RegisteredSaveWork> registeredSaveViewModelList = registeredSaveViewModel.initSlotSelection();
                        foreach (RegisteredSaveWork registeredSaveWork in registeredSaveViewModelList)
                        {
                            if (registeredSaveWork.getSaveName() == saveWorkName)
                            {
                                foreach (Loader loader in Loader.loaders)
                                {
                                    if (loader.getSaveModel().registeredSaveWork.saveName == registeredSaveWork.saveName)
                                    {
                                        loader.getSaveModel().Stop();
                                        loader.loadingViewGUI.Dispatcher.Invoke(() =>
                                        {
                                            loader.loadingViewGUI.closeAllowed = true;
                                            loader.loadingViewGUI.Close();
                                        });
                                    }
                                }
                                EnvoyerMessage(serv, "Ce save work n'est pas en cours");
                            }
                        }
                        EnvoyerMessage(serv, "Ce save work n'existe pas");
                    }
                }
            }

        }
    }
}
