using Common;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitoringConsoleApp
{
    public class ConsoleServer
    {
        private ServiceHost serviceHost;
        // dodati endpoint sa ovim imenom u ServiceDefinition
        private String endpointName = "Console";
        public ConsoleServer()
        {
            string endpoint = String.Format("net.tcp://localhost:15000/{0}", endpointName);
            serviceHost = new ServiceHost(typeof(ConsoleServerProvider));
            NetTcpBinding binding = new NetTcpBinding();
            serviceHost.AddServiceEndpoint(typeof(IConsole), binding, endpoint);
        }
        public void Open()
        {
            try
            {
                serviceHost.Open();
                //Trace.TraceInformation(String.Format("Host for {0} endpoint type opened successfully at {1}", endpointName, DateTime.Now));
            }
            catch (Exception e)
            {
                //Trace.TraceInformation("Host open error for {0} endpoint type. Error message is: {1}. ", endpointName, e.Message);
            }
        }
        public void Close()
        {
            try
            {
                serviceHost.Close();
               // Trace.TraceInformation(String.Format("Host for {0} endpoint type closed successfully at {1}", endpointName, DateTime.Now));
            }
            catch (Exception e)
            {
                //Trace.TraceInformation("Host close error for {0} endpoint type. Error  message is: {1}. ", endpointName, e.Message);
            }
        }
    }
}
