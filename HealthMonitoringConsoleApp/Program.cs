using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HealthMonitoringConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleServer server = new ConsoleServer();
            server.Open();

            //Samostalno propitivanje da li je instanca ziva, iz konzolne aplikacije..\

            

            Console.WriteLine("Checking if instances are alive...");

            while (true)
            {
                var binding = new NetTcpBinding();
                ChannelFactory<IInstantServiceChecker> factory = new ChannelFactory<IInstantServiceChecker>(binding, new
                EndpointAddress("net.tcp://localhost:16000/InputRequest"));

                IInstantServiceChecker proxy = factory.CreateChannel();
                string isAlive = proxy.CheckIfInstranceIsAlive();
                Console.WriteLine($"Entity handler isntance {isAlive} is Alive");
                Thread.Sleep(1000);
            }

            //Console.ReadKey();
        }
    }
}
