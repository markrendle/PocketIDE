using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using PocketIDE.Web.Data;
using HtmlAgilityPack;
using PocketIDE.Web.Msdn;

namespace PocketIDE.Web.Controllers
{
    public class MsdnController : Controller
    {
        static readonly Regex RegionTitles = new Regex(@"<span\s+class=""regiontitle"".*?>\s*(?<title>.*?)\s*</span>",RegexOptions.Multiline);

        //
        // GET: /Msdn/

        public ActionResult Index(string originalUrl)
        {
            var lobandUrl = originalUrl.Replace(".aspx", "(loband).aspx");
            string html;
            using (var client = new WebClient())
            {
                html = client.DownloadString(lobandUrl);
            }
            var littleDocumentGenerator = LittleDocumentGenerator.Create(html);
            html = Encoding.Default.GetString(littleDocumentGenerator.ProcessedDocument);
            return Content(html, "text/html");
        }
    }
}
