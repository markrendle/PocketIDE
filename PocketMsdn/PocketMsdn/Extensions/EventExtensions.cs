using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace System
{
    static class EventExtensions
    {
        public static void Raise<T>(this EventHandler<T> handler, object sender, Func<T> args)
            where T : EventArgs
        {
            if (handler != null)
            {
                handler(sender, args());
            }
        }
    }
}
