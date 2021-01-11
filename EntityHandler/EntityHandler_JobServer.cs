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
    public class EntityHandler_JobServer
    {
        private ServiceHost serviceHost;
        // dodati endpoint sa ovim imenom u ServiceDefinition
        private String externalEndpointName = "InternalRequest";
        public EntityHandler_JobServer()
        {
            RoleInstanceEndpoint inputEndPoint = RoleEnvironment.
            CurrentRoleInstance.InstanceEndpoints[externalEndpointName];
            string endpoint = String.Format("net.tcp://{0}/{1}", inputEndPoint.IPEndpoint,
            externalEndpointName);
            serviceHost = new ServiceHost(typeof(EntityHandler_JobServerProvider));
            NetTcpBinding binding = new NetTcpBinding();
            serviceHost.AddServiceEndpoint(typeof(IEntityHandler), binding, endpoint);
        }
        public void Open()
        {
            try
            {
                serviceHost.Open();
                Trace.TraceInformation(String.Format("Host for {0} endpoint type openedsuccessfully at {1}", externalEndpointName, DateTime.Now));
            }
            catch (Exception e)
            {
                Trace.TraceInformation("Host open error for {0} endpoint type. Errormessage is: {1}. ", externalEndpointName, e.Message);
            }
        }
        public void Close()
        {
            try
            {
                serviceHost.Close();
                Trace.TraceInformation(String.Format("Host for {0} endpoint type closedsuccessfully at {1}", externalEndpointName, DateTime.Now));
            }
            catch (Exception e)
            {
                Trace.TraceInformation("Host close error for {0} endpoint type. Errormessage is: {1}. ", externalEndpointName, e.Message);
            }
        }
    }
}
