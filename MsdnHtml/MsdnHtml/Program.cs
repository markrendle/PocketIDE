using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace MsdnHtml
{
    class Program
    {
        static readonly Regex RegionTitles = new Regex(@"<span\s+class=""regiontitle"".*?>\s*(?<title>.*?)\s*</span>");
        static void Main(string[] args)
        {
            var titles = RegionTitles.Matches(Properties.Resources.Html).Cast<Match>()
                .Select(m => m.Groups["title"].Value);

            foreach (var title in titles)
            {
                Console.WriteLine(title);
            }
        }
    }
}
