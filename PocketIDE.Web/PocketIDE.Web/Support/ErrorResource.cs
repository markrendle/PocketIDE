using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web;

namespace PocketIDE.Web.Support
{
    [ServiceContract]
    [Export]
    public class ErrorResource
    {
#if(DEBUG)
        [WebGet(UriTemplate = "test")]
        public string RunTest()
        {
            return "Hello.";
        }
#endif

        [WebInvoke(UriTemplate = "report", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        public void ReportError(ErrorReport report, HttpResponseMessage responseMessage)
        {
            if (!report.IsValid())
            {
                responseMessage.StatusCode = HttpStatusCode.BadRequest;
                return;
            }
            try
            {
                throw new PhoneException("Exception thrown in Phone app", report.Text.Base64Decode());
            }
            catch (PhoneException ex)
            {
                Trace.WriteLine(ex.Text);
            }
            responseMessage.StatusCode = HttpStatusCode.OK;
        }
    }

    [Serializable]
    public class PhoneException : Exception
    {
        private readonly string _text;

        public string Text
        {
            get { return _text; }
        }

        public PhoneException(string message, string text) : base(message)
        {
            _text = text;
        }

        public PhoneException(string message, Exception inner, string text) : base(message, inner)
        {
            _text = text;
        }

        protected PhoneException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
            _text = info.GetString("Text");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Text", _text);
            base.GetObjectData(info, context);
        }
    }

    [DataContract]
    [Serializable]
    public class ErrorReport
    {
        private const string Salt = "58D0CA96DFBC4A42A080E151FAC2A2A4";

        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public string Hash { get; set; }

        public bool IsValid()
        {
            return CreateHash(this) == Hash;
        }

        public static string CreateHash(ErrorReport report)
        {
            var sha1 = new SHA1Managed();
            return Convert.ToBase64String(sha1.ComputeHash(Encoding.UTF8.GetBytes(report.Text + Salt)));
        }
    }
}