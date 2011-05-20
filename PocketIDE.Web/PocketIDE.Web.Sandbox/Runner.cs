using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Policy;
using System.Text;
using Microsoft.CSharp;

namespace PocketIDE.Web.Sandbox
{
    public class Runner
    {
        private static readonly string[] ConsoleAssemblyNames = new[]
                                                                    {
                                                                        "mscorlib.dll", "Microsoft.CSharp.dll",
                                                                        "System.dll", "System.Core.dll",
                                                                        "System.Data.dll", "System.Data.DataSetExtensions.dll",
                                                                        "System.Xml.dll", "System.Xml.Linq.dll",
                                                                    };

        private static readonly Dictionary<string, string> CSharpCodeProviderOptions = new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } };

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
            using (var csc = new CSharpCodeProvider(CSharpCodeProviderOptions))
            {
                var parameters = new CompilerParameters(ConsoleAssemblyNames) { IncludeDebugInformation = true, GenerateInMemory = true};
                var results = csc.CompileAssemblyFromSource(parameters, code);
                return results;
            }
        }

        private static string ReturnOutput(CompilerResults compiled)
        {
            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);

                var assembly = compiled.CompiledAssembly;

                var type = assembly.GetType("Program");
                var main = type.GetMethod("Main");
                main.Invoke(null, null);
                return writer.ToString();
            }
        }
    }
}
