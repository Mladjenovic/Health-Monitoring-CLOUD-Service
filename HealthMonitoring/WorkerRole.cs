using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using CloudService_Data;
using Common;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace HealthMonitoring
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        public static List<EndpointAddress> RetreiveAllInstances()
        {
            string internalEndpointName = "InternalRequest";
            List<EndpointAddress> internalEndpoints = new List<EndpointAddress>();
            foreach (RoleInstance instance in RoleEnvironment.Roles["EntityHandler"].Instances)
            {
                internalEndpoints.Add(new EndpointAddress($"net.tcp://{instance.InstanceEndpoints[internalEndpointName].IPEndpoint.ToString()}/{internalEndpointName}"));
            }

            return internalEndpoints;
        }

        public override void Run()
        {
            CloudQueue queue = QueueHelper.GetQueueReference("healthmonitoring");

            Trace.TraceInformation("Health service checking method called!");


            while (true)
            {
                List<EndpointAddress> internalEndpoints = RetreiveAllInstances();
                IEntityHandler proxy = new ChannelFactory<IEntityHandler>(new NetTcpBinding(), internalEndpoints[0]).CreateChannel();
                bool isAlive = proxy.IsAlive();
                if (isAlive)
                {
                    queue.AddMessage(new CloudQueueMessage($"{RoleEnvironment.Roles["EntityHandler"].Instances[0].Id} - Is Alive"));
                }
                Thread.Sleep(4000);
            }

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("HealthMonitoring has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("HealthMonitoring is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("HealthMonitoring has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                //Trace.TraceInformation("Working");
                await Task.Delay(1000);
            }
        }
    }
}
