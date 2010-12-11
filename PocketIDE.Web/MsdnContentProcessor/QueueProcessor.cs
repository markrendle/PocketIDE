using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.WindowsAzure.StorageClient;

namespace MsdnContentProcessor
{
    class QueueProcessor
    {
        private readonly CloudQueue _queue = CloudStorageHelper.CreateQueueClient().GetQueueReference("process-msdn-content");

        public void Start()
        {
            _queue.BeginGetMessages(8, GotMessages, null);
        }

        private void GotMessages(IAsyncResult asyncResult)
        {
            bool any = false;
            var messages = _queue.EndGetMessages(asyncResult);

            foreach (var message in messages)
            {
                new ContentProcessor(message.AsString).ProcessAsync();
            }

            Thread.Sleep(1000);
            Start();
        }
    }
}
