using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ComponentModel.Composition;
using System.ServiceModel.Web;
using System.Text;
using System.Web;

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
        public RunResult Run(Program program)
        {
            var code = Encoding.UTF8.GetString(Convert.FromBase64String(program.Code));
            return new Runner().CompileAndRun(code);
        }
    }

    public class Program
    {
        public string AuthorId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}