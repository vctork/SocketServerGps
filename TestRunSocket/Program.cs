using LibSocketServerGps;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRunSocket
{
    class Program
    {
        static void Main(string[] args)
        {
            string ips = ConfigurationManager.AppSettings["Port"];
            int ip = Convert.ToInt32(ips);
            TcpServer tcpServer = new TcpServer(ip);
            tcpServer.Starting();
        }
    }
}
