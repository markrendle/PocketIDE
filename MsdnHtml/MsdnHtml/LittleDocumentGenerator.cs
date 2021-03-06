﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace MsdnHtml
{
    class LittleDocumentGenerator
    {
        private readonly HtmlDocument _document;
        public LittleDocumentGenerator(HtmlDocument document)
        {
            _document = document;
        }

        public void GenerateLittleDocuments()
        {
            var contentNodes = new Dictionary<string, HtmlNode> {{"toc", _document.GetElementbyId("toc")}};

            RemoveUnwantedNodes();

            ExtractContentNodes(contentNodes);

            HtmlNode bodyNode = EmptyBodyNode();

            TweakCss();

            AddScript();

            AddContentRegions(contentNodes, bodyNode);

            var htmlBytes = Encoding.Default.GetBytes(GetHtmlText());
            using (var stream = File.OpenWrite("main.html"))
            {
                stream.Write(htmlBytes, 0, htmlBytes.Length);
            }

            foreach (var contentNode in contentNodes)
            {
                RemoveTopLink(contentNode);
                WriteLittleDocument(contentNode, bodyNode);
            }
        }

        private static void AddContentRegions(Dictionary<string, HtmlNode> contentNodes, HtmlNode bodyNode)
        {
            foreach (var contentNode in contentNodes)
            {
                var titleNode = CreateContentTitleNode(contentNode);
                var linkNode = CreateContentLinkNode(titleNode);
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

        private static HtmlNode CreateContentLinkNode(HtmlNode titleNode)
        {
            var linkNode = HtmlNode.CreateNode("<a />");
            linkNode.SetAttributeValue("class", "region-link");
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
            AddScriptReference("http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.4.4.min.js");
            AddScriptReference("http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.6/jquery-ui.min.js");
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
            var headNode = _document.DocumentNode.Element("html").Element("head");
            var styleNode =
                HtmlNode.CreateNode(
                    @"<style type=""text/css""></span>");
            styleNode.InnerHtml = Properties.Resources.CodeCss;
            headNode.AppendChild(styleNode);
        }

        private void WriteLittleDocument(KeyValuePair<string, HtmlNode> contentNode, HtmlNode bodyNode)
        {
            bodyNode.AppendChild(contentNode.Value);
            string htmlText = GetHtmlText();

            var htmlBytes = Encoding.Default.GetBytes(htmlText);
            using (var stream = File.Create(contentNode.Key + ".html"))
            {
                stream.Write(htmlBytes, 0, htmlBytes.Length);
            }
            contentNode.Value.Remove();
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
            var bodyNode = _document.DocumentNode.Element("html").Element("body");
            bodyNode.RemoveAllChildren();
            return bodyNode;
        }

        private void ExtractContentNodes(Dictionary<string, HtmlNode> contentNodes)
        {
            var regionTitleNodes = _document.DocumentNode.GetElementsByClassName("span", "regiontitle").ToList();
            foreach (var node in regionTitleNodes)
            {
                var spanNode = node.ParentNode.ParentNode;
                contentNodes[node.InnerText.ToLower()] = spanNode;
                spanNode.Remove();
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
            var unwantedLanguageNodes = _document.DocumentNode.GetElementsByClassName("div", "libCScode").ToList();
            foreach (var node in unwantedLanguageNodes)
            {
                var titleBarNode = node.GetElementsByClassName("div", "CodeSnippetTitleBar").FirstOrDefault();
                if (titleBarNode != null)
                {
                    var languageNode =
                        titleBarNode.GetElementsByClassName("div", "CodeDisplayLanguage").FirstOrDefault();
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