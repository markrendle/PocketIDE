using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace PocketIDE
{
    internal class SyntaxHighlighter
    {
        private static readonly Regex KeywordRegex = new Regex(@"[a-z]+");
        private readonly TextBlock _textBlock;
        private string _text;
        private int _index;
        private StringBuilder _builder;

        public SyntaxHighlighter(TextBlock textBlock)
        {
            _textBlock = textBlock;
        }

        private char Current
        {
            get { return _text[_index]; }
        }

        public void Highlight(string text)
        {
            _textBlock.Inlines.Clear();
            _builder = new StringBuilder();
            _text = text;
            for (_index = 0; _index < _text.Length; _index++)
            {
                if (Current == '"')
                {
                    AddRun(Colors.Black);
                    HighlightStringConstant();
                }
                else
                {
                    if (!(char.IsLower(Current)))
                    {
                        CheckForKeyword();
                    }
                    _builder.Append(Current);
                }
            }

            AddRun(Colors.Black);
        }

        private void CheckForKeyword()
        {
            var match = KeywordRegex.Matches(_builder.ToString()).Cast<Match>().LastOrDefault();
            if (match != null)
            {
                var word = match.Value;
                if (KeyWords.Contains(word))
                {
                    if (match.Index > 0)
                    {
                        _builder.Remove(match.Index, _builder.Length - match.Index);
                        AddRun(Colors.Black);
                        _builder = new StringBuilder(word);
                    }
                    AddRun(Colors.Blue);
                }
            }
        }

        private void HighlightStringConstant()
        {
            _index++;
            bool escaped = false;
            _builder.Append('"');
            while (_index < _text.Length && (Current != '"' || escaped))
            {
                _builder.Append(Current);
                if (Current == '\\')
                {
                    escaped = !escaped;
                }
                else
                {
                    escaped = false;
                }
                _index++;
            }
            if (_index < _text.Length && Current == '"')
            {
                _builder.Append('"');
            }
            AddRun(Colors.Red);
        }

        private void AddRun(Color color)
        {
            _textBlock.Inlines.Add(new Run { Foreground = new SolidColorBrush(color), Text = _builder.ToString() });
            _builder = new StringBuilder();
        }

        private static readonly HashSet<string> KeyWords = new HashSet<string> {
                                                            "bool",
                                                            "byte",
                                                            "char",
                                                            "decimal",
                                                            "double",
                                                            "float",
                                                            "int",
                                                            "long",
                                                            "object",
                                                            "sbyte",
                                                            "short",
                                                            "string",
                                                            "uint",
                                                            "ulong",
                                                            "ushort",
                                                            "void",
                                                            "class",
                                                            "delegate",
                                                            "enum",
                                                            "interface",
                                                            "namespace",
                                                            "struct",
                                                            "break",
                                                            "continue",
                                                            "do",
                                                            "for",
                                                            "foreach",
                                                            "goto",
                                                            "return",
                                                            "while",
                                                            "else",
                                                            "if",
                                                            "switch",
                                                            "case",
                                                            "default",
                                                            "abstract",
                                                            "const",
                                                            "extern",
                                                            "internal",
                                                            "override",
                                                            "private",
                                                            "protected",
                                                            "public",
                                                            "readonly",
                                                            "sealed",
                                                            "static",
                                                            "virtual",
                                                            "volatile",
                                                            "false",
                                                            "null",
                                                            "true",
                                                            "try",
                                                            "catch",
                                                            "finally",
                                                            "throw",
                                                            "as",
                                                            "base",
                                                            "checked",
                                                            "event",
                                                            "fixed",
                                                            "in",
                                                            "is",
                                                            "lock",
                                                            "new",
                                                            "operator",
                                                            "out",
                                                            "params",
                                                            "ref",
                                                            "sizeof",
                                                            "stackalloc",
                                                            "this",
                                                            "typeof",
                                                            "unchecked",
                                                            "unsafe",
                                                            "using",
                                                            "add",
                                                            "remove",
                                                            "value",
                                                            "explicit",
                                                            "implicit"
                                                        };
    }
}