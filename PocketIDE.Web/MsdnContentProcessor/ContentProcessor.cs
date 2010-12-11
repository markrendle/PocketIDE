using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace MsdnContentProcessor
{
    public class ContentProcessor
    {
        private readonly string _originalUrl;

        public ContentProcessor(string originalUrl)
        {
            _originalUrl = originalUrl;
        }

        public void ProcessAsync()
        {
            Action process = Process;
            process.BeginInvoke(process.EndInvoke, null);
        }

        private void Process()
        {
            
        }
        
        private static readonly HashSet<string> UnwantedLanguages = new HashSet<string> { "vb", "cpp", "nu" };
        private static bool IsLanguageSpan(HtmlNode span)
        {
            return UnwantedLanguages.Contains(span.GetAttributeValue("class", String.Empty));
        }

        private static bool IsUnwantedLanguageDiv(HtmlNode div)
        {
            return div.GetAttributeValue("class", "").Equals("libCScode")
                   &&
                   !div.Element("div").Element("div").InnerText.Trim().Equals("c#",
                                                                              StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
