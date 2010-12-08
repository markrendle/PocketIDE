using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace MsdnHtml
{
    static class HtmlNodeExtensions
    {
        public static IEnumerable<HtmlNode> GetElementsByClassName(this HtmlNode node, string tagName, string className)
        {
            return node.Descendants(tagName)
                .Where(n => n.HasAttributes && n.Attributes["class"] != null && n.Attributes["class"].Value.ContainsDelimited(className, " "));
        }

        private static bool ContainsDelimited(this string str, string search, string delimiter)
        {
            return (delimiter + str + delimiter).Contains(delimiter + search + delimiter);
        }
    }
}
