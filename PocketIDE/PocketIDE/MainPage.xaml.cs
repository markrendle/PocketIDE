using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace PocketIDE
{
    public partial class MainPage : PhoneApplicationPage
    {
        private readonly SyntaxHighlighter _highlighter;
        private int _lastCodeLength;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            _highlighter = new SyntaxHighlighter(ColorTextBlock);

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
        }

        static void UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            App.ViewModel.CodeEditorViewModel.Output = e.Result;
        }

        private void MsdnButtonClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MsdnSearchPage.xaml", UriKind.Relative));
        }

        private void RunButtonClick(object sender, EventArgs e)
        {
            App.ViewModel.CodeEditorViewModel.Output = "Running...";
            App.ViewModel.CodeEditorViewModel.Code = CodeTextBox.Text;
            var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(App.ViewModel.CodeEditorViewModel.Code));
            var json = string.Format(@"{{""code"":""{0}""}}", encoded);
            var webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
            webClient.UploadStringCompleted += UploadStringCompleted;
            webClient.UploadStringAsync(new Uri("http://pocketide.cloudapp.net/Run/Create"), json);
            NavigationService.Navigate(new Uri("/BuildRunOutput.xaml", UriKind.Relative));
        }

        private void FontSizeUpButtonClick(object sender, EventArgs e)
        {
            ColorTextBlock.FontSize = CodeTextBox.FontSize += 2.0;
        }

        private void FontSizeDownButtonClick(object sender, EventArgs e)
        {
            ColorTextBlock.FontSize = CodeTextBox.FontSize -= 2.0;
        }

        private void CodeTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (CodeHasGotLonger())
            {
                _lastCodeLength = CodeTextBox.Text.Length;
                int caretPosition = CodeTextBox.SelectionStart;
                if (caretPosition > 0 && CodeTextBox.Text[caretPosition - 1] == '\r')
                {
                    IndentNewLine(caretPosition);
                    return;
                }
            }
            _highlighter.Highlight(CodeTextBox.Text);
        }

        private void IndentNewLine(int caretPosition)
        {
            var textBefore = CodeTextBox.Text.Substring(0, caretPosition);
            int spaces = CountSpacesAtStartOfLine(textBefore, textBefore.Length - 2);
            if (CodeTextBox.Text[caretPosition - 2] == '{')
            {
                spaces += 2;
            }
            var textAfter = CodeTextBox.Text.Substring(caretPosition + CodeTextBox.SelectionLength);
            CodeTextBox.Text = textBefore + new string(' ', spaces) + textAfter;
            CodeTextBox.SelectionStart = caretPosition + spaces;
        }

        private static int CountSpacesAtStartOfLine(string textBefore, int startFrom)
        {
            int spaces = 0;
            for (int i = startFrom; i >= 0 && textBefore[i] != '\r'; i--)
            {
                spaces = textBefore[i] == ' ' ? spaces + 1 : 0;
            }
            return spaces;
        }

        private bool CodeHasGotLonger()
        {
            var value = CodeTextBox.Text.Length > _lastCodeLength;
            _lastCodeLength = CodeTextBox.Text.Length;
            return value;
        }

        private void ExpandSnippetButtonClick(object sender, EventArgs e)
        {
            var snippetExpander = SnippetExpander.GetExpander(CodeTextBox.Text, CodeTextBox.SelectionStart);
            if (snippetExpander != null)
            {
                CodeTextBox.TextChanged -= CodeTextBoxTextChanged;
                CodeTextBox.Text = snippetExpander.NewText;
                CodeTextBox.SelectionStart = snippetExpander.NewCaretPosition;
                CodeTextBox.TextChanged += CodeTextBoxTextChanged;
                _highlighter.Highlight(CodeTextBox.Text);
            }
        }
    }
}