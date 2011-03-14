using System;
using System.Net;
using System.Security.Cryptography;
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
    public class ErrorReport
    {
        private const string Salt = "58D0CA96DFBC4A42A080E151FAC2A2A4";

        public string Text { get; set; }
        public string Hash { get; set; }

        public static string CreateHash(ErrorReport report)
        {
            var sha1 = new SHA1Managed();
            return Convert.ToBase64String(sha1.ComputeHash(Encoding.UTF8.GetBytes(report.Text + Salt)));
        }
    }
}
