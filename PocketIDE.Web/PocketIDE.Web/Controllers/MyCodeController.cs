using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PocketIDE.Web.Code;
using PocketIDE.Web.Data;
using PocketIDE.Web.MvcUtil;

namespace PocketIDE.Web.Controllers
{
    public class MyCodeController : Controller
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

        //public ActionResult View(string id)
        //{
        //    var name = id + ".cs";
        //    ViewData["Name"] = name;
        //    ViewData["Code"] = NInjectFactory.Get<Loader>().Load(name);
        //    return View();
        //}

        //public ActionResult List()
        //{
        //    return Content(string.Join(";", NInjectFactory.Get<Loader>().List()));
        //}

        public ActionResult Open(string id)
        {
            return Redirect("http://pocketide.blob.core.windows.net/code/" + id + ".cs");
        }
    }
}
