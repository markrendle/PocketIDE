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
using Microsoft.WindowsAzure.StorageClient;
using PocketIDE.Web.Data;
using PocketIDE.Web.Sandbox;

namespace PocketIDE.Web.Code
{
    [ServiceContract]
    [Export]
    public class CodeResource
    {
#if(DEBUG)
        [WebGet(UriTemplate = "run")]
        public string RunTest()
        {
            return "Hello.";
        }
#endif

        [WebInvoke(UriTemplate = "run", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        public RunResult Run(Program program, HttpResponseMessage responseMessage)
        {
            if (!program.IsTrusted())
            {
                responseMessage.StatusCode = HttpStatusCode.Forbidden;
                responseMessage.Content = new StringContent("Request does not appear to be from a trusted source.");
                return null;
            }

            NInjectFactory.Get<ProgramAuditor>().Audit(program);

            var code = Encoding.UTF8.GetString(Convert.FromBase64String(program.Code));
            string output;
            return new Runner().CompileAndRun(code, out output)
                       ? new RunResult {Output = output}
                       : new RunResult {CompileError = output};
        }

        [WebInvoke(UriTemplate = "save", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        public void Save(Program program, HttpResponseMessage responseMessage)
        {
            if (!program.IsTrusted())
            {
                responseMessage.StatusCode = HttpStatusCode.Forbidden;
                responseMessage.Content = new StringContent("Request does not appear to be from a trusted source.");
                return;
            }

            var code = Encoding.UTF8.GetString(Convert.FromBase64String(program.Code));
            var user = new UserContext().GetOrAdd(program.AuthorId);
            var saver = NInjectFactory.Get<Saver>();
            saver.Save(user.UserId.ToString("N"), program.Name, code);
        }

        [WebInvoke(UriTemplate = "publish", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        public string Publish(Program program, HttpResponseMessage responseMessage)
        {
            if (!program.IsTrusted())
            {
                responseMessage.StatusCode = HttpStatusCode.Forbidden;
                responseMessage.Content = new StringContent("Request does not appear to be from a trusted source.");
                return string.Empty;
            }

            var code = Encoding.UTF8.GetString(Convert.FromBase64String(program.Code));
            var user = new UserContext().GetOrAdd(program.AuthorId);
            var publisher = NInjectFactory.Get<Publisher>();
            return publisher.Publish(user.UserId.ToString("N"), program.Name, code);
        }

        [WebGet(UriTemplate = "open/{windowsLiveAnonymousId}/{name}", ResponseFormat = WebMessageFormat.Json)]
        public Program Open(string windowsLiveAnonymousId, string name, HttpResponseMessage responseMessage)
        {
            var user = new UserContext().GetOrAdd(windowsLiveAnonymousId);
            var loader = NInjectFactory.Get<Loader>();
            return loader.Load(user, name);
        }

        [WebGet(UriTemplate = "list/{windowsLiveAnonymousId}", ResponseFormat = WebMessageFormat.Json)]
        public List<string> List(string windowsLiveAnonymousId, HttpResponseMessage responseMessage)
        {
            var user = new UserContext().GetOrAdd(windowsLiveAnonymousId);
            var loader = NInjectFactory.Get<Loader>();
            try
            {
                return loader.List(user.UserId).ToList();
            }
            catch (StorageClientException)
            {
                return null;
            }
        }
    }
}