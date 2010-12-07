using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Xml.Linq;

namespace PocketIDE.Services
{
    public class Bing
    {
        private const string ApiAddress = "http://api.search.live.net/xml.aspx";
        private const string AppId = "2F635E676F1E8DCC2F15C0BB6D65BDE114703D29";
        private const string QueryStart = "site%3Amsdn.microsoft.com%20";
        private const string ApiUriStart = ApiAddress + "?Appid=" + AppId + "&sources=web&query=" + QueryStart;

        private static readonly XNamespace WebNs =
            XNamespace.Get("http://schemas.microsoft.com/LiveSearch/2008/04/XML/web");

        private readonly WebClient _webClient = new WebClient();

        public void SearchAsync(string searchTerms)
        {
            _webClient.DownloadStringCompleted += WebClientDownloadStringCompleted;
            _webClient.DownloadStringAsync(ConstructSearchUri(searchTerms));
        }

        public event EventHandler<BingSearchCompletedEventArgs> BingSearchCompleted;

        public void CancelAsync()
        {
            _webClient.CancelAsync();
        }

        void WebClientDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                return;
            }

            BingSearchCompleted.Raise(this, () => CreateEventArgs(e));
        }

        private static BingSearchCompletedEventArgs CreateEventArgs(DownloadStringCompletedEventArgs e)
        {
            var results = e.Error != null
                              ? ExceptionToSearchResults(e.Error)
                              : ParseResults(e.Result);

            return new BingSearchCompletedEventArgs(results);
        }

        private static IEnumerable<BingSearchResult> ParseResults(string text)
        {
            try
            {
                var xml = XDocument.Parse(text).Root;
                if (xml == null) return Enumerable.Empty<BingSearchResult>();

                return xml.MaybeElement(WebNs + "Web")
                    .Element(WebNs + "Results")
                    .Elements(WebNs + "WebResult")
                    .Select(e =>
                            new BingSearchResult(e.Element(WebNs + "Title").Value,
                                                 e.Element(WebNs + "Description").Value, e.Element(WebNs + "Url").Value));
            }
            catch (Exception ex)
            {
                return ExceptionToSearchResults(ex);
            }
        }

        static Uri ConstructSearchUri(string searchTerms)
        {
            return new Uri(ApiUriStart + Uri.EscapeDataString(searchTerms));
        }

        static IEnumerable<BingSearchResult> ExceptionToSearchResults(Exception ex)
        {
            yield return
                new BingSearchResult(ex.GetType().Name,
                                     ex.Message + " (This is not help. This exception has actually happened.)", "");
        }
    }
}
