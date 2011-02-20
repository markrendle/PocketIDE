using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PocketIDE.Web.Code;
using PocketIDE.Web.Data;
using PocketIDE.Web.MvcUtil;

namespace PocketIDE.Web.Controllers
{
    public class CodeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [RequireRouteValues("id")]
        public ActionResult Index(string id)
        {
            return View("Code");
        }
        //
        // POST: /Run/Create

        [HttpPost]
        public ActionResult Save(string name, string code)
        {
            code = Encoding.UTF8.GetString(Convert.FromBase64String(code));
            new Saver().Save(name, code);
            return Content("Saved", "text/text");
        }

        public ActionResult View(string id)
        {
            var name = id + ".cs";
            ViewData["Name"] = name;
            ViewData["Code"] = new Loader().Load(name);
            return View();
        }

        public ActionResult List()
        {
            return Content(string.Join(";", new Loader().List()));
        }

        public ActionResult Open(string id)
        {
            return Redirect("http://pocketide.blob.core.windows.net/code/" + id + ".cs");
        }
    }
}
