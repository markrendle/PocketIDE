using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;

namespace CodeRunner.Msdn
{
    public class DocumentProcessor
    {
        private readonly HtmlDocument _document = new HtmlDocument();

        private DocumentProcessor()
        {

        }

        private void LoadHtml(string html)
        {
            _document.LoadHtml(html);
        }

        public static DocumentProcessor Create(string html)
        {
            var instance = new DocumentProcessor();
            instance.LoadHtml(html);
            return instance;
        }

        public void RunAsync()
        {
            Action asyncAction = Run;
            asyncAction.BeginInvoke(asyncAction.EndInvoke, null);
        }

        private void Run()
        {
            
        }
    }
}