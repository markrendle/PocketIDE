using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace CodeRunnerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                RunCode();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static void RunCode()
        {
            var code =
                @"using System;
class Program {
              public static void Main() {
                Console.WriteLine(""POSTED!"");
              }
            }";
            using (var client = new WebClient())
            {
                client.Headers["ContentType"] = "text/text";
                var results = client.UploadString("http://127.0.0.1:81/Run/Create", code);
                Console.WriteLine(results);
            }
        }
    }
}
