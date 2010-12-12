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

namespace PocketIDE
{
    public class Code
    {
        public static string ToBase64Json(string code)
        {
            var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(code));
            return string.Format(@"{{""code"":""{0}""}}", encoded);
        }
    }
}
