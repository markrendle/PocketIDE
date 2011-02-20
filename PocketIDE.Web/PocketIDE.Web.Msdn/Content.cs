using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace PocketIDE.Web.Msdn
{
    public class Content
    {
        public static string GetBlobFolderName(string msdnUrl)
        {
            var blobUrl = msdnUrl.Replace("http://msdn.microsoft.com/", "");
            return Regex.Replace(blobUrl, "[^a-z0-9]", "");
        }

        public static string GetBlobUrl(string msdnUrl, string sectionName)
        {
            return "http://pocketide.blob.core.windows.net/msdn/" + GetBlobFolderName(msdnUrl) + "/" + Uri.EscapeDataString(sectionName) + ".html";
        }
    }
}