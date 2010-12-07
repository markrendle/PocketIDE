using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HtmlAgilityPack;

namespace CodeRunner.Controllers
{
    public class MsdnController : Controller
    {
        //
        // GET: /Msdn/

        public ActionResult Index(string originalUrl)
        {
            var robotUrl = originalUrl.Replace(".aspx", "(loband).aspx");
            var htmlWeb = new HtmlWeb();
            var doc = htmlWeb.Load(robotUrl);
            var mainBody = doc.GetElementbyId("mainBody");
            doc.DocumentNode.Element("html").Element("body").RemoveAllChildren();
            doc.DocumentNode.Element("html").Element("body").AppendChild(mainBody);

            doc.DocumentNode.Descendants("span").Where(IsLanguageSpan).ToList().ForEach(node => node.Remove());
            doc.DocumentNode.Descendants("div").Where(IsUnwantedLanguageDiv).ToList().ForEach(node => node.Remove());
            using (var writer = new StringWriter())
            {
                doc.Save(writer);
                return Content(writer.ToString());
            }
        }

        private static readonly HashSet<string> UnwantedLanguages = new HashSet<string> { "vb", "cpp", "nu"};
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
