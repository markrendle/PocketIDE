using System;
using System.ComponentModel;

namespace System
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
        public static void Raise(this PropertyChangedEventHandler handler, object sender, string propertyName)
        {
            if (handler != null)
            {
                handler(sender, new PropertyChangedEventArgs(propertyName));
            }
        }
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
