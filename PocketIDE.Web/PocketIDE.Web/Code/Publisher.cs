using System;
using System.Net;
using System.Xml.Linq;
using Ninject;

namespace PocketIDE.Web.Code
{
    public class Publisher
    {
        private const string BitlyLogin = "pocketide";
        private const string BitlyApiKey = "R_a0bfe0f41c18467c241a9be95acfba32";

        private readonly IBlobHelper _blobHelper;

        [Inject]
        public Publisher(IBlobHelper blobHelper)
        {
            _blobHelper = blobHelper;
        }

        public string Publish(string userId, string saveName, string code)
        {
            var html = Properties.Resources.PublishTemplate
                .Replace("{TITLE}", saveName)
                .Replace("{CODE}", code);

            saveName = saveName.ToToken();

            if (!saveName.EndsWith(".html", StringComparison.InvariantCultureIgnoreCase))
            {
                saveName = saveName + ".html";
            }

            var url = _blobHelper.SaveText("pub", Guid.NewGuid().ToToken() + "/" + saveName.ToToken(), html, "text/html");
            return Shorten(url);
        }

        public static string Shorten(string urlToShorten)
        {
            try
            {
                using (var client = new WebClient())
                {
                    var bitlyUrl =
                        string.Format("http://api.bitly.com/v3/shorten?login={0}&apiKey={1}&longUrl={2}&format=xml",
                                      BitlyLogin, BitlyApiKey, Uri.EscapeUriString(urlToShorten));
                    var response = client.DownloadString(bitlyUrl);
                    var xml = XElement.Parse(response);
                    if (xml.Element("status_code") == null || xml.Element("status_code").Value != "200")
                    {
                        return urlToShorten;
                    }
                    return xml.Element("data").Element("url").Value;
                }
            }
            catch (WebException)
            {
                return urlToShorten;
            }
        }
    }
}