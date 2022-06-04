using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using LibSocketServerGps;

namespace LibServerServiceThread
{
    public partial class Service1 : ServiceBase
    {
        TcpServer tcpServer;
        private static string ports = ConfigurationManager.AppSettings["Port"];
        public Service1()
        {
            InitializeComponent();
            if (!EventLog.SourceExists("MySocket"))
            {

                EventLog.CreateEventSource("MySocket", "MyNewLogSocket");
            }
            int pot = Convert.ToInt32(ports);
            tcpServer = new TcpServer(pot);
            eventLog1.Source = "MySocket";
            eventLog1.Log = "MyNewLogSocket";
        }

        protected override void OnStart(string[] args)
        {
            if (tcpServer == null)
            {
                eventLog1.WriteEntry("No se  pudo iniciar el servicio");
            }
            else
            {
                tcpServer.Starting();
                eventLog1.WriteEntry("Inciando las Socket.......");
            }
        }

        protected override void OnStop()
        {
            if (tcpServer == null)
            {

                eventLog1.WriteEntry(" No se pudo Detener.........");
            }
            else
            {
                 tcpServer.Stoping();
                eventLog1.WriteEntry("Deteniendo.........");
            }
        }
    }
}
