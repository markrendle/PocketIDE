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

namespace PocketCSharp
{
    public static class UriFactory
    {
        private const string Base = "http://pocketide.cloudapp.net/";
        public static Uri Create(string path)
        {
            return new Uri(Base + path.TrimStart('/'));
        }
    }
}
