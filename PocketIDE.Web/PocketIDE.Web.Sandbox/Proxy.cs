using System;
using System.IO;
using System.Reflection;

namespace PocketIDE.Web.Sandbox
{
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