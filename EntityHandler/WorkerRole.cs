using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;

namespace EntityHandler
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        EntityHandler_JobServer ehjs = new EntityHandler_JobServer();
        EntityHandler_JobServer2 ehjs2 = new EntityHandler_JobServer2();
        public override void Run()
        {
            Trace.TraceInformation("EntityHandler is running");

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

            ehjs.Open();
            ehjs2.Open();
            bool result = base.OnStart();

            Trace.TraceInformation("EntityHandler has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("EntityHandler is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            ehjs2.Close();
            ehjs.Close();
            base.OnStop();

            Trace.TraceInformation("EntityHandler has stopped");
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
