using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using CodeRunner.Data;
using HtmlAgilityPack;

namespace CodeRunner.Controllers
{
    public class MsdnController : Controller
    {
        static readonly Regex RegionTitles = new Regex(@"<span\s+class=""regiontitle"".*?>\s*(?<title>.*?)\s*</span>");

        //
        // GET: /Msdn/

        public ActionResult Index(string originalUrl)
        {
            ProcessMsdnContentQueue.EnqueueAsync(originalUrl);

            var robotUrl = originalUrl.Replace(".aspx", "(robot).aspx");
            string html;
            using (var client = new WebClient())
            {
                html = client.DownloadString(robotUrl);
            }
            var titles = RegionTitles.Matches(html).Cast<Match>()
                .Select(m => m.Groups["title"].Value);

            return Content(string.Join("#", titles), "text/text");
        }
    }
}
