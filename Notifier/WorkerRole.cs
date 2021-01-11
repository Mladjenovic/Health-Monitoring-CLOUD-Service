using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;
using CloudService_Data;
using Common;
using Microsoft.Azure;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Notifier
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        CloudQueue queue = QueueHelper.GetQueueReference("healthmonitoring");
        public override void Run()
        {

            while (true)
            {
                var message = queue.GetMessage();


                var storageAccount =
                    CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
                CloudBlobClient blobStorage = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container =
                blobStorage.GetContainerReference("blobkontejner");
                CloudBlockBlob blob = container.GetBlockBlobReference(DateTime.Now.ToString("dd-MM-yyyy"));
                string content = "";
                if (blob.Exists())
                {
                    content = blob.DownloadText() + "|";
                }
                content = RoleEnvironment.CurrentRoleInstance.Id + " ; Instance is Alive  " + ";" + DateTime.Now.ToString("dd-MM-yyyy") + ";" +
                 DateTime.Now.ToString("dd-MM-yyyy");
                blob.UploadText(content);

                try
                {
                    ChannelFactory<IConsole> factory = new ChannelFactory<IConsole>(new NetTcpBinding(), new
                        EndpointAddress("net.tcp://localhost:15000/Console"));

                    IConsole proxy = factory.CreateChannel();
                    proxy.PrintMessage(message.AsString);
                    

                    Trace.TraceInformation(message.AsString);
                }
                catch (Exception )
                {
                    //PASS
                }
                Thread.Sleep(4000);
            }
            //Trace.TraceInformation("Notifier is running");

            //try
            //{
            //    this.RunAsync(this.cancellationTokenSource.Token).Wait();
            //}
            //finally
            //{
            //    this.runCompleteEvent.Set();
            //}
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.


            try
            {
                // read account configuration settings
                var storageAccount =
                CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
                // create blob container 
                CloudBlobClient blobStorage = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobStorage.GetContainerReference("blobkontejner");
                container.CreateIfNotExists();
                // configure container for public access
                var permissions = container.GetPermissions();
                permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                container.SetPermissions(permissions);
            }
            catch (WebException)
            {
            }

            bool result = base.OnStart();

            Trace.TraceInformation("Notifier has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("Notifier is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("Notifier has stopped");
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
