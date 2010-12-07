using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace MsdnHtml
{
    class Program
    {
        static void Main(string[] args)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(Properties.Resources.Html);
            doc.DocumentNode.Descendants("span")
                .Where(IsRegionArea)
                .ToList()
                .ForEach(node => Console.WriteLine(node.InnerHtml));
        }

        private static bool IsRegionArea(HtmlNode node)
        {
            if (node.FirstChild == null) return false;
            var div = 
            return node.FirstChild.GetAttributeValue("class", "") == "regionArea";
        }
    }
}
