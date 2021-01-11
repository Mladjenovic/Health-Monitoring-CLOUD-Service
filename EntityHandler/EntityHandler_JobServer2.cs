using Common;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EntityHandler
{
    public class EntityHandler_JobServer2
    {
        private ServiceHost serviceHost;
        // dodati endpoint sa ovim imenom u ServiceDefinition
        private String endpointName = "InputRequest";
        public EntityHandler_JobServer2()
        {
            RoleInstanceEndpoint inputEndPoint = RoleEnvironment.
            CurrentRoleInstance.InstanceEndpoints[endpointName];
            string endpoint = String.Format("net.tcp://{0}/{1}", inputEndPoint.IPEndpoint, endpointName);
            serviceHost = new ServiceHost(typeof(EntityHandler_JobServerProvider2));
            NetTcpBinding binding = new NetTcpBinding();
            serviceHost.AddServiceEndpoint(typeof(IInstantServiceChecker), binding, endpoint);
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
                //Trace.TraceInformation(String.Format("Host for {0} endpoint type closed successfully at {1}", endpointName, DateTime.Now));
            }
            catch (Exception e)
            {
                //Trace.TraceInformation("Host close error for {0} endpoint type. Error  message is: {1}. ", endpointName, e.Message);
            }
        }
    }
}
