using DATA;
using NEGOCIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LibSocketServerGps
{
    public class ThreadListener
    {
        TcpClient tcpClient;
        Thread thread;
        TramaComtroller tramaComtroller;
        MonTramasController MonTramas;
        public ThreadListener(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            tramaComtroller = new TramaComtroller();
            MonTramas = new MonTramasController();
        }
        public void Iniciar()
        {
            thread = new Thread(new ThreadStart(HandleClient))
            {
                IsBackground = true
            };
            thread.Start();
        }
        public void HandleClient()
        {
            // retrieve client from parameter passed to thread
            TcpClient client = (TcpClient)tcpClient;

            // sets two streams
            StreamWriter sWriter = new StreamWriter(client.GetStream(), Encoding.ASCII);
            StreamReader sReader = new StreamReader(client.GetStream(), Encoding.ASCII);
            // you could use the NetworkStream to read and write, 
            // but there is no forcing flush, even when requested

            Boolean bClientConnected = true;
            String sData = null;

            while (bClientConnected)
            {
                try
                {
                    // reads from stream
                    sData = sReader.ReadLine();
                    if (!String.IsNullOrEmpty(sData))
                    {
                        // shows content on the Console.
                        Console.WriteLine("Client &gt; " + sData);
                        Tr t = new Tr() { Trama = sData };
                        MonTrama cr = new MonTrama()
                        {
                            Data = sData.Trim(),
                            Estado = false,
                            Fecha = DateTime.Now
                        };
                        MonTramas.Save(cr);
                        if (tramaComtroller.addTrama(t))
                            Console.WriteLine("se ha guardo exitosamente");
                        // Console.WriteLine("se ha guardo exitosamente");
                        else
                            Console.WriteLine("no se pudo guardar");
                        // Console.WriteLine("no se pudo guardar");
                        // to write something back.
                        // sWriter.WriteLine("Meaningfull things here");
                        // sWriter.Flush();

                    }
                    else
                    {
                        bClientConnected = false;

                    }
                }
                catch (Exception ex)
                {
                    bClientConnected = false;
                    // Console.WriteLine("error Cliente" + ex.Message);
                    Console.WriteLine("error Cliente" + ex.Message);
                }


            }
            sWriter.Close();
            sReader.Close();
            if (client != null)
            {
                client.Close();
            }

            if (thread.IsAlive)
                thread.Abort();
        }
    }
}
