using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Marketplace;
using Microsoft.Phone.Tasks;
using PocketCSharp.Local;

namespace PocketCSharp
{
    public partial class MainPage
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

        private void HelpButtonClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/HelpPage.xaml", UriKind.Relative));
        }

        private void RunButtonClick(object sender, EventArgs e)
        {
            if (AppSettings.Default.IsTrial && ++AppSettings.Default.TrialRunCount > 16)
            {
                var result = MessageBox.Show(
                    "You have used your 16 trial runs. Please buy Pocket C# to help pay for the service hosting.",
                    "Trial Expired", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    new MarketplaceDetailTask().Show();
                }
            }
            App.ViewModel.CodeEditorViewModel.Output = "Running...";
            App.ViewModel.CodeEditorViewModel.Code = CodeTextBox.Text;

            new CodeRunner(App.ViewModel.CodeEditorViewModel).Run();
            NavigationService.Navigate(new Uri("/BuildRunOutput.xaml", UriKind.Relative));
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
            App.ViewModel.CodeEditorViewModel.Code = CodeTextBox.Text;
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
                App.ViewModel.CodeEditorViewModel.Code = snippetExpander.NewText;
//                CodeTextBox.Text = snippetExpander.NewText;
                CodeTextBox.SelectionStart = snippetExpander.NewCaretPosition;
                CodeTextBox.TextChanged += CodeTextBoxTextChanged;
                _highlighter.Highlight(CodeTextBox.Text);
            }
        }

        private void SettingsButtonClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        private void OpenMenuItemClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/OpenPage.xaml", UriKind.Relative));
        }

        private void SaveMenuItemClick(object sender, EventArgs e)
        {
            App.ViewModel.CodeEditorViewModel.Code = CodeTextBox.Text;
            if (string.IsNullOrEmpty(App.ViewModel.CodeEditorViewModel.SaveName))
            {
                SaveAsMenuItemClick(sender, e);
            }
            else
            {
                new CodeSaver().SaveAsync();
            }
        }

        private void SaveAsMenuItemClick(object sender, EventArgs e)
        {
            App.ViewModel.CodeEditorViewModel.Code = CodeTextBox.Text;
            NavigationService.Navigate(new Uri("/SaveAsPage.xaml", UriKind.Relative));
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (string.IsNullOrEmpty(App.ViewModel.CodeEditorViewModel.Code))
                App.ViewModel.CodeEditorViewModel.Code = new CurrentCodePersister().Load();
            if (string.IsNullOrEmpty(App.ViewModel.CodeEditorViewModel.Code))
                App.ViewModel.CodeEditorViewModel.Reset();
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            new CurrentCodePersister().Save(App.ViewModel.CodeEditorViewModel.Code);
        }

        private void NewMenuItemClick(object sender, EventArgs e)
        {
            App.ViewModel.CodeEditorViewModel.Reset();
        }

        private void PublishMenuItemClick(object sender, EventArgs e)
        {
            App.ViewModel.CodeEditorViewModel.Code = CodeTextBox.Text;
            NavigationService.Navigate(new Uri("/PublishPage.xaml", UriKind.Relative));
        }
    }
}