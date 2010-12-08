using System;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace MsdnHtml
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static void Run()
        {
            var document = new HtmlDocument();
            document.LoadHtml(Properties.Resources.Html);

            new LittleDocumentGenerator(document).GenerateLittleDocuments();
        }
    }
}
