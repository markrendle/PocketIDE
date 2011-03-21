using System;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace PocketIDE.Extensions
{
    public static class StringEx
    {
        public static string Base64Encode(this string source)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(source));
        }

        public static string Base64Decode(this string source)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(source));
        }
    }
}
