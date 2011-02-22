using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.ComponentModel.Composition;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using PocketIDE.Web.Sandbox;

namespace PocketIDE.Web.Code
{
    [ServiceContract]
    [Export]
    public class CodeResource
    {
        [WebGet(UriTemplate = "run")]
        public string RunTest()
        {
            return "Hello.";
        }

        [WebInvoke(UriTemplate = "run", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        public RunResult Run(Program program, HttpResponseMessage responseMessage)
        {
            if (!program.IsTrusted())
            {
                responseMessage.StatusCode = HttpStatusCode.Forbidden;
                responseMessage.Content = new StringContent("Request does not appear to be from a trusted source.");
                return null;
            }

            var code = Encoding.UTF8.GetString(Convert.FromBase64String(program.Code));
            string output;
            new Runner().CompileAndRun(code, out output);
            return new RunResult {Output = output};
        }
    }

    public class RunResult
    {
        public string Output { get; set; }
    }
}