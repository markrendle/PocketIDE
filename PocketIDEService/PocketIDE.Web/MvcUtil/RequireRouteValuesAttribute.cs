using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace PocketIDE.Web.MvcUtil
{
    public class RequireRouteValuesAttribute : ActionMethodSelectorAttribute
    {
        private readonly string[] _valueNames;

        public RequireRouteValuesAttribute(params string[] valueNames)
        {
            _valueNames = valueNames.ToArray();
        }

        /// <summary>
        /// Determines whether the action method selection is valid for the specified controller context.
        /// </summary>
        /// <returns>
        /// true if the action method selection is valid for the specified controller context; otherwise, false.
        /// </returns>
        /// <param name="controllerContext">The controller context.</param><param name="methodInfo">Information about the action method.</param>
        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            return !_valueNames.Any(name => !controllerContext.RequestContext.RouteData.Values.ContainsKey(name));
        }
    }
}