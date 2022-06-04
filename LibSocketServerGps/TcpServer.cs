using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LibSocketServerGps
{
    public class TcpServer
    {
        private TcpListener _server;
        private Boolean _isRunning;
        private int port;
        private Thread thrServicio;

        public TcpServer(int port)
        {
            this.port = port;
            _isRunning = false;
        }
        public void Starting()
        {
            //_server = new TcpListener(IPAddress.Any, port);
            //_server.Start();

            //_isRunning = true;

            //LoopClients();
            if (!_isRunning)
            {
                try
                {
                    _isRunning = true;

                    if (_server == null)
                    {
                        _server = new TcpListener(IPAddress.Any, port);
                    }

                    _server.Start();


                    thrServicio = new Thread(new ThreadStart(InServicio));
                    thrServicio.IsBackground = true;
                    thrServicio.Start();
                }
                catch (Exception ex)
                {

                    throw new Exception("error: " + ex);
                }
            }

        }
        public void InServicio()
        {
            while (_isRunning)
            {
                Thread.Sleep(100);
                try
                {
                    TcpClient newClient = _server.AcceptTcpClient();

                    if (newClient != null)
                    {
                        var ip = ((IPEndPoint)(newClient.Client.RemoteEndPoint)).Address.ToString();

                        // client found.
                        // create a thread to handle communication
                        //Thread t = new Thread(new ParameterizedThreadStart(HandleClient));

                        // t.Start(newClient);
                        // Console.WriteLine("Cliente conectado :" + ip);
                        Console.WriteLine("Cliente conectado :" + ip);
                        ThreadListener threadListener = new ThreadListener(newClient);
                        threadListener.Iniciar();
                    }
                }
                catch (SocketException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        //private void LoopClients()
        //{
        //    while (_isRunning)
        //    {
        //        // wait for client connection
        //        TcpClient newClient = _server.AcceptTcpClient();

        //        var ip = ((IPEndPoint)(newClient.Client.RemoteEndPoint)).Address.ToString();

        //        // client found.
        //        // create a thread to handle communication
        //        //Thread t = new Thread(new ParameterizedThreadStart(HandleClient));

        //        // t.Start(newClient);
        //        // Console.WriteLine("Cliente conectado :" + ip);
        //        Console.WriteLine("Cliente conectado :" + ip);
        //        ThreadListener threadListener = new ThreadListener(newClient);
        //        threadListener.Iniciar();
        //    }
        //}
        public void Stoping()
        {
            if (_isRunning)
            {
                try
                {
                    _isRunning = false;
                    _server.Stop();
                    if (thrServicio.IsAlive)
                    {
                        thrServicio.Abort();
                    }

                    // Environment.Exit(0);
                }
                catch (SocketException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
