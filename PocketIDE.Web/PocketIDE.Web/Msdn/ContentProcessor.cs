using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;

namespace PocketIDE.Web.Msdn
{
    public class ContentProcessor
    {
        public static readonly ContentProcessor Instance = new ContentProcessor();

        private readonly ConcurrentQueue<string> _urlsToProcess = new ConcurrentQueue<string>();
        private readonly Timer _timer;

        private ContentProcessor()
        {
            _timer = new Timer(TimerCallback, null, 100, 100);
        }

        public void EnqueueUrl(string originalMsdnUrl)
        {
            _urlsToProcess.Enqueue(originalMsdnUrl);
        }

        private void TimerCallback(object state)
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);

            ProcessQueue();

            _timer.Change(100, 100);
        }

        private void ProcessQueue()
        {
            string originalMsdnUrl;
            while (_urlsToProcess.TryDequeue(out originalMsdnUrl))
            {
                var html = DownloadDocument(originalMsdnUrl);
                var processor = DocumentProcessor.Create(html);
                processor.RunAsync();
            }
        }

        private static string DownloadDocument(string originalMsdnUrl)
        {
            var lobandUrl = originalMsdnUrl.Replace(".aspx", "(loband).aspx");
            using (var client = new WebClient())
            {
                return client.DownloadString(lobandUrl);
            }
            
        }
    }
}