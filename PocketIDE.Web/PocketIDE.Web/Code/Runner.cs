using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.CSharp;

namespace PocketIDE.Web.Code
{
    public class Runner
    {
        public RunResult CompileAndRun(string code)
        {
            try
            {
                CompilerResults compiled = Compile(code);

                if (compiled.Errors.HasErrors)
                {
                    return new RunResult {CompileError = ReturnErrors(compiled)};
                }
                return new RunResult {Output = ReturnOutput(compiled)};
            }
            catch (Exception ex)
            {
                return new RunResult {Output = ex.ToString()};
            }
        }

        private static string ReturnOutput(CompilerResults compiled)
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
                var parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll" }) { GenerateInMemory = true, IncludeDebugInformation = false };
                return csc.CompileAssemblyFromSource(parameters, code);
            }
        }
    }

    public class RunResult
    {
        public string Output { get; set; }
        public string CompileError { get; set; }
    }
}