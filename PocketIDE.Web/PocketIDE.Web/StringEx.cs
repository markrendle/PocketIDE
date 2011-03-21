using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace PocketIDE.Web
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