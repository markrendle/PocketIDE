using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace PocketIDE.Web.Data
{
    public class ProcessMsdnContentQueue
    {
        private static readonly Action<string> EnqueueMethod = Enqueue;

        public static void EnqueueAsync(string msdnUrl)
        {
            EnqueueMethod.BeginInvoke(msdnUrl, EnqueueMethod.EndInvoke, null);
        }

        public static void Enqueue(string msdnUrl)
        {
            CloudStorageHelper.CreateQueueClient().GetQueueReference("process-msdn-content").AddMessage(new CloudQueueMessage(msdnUrl));
        }
    }
}