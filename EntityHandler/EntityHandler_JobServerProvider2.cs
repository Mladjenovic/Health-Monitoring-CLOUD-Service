using Common;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EntityHandler
{
    public class EntityHandler_JobServerProvider2 : IInstantServiceChecker
    {
        //public static List<EndpointAddress> RetreiveAllInstances()
        //{
        //    string internalEndpointName = "InternalRequest";
        //    List<EndpointAddress> internalEndpoints = new List<EndpointAddress>();
        //    foreach (RoleInstance instance in RoleEnvironment.Roles["EntityHandler"].Instances)
        //    {
        //        internalEndpoints.Add(new EndpointAddress($"net.tcp://{instance.InstanceEndpoints[internalEndpointName].IPEndpoint.ToString()}/{internalEndpointName}"));
        //    }

        //    return internalEndpoints;
        //}
        public string CheckIfInstranceIsAlive()
        {
            return RoleEnvironment.CurrentRoleInstance.Id;

            //List<EndpointAddress> internalEndpoints = RetreiveAllInstances();
            //IEntityHandler proxy = new ChannelFactory<IEntityHandler>(new NetTcpBinding(), internalEndpoints[0]).CreateChannel();
            //bool isAlive = proxy.IsAlive();
        }
    }
}
