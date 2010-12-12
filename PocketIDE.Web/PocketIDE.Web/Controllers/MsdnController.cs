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

        public ActionResult Index(string id)
        {
            var originalUrl = "http://msdn.microsoft.com/en-us/" + id;
            var blobUrl = Msdn.Content.GetBlobUrl(originalUrl, "root");
            if (!UrlIsValid(blobUrl))
            {
                var lobandUrl = originalUrl.Replace(".aspx", "(loband).aspx");
                string html;
                using (var client = new WebClient())
                {
                    html = client.DownloadString(lobandUrl);
                }
                var littleDocumentGenerator = LittleDocumentGenerator.Create(originalUrl, html);
                html = Encoding.UTF8.GetString(littleDocumentGenerator.ProcessedDocument);
                var toSave = littleDocumentGenerator.Contents.Append("root", littleDocumentGenerator.ProcessedDocument);
                new ContentSaver().SaveContentsAsync(originalUrl, toSave);
            }
            return Redirect(blobUrl);
//            return Content(html, "text/html");
        }

        private static bool UrlIsValid(string url)
        {
            try
            {
                var request = WebRequest.Create(url);
                request.GetResponse();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
