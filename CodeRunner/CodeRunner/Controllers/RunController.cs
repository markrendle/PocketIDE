using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Microsoft.CSharp;

namespace CodeRunner.Controllers
{
    public class RunController : Controller
    {
        public ActionResult Index()
        {
            const string code = @"using System;
class Program {
              public static void Main() {
                Console.WriteLine(""Hello World!"");
              }
            }";
            return Content(CompileAndRun(code), "text/text");
        }
        //
        // POST: /Run/Create

        [HttpPost]
        public ActionResult Create(string code)
        {
            code = Encoding.UTF8.GetString(Convert.FromBase64String(code));
            return Content(CompileAndRun(code), "text/text");
        }

        private static string CompileAndRun(string code)
        {
            try
            {
                CompilerResults compiled;
                using (var csc = new CSharpCodeProvider(new Dictionary<string, string>() {{"CompilerVersion", "v4.0"}}))
                {
                    var parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll" })
                                         {GenerateInMemory = true, IncludeDebugInformation = false};
                    compiled = csc.CompileAssemblyFromSource(parameters, code);
                }


                if (compiled.Errors.HasErrors)
                {
                    var builder = new StringBuilder();
                    builder.AppendLine("Errors:");
                    compiled.Errors.Cast<CompilerError>()
                        .Select(ce => ce.ErrorText)
                        .ToList()
                        .ForEach(error => builder.AppendLine(error));
                    return builder.ToString();
                }
                else
                {
                    using (var writer = new StringWriter())
                    {
                        Console.SetOut(writer);

                        var type = compiled.CompiledAssembly.GetType("Program");
                        var main = type.GetMethod("Main");
                        main.Invoke(null, null);
                        return writer.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
