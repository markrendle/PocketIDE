using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Policy;
using System.Text;
using Microsoft.CSharp;

namespace PocketIDE.Web.Sandbox
{
    public class Runner
    {
        public bool CompileAndRun(string code, out string output)
        {
            var compiled = Compile(code);

            if (compiled.Errors.HasErrors)
            {
                output = ReturnErrors(compiled);
                return false;
            }
            output = ReturnOutput(compiled);
            return true;
        }

        private static string ReturnErrors(CompilerResults compiled)
        {
            var builder = new StringBuilder();
            builder.AppendLine("Compilation errors:");
            compiled.Errors.Cast<CompilerError>()
                .Select(ce => ce.ErrorText)
                .ToList()
                .ForEach(error => builder.AppendLine(error));
            return builder.ToString();
        }

        private static CompilerResults Compile(string code)
        {
            using (var csc = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } }))
            {
                var parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll" }) { IncludeDebugInformation = false, GenerateInMemory = false};
                var results = csc.CompileAssemblyFromSource(parameters, code);
                return results;
            }
        }

        private static string ReturnOutput(CompilerResults compiled)
        {
            //using (var writer = new StringWriter())
            //{
            //    Console.SetOut(writer);

                var setup = new AppDomainSetup { ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase };
                // get the set of permissions granted to code from the Internet.
                var internetEvidence = new Evidence(
                    new EvidenceBase[] { new Zone(SecurityZone.MyComputer) },
                    null);

                var permissionSet =
                    SecurityManager.GetStandardSandbox(internetEvidence);

                var strongName = typeof(Runner).Assembly.Evidence.GetHostEvidence<StrongName>();

                var appDomain = AppDomain.CreateDomain("foo", internetEvidence, setup, permissionSet, strongName);

                try
                {
                    var proxy = (Proxy)appDomain.CreateInstanceAndUnwrap(typeof (Proxy).Assembly.FullName, typeof (Proxy).FullName);
                    return proxy.RunMethod(compiled.PathToAssembly);
                }
                finally
                {
                    AppDomain.Unload(appDomain);
                }

            //    var assembly = appDomain.Load(compiled.CompiledAssembly.GetName());

            //    var type = assembly.GetType("Program");
            //    var main = type.GetMethod("Main");
            //    main.Invoke(null, null);
            //    return writer.ToString();
            //}
        }
    }

    public class Proxy : MarshalByRefObject
    {
        public string RunMethod(string assemblyPath)
        {
            var assembly = Assembly.LoadFrom(assemblyPath);
            var program = assembly.GetType("Program");
            var method = program.GetMethod("Main");
            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);
                method.Invoke(null, null);
                return writer.ToString();
            }
        }
    }
}
