using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace SyntaxSpike
{
    public partial class MainPage : PhoneApplicationPage
    {
        private readonly SyntaxHighlighter _highlighter;
        private Key _key1;
        private Key _key2;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            _highlighter = new SyntaxHighlighter(ColorTextBlock);
        }

        private void TheTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (CodeTextBox.Text.EndsWith(". ") && _key1 == Key.Space && _key2 == Key.Space)
            {
                CodeTextBox.Text = CodeTextBox.Text.Remove(CodeTextBox.Text.Length - 2) + "  ";
            }
            else
            {
                _highlighter.Highlight(CodeTextBox.Text);
            }
        }

        private void TheTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            _key1 = _key2;
            _key2 = e.Key;
        }
    }
}