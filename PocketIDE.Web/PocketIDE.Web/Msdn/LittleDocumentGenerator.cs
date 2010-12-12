using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using HtmlAgilityPack;

namespace PocketIDE.Web.Msdn
{
    class LittleDocumentGenerator
    {
        private readonly string _originalUrl;
        private readonly HtmlDocument _document;
        private readonly object _sync = new object();
        private readonly CssAggregator _cssAggregator = new CssAggregator();
        private bool _done;
        private byte[] _processedDocument;
        private readonly List<Tuple<string, byte[]>> _contents = new List<Tuple<string, byte[]>>();

        public static LittleDocumentGenerator Create(string originalUrl, string html)
        {
            var document = new HtmlDocument();
            document.LoadHtml(html);
            return new LittleDocumentGenerator(originalUrl, document);
        }

        private LittleDocumentGenerator(string originalUrl, HtmlDocument document)
        {
            _originalUrl = originalUrl;
            _document = document;
        }

        public byte[] ProcessedDocument
        {
            get
            {
                Generate();
                return _processedDocument;
            }
        }

        public IEnumerable<Tuple<string, byte[]>> Contents
        {
            get
            {
                Generate();
                return _contents;
            }
        }

        public Encoding Encoding
        {
            get { return _document.Encoding; }
        }

        private void Generate()
        {
            if (!_done)
            {
                lock (_sync)
                {
                    if (!_done)
                    {
                        GenerateLittleDocuments();
                        _done = true;
                    }
                }
            }
        }

        private void GenerateLittleDocuments()
        {
            AggregateCssAsync();

            var contentNodes = new Dictionary<string, HtmlNode> {{"toc", _document.GetElementbyId("toc")}};

            RemoveUnwantedNodes();

            ExtractContentNodes(contentNodes);

            HtmlNode bodyNode = EmptyBodyNode();

            TweakCss();

            AddScript();

            AddContentRegions(contentNodes, bodyNode);

            _processedDocument = Encoding.Default.GetBytes(GetHtmlText());

            _contents.AddRange(CreateContentNodes(contentNodes, bodyNode));
        }

        private void AggregateCssAsync()
        {
            var stylesheetLinkNodes = _document.DocumentNode.Element("html").Element("head").Elements("link")
                .Where(node => node.GetAttributeValue("rel", "") == "stylesheet").ToList();
            foreach (var node in stylesheetLinkNodes)
            {
                _cssAggregator.AddAsync(node.GetAttributeValue("href", ""));
                node.Remove();
            }
        }

        private IEnumerable<Tuple<string, byte[]>> CreateContentNodes(Dictionary<string, HtmlNode> contentNodes, HtmlNode bodyNode)
        {
            foreach (var contentNode in contentNodes)
            {
                RemoveTopLink(contentNode);
                yield return Tuple.Create(contentNode.Key, WriteLittleDocument(contentNode, bodyNode));
            }
        }

        private void AddContentRegions(Dictionary<string, HtmlNode> contentNodes, HtmlNode bodyNode)
        {
            foreach (var contentNode in contentNodes)
            {
                var titleNode = CreateContentTitleNode(contentNode);
                var linkNode = CreateContentLinkNode(titleNode, contentNode.Key);
                var headerNode = CreateContentHeaderNode(linkNode);
                bodyNode.AppendChild(headerNode);
            }
        }

        private static HtmlNode CreateContentTitleNode(KeyValuePair<string, HtmlNode> contentNode)
        {
            var titleNode = HtmlNode.CreateNode("<div />");
            titleNode.SetAttributeValue("class", "regionTitle fullWidth regionHeader");
            titleNode.InnerHtml = contentNode.Key;
            return titleNode;
        }

        private HtmlNode CreateContentLinkNode(HtmlNode titleNode, string sectionName)
        {
            var linkNode = HtmlNode.CreateNode("<a />");
            linkNode.SetAttributeValue("class", "region-link");
            linkNode.SetAttributeValue("data-content-url", Content.GetBlobUrl(_originalUrl, sectionName));
            linkNode.SetAttributeValue("href", "#");
            linkNode.AppendChild(titleNode);
            return linkNode;
        }

        private static HtmlNode CreateContentHeaderNode(HtmlNode linkNode)
        {
            var headerNode = HtmlNode.CreateNode("<div />");
            headerNode.SetAttributeValue("class", "regionMain fullWidth");
            headerNode.AppendChild(linkNode);
            return headerNode;
        }

        private void AddScript()
        {
            var headNode = _document.DocumentNode.Element("html").Element("head");
            AddScriptReference(Properties.Settings.Default.JQueryUri);
//            AddScriptReference(Properties.Settings.Default.JQueryUIUri);
            var scriptNode = HtmlNode.CreateNode("<script></script>");
            scriptNode.SetAttributeValue("type", "text/javascript");
            scriptNode.InnerHtml = Properties.Resources.MsdnJQueryCode;
            headNode.AppendChild(scriptNode);

            var baseNode = HtmlNode.CreateNode("<base />");
            baseNode.SetAttributeValue("href", string.Format("{0}://{1}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority));
            headNode.AppendChild(baseNode);
        }

        private void AddScriptReference(string src)
        {
            var jqueryNode = HtmlNode.CreateNode("<script></script>");
            jqueryNode.SetAttributeValue("type", "text/javascript");
            jqueryNode.SetAttributeValue("src", src);
            _document.DocumentNode.Element("html").Element("head").AppendChild(jqueryNode);
        }

        private void TweakCss()
        {
            EmbedCss(_cssAggregator.GetAggregatedCss());
            EmbedCss(Properties.Resources.CustomCss);
        }

        private void EmbedCss(string cssText)
        {
            var headNode = _document.DocumentNode.Element("html").Element("head");
            var styleNode =
                HtmlNode.CreateNode(
                    @"<style type=""text/css""></span>");
            styleNode.InnerHtml = cssText;
            headNode.AppendChild(styleNode);
        }

        private byte[] WriteLittleDocument(KeyValuePair<string, HtmlNode> contentNode, HtmlNode bodyNode)
        {
            return Encoding.Default.GetBytes(contentNode.Value.OuterHtml);
        }

        private string GetHtmlText()
        {
            var builder = new StringBuilder();
            using (var writer = new StringWriter(builder))
            {
                writer.NewLine = "\n";
                _document.Save(writer);
            }

            var htmlText = builder.ToString();
            if (htmlText.Contains("\r"))
            {
                htmlText = htmlText.Replace("\r", "");
            }
            return htmlText;
        }

        private static void RemoveTopLink(KeyValuePair<string, HtmlNode> contentNode)
        {
            var topLinkNode = contentNode.Value.Descendants("a").FirstOrDefault(
                link => link.GetAttributeValue("href", "").Equals("#mainbody", StringComparison.OrdinalIgnoreCase));
            if (topLinkNode != null)
            {
                topLinkNode.Remove();
            }
        }

        private HtmlNode EmptyBodyNode()
        {
            var mainBodyDiv = _document.GetElementbyId("mainBody");
            var summaryDiv = mainBodyDiv.GetElementsByClassName("div", "summary").FirstOrDefault();
            var summaryPara = summaryDiv != null ? summaryDiv.Element("p") : HtmlNode.CreateNode("<p />");
            summaryPara.SetAttributeValue("class", "summary");
            var bodyNode = _document.DocumentNode.Element("html").Element("body");
            bodyNode.RemoveAll();
            bodyNode.AppendChild(summaryPara);
            return bodyNode;
        }

        private void ExtractContentNodes(Dictionary<string, HtmlNode> contentNodes)
        {
            var regionTitleNodes = HtmlNodeExtensions.GetElementsByClassName(_document.DocumentNode, "span", "regiontitle").ToList();
            foreach (var node in regionTitleNodes)
            {
                var spanNode = node.ParentNode.ParentNode;
                spanNode.Remove();
                spanNode.Element("div").Remove();
                contentNodes[node.InnerText.ToLower()] = spanNode;
            }
        }

        private void RemoveUnwantedNodes()
        {
            _document.DocumentNode.Element("html").Elements("meta").ToList()
                .ForEach(node => node.Remove());

            _document.GetElementbyId("toc").Remove();
            _document.GetElementbyId("header").Remove();
            _document.GetElementbyId("feedback").Remove();
            _document.GetElementbyId("community").Remove();

            RemoveUnwantedLanguageSnippets();
        }

        private void RemoveUnwantedLanguageSnippets()
        {
            var unwantedLanguageNodes = HtmlNodeExtensions.GetElementsByClassName(_document.DocumentNode, "div", "libCScode").ToList();
            foreach (var node in unwantedLanguageNodes)
            {
                var titleBarNode = HtmlNodeExtensions.GetElementsByClassName(node, "div", "CodeSnippetTitleBar").FirstOrDefault();
                if (titleBarNode != null)
                {
                    var languageNode =
                        HtmlNodeExtensions.GetElementsByClassName(titleBarNode, "div", "CodeDisplayLanguage").FirstOrDefault();
                    if (languageNode != null)
                    {
                        if (languageNode.InnerText.Trim() != "C#")
                        {
                            node.Remove();
                        }
                    }
                }
            }
        }
    }
}