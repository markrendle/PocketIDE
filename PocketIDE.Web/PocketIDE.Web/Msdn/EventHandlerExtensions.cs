using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PocketIDE.Web.Msdn
{
    static class EventHandlerExtensions
    {
        public static void Raise(this EventHandler handler, object sender)
        {
            if (handler != null)
            {
                handler(sender, EventArgs.Empty);
            }
        }
    }
}