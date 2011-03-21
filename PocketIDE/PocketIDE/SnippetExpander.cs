using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace PocketCSharp
{
    public class SnippetExpander
    {
        private static readonly Dictionary<string, Snippet> Snippets = new Dictionary<string, Snippet>
                                                                           {
                                                                               { "cw", Snippet.ConsoleWriteLine },
                                                                               { "for", Snippet.ForLoop },
                                                                               { "foreach", Snippet.ForEachLoop },
                                                                           };
        private static readonly Regex SnippetRegex = new Regex("[a-z]+$");

        public static SnippetExpander GetExpander(string fullText, int caretPosition)
        {
            if (caretPosition == 0) return null;
            if (char.IsLower(fullText[caretPosition - 1]))
            {
                var snippetName = SnippetRegex.Match(fullText.Substring(0, caretPosition)).Value;
                if (Snippets.ContainsKey(snippetName))
                {
                    return new SnippetExpander(snippetName, fullText, caretPosition);
                }
            }
            return null;
        }

        private readonly string _snippetName;
        private readonly Snippet _snippet;
        private readonly string _originalText;
        private readonly int _caretPosition;
        private string _newText;
        private int _newCaretPosition = -1;

        private SnippetExpander(string snippetName, string originalText, int caretPosition)
        {
            _snippetName = snippetName;
            _caretPosition = caretPosition;
            _originalText = originalText;
            _snippet = Snippets[snippetName];
        }

        public string NewText
        {
            get { return _newText ?? ExpandAndReturnText(); }
        }

        public int NewCaretPosition
        {
            get { return _newCaretPosition >= 0 ? _newCaretPosition : ExpandAndReturnCaretPosition(); }
        }

        private int ExpandAndReturnCaretPosition()
        {
            Expand();
            return _newCaretPosition;
        }

        private string ExpandAndReturnText()
        {
            Expand();
            return _newText;
        }

        private void Expand()
        {
            var spaces = SpacesAtStartOfCurrentLine();

            var startText = _originalText.Substring(0, _caretPosition - _snippetName.Length) + _snippet.Before.Replace("\r", "\r" + spaces);
            _newCaretPosition = startText.Length;
            _newText = startText + _snippet.After.Replace("\r", "\r" + spaces) + _originalText.Substring(_caretPosition);
        }

        private string SpacesAtStartOfCurrentLine()
        {
            int spaces = 0;
            for (int i = _caretPosition - 1; i >= 0 && _originalText[i] != '\r'; i--)
            {
                spaces = _originalText[i] == ' ' ? spaces + 1 : 0;
            }
            return spaces == 0 ? string.Empty : new string(' ', spaces);
        }

    }
}
