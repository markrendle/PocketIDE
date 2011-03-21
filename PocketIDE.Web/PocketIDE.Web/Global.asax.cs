using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.ServiceModel.Http;
using PocketIDE.Web.Code;
using PocketIDE.Web.Support;

namespace PocketIDE.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Msdn", "en-us/{*id}", new {controller = "Msdn", action = "Index"});
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            RegisterWcfRoutes();

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        private static void RegisterWcfRoutes()
        {
            // use MEF for providing instances
            var catalog = new AssemblyCatalog(typeof(MvcApplication).Assembly);
            var container = new CompositionContainer(catalog);
            var configuration = new CodeConfiguration(container);

            RouteTable.Routes.AddServiceRoute<CodeResource>("code", configuration);
            RouteTable.Routes.AddServiceRoute<ErrorResource>("exceptions", configuration);
        }
    }
}