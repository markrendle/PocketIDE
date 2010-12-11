using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;

namespace PocketIDE.Web.Msdn
{
    public class CssAggregator
    {
        private readonly ConcurrentBag<WebClient> _webClients = new ConcurrentBag<WebClient>();
        private int _running;
        private readonly ConcurrentBag<string> _parts = new ConcurrentBag<string>();

        public void AddAsync(string uri)
        {
            Interlocked.Increment(ref _running);
            Action action = () =>
                                {
                                    using (var webClient = new WebClient())
                                    {
                                        var css = webClient.DownloadString(uri);
                                        _parts.Add(css);
                                    }
                                };
            action.BeginInvoke(iar =>
                                   {
                                       action.EndInvoke(iar);
                                       Interlocked.Decrement(ref _running);
                                   }, null);
        }

        public event EventHandler AllCompleted;

        public string GetAggregatedCss()
        {
            while (_running > 0)
            {
                Thread.Sleep(1000);
            }

            return string.Join("", _parts);
        }

        void DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            _parts.Add(e.Result);
            Interlocked.Decrement(ref _running);
            if (_running == 0)
            {
                AllCompleted.Raise(this);
            }
            ((IDisposable)sender).Dispose();
        }
    }
}