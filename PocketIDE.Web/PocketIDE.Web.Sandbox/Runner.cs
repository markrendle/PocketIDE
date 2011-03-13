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
                var parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.dll", "System.Core.dll" }) { IncludeDebugInformation = true, GenerateInMemory = true};
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
