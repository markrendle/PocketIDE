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
using PocketIDE.ViewModels;

namespace PocketIDE.PivotViews
{
    public partial class CodeEditor : UserControl
    {
        private readonly CodeEditorViewModel _viewModel;
        public CodeEditor()
        {
            InitializeComponent();
            DataContext = _viewModel = new CodeEditorViewModel();
        }

        private void RunButtonClick(object sender, RoutedEventArgs e)
        {
            var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(_viewModel.Code));
            var json = string.Format(@"{{""code"":""{0}""}}", encoded);
            var webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
            webClient.UploadStringCompleted += UploadStringCompleted;
            webClient.UploadStringAsync(new Uri("http://127.0.0.1:81/Run/Create"), json);
        }

        void UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            Console.WriteLine(e.Result);
            _viewModel.Output = e.Result;
            ShowOutput();
        }

        private void CodeButtonClick(object sender, RoutedEventArgs e)
        {
            ShowCode();
        }

        private void ShowCode()
        {
            CodeTextBox.Visibility = Visibility.Visible;
            ResultsText.Visibility = Visibility.Collapsed;
        }

        private void OutputButtonClick(object sender, RoutedEventArgs e)
        {
            ShowOutput();
        }

        private void ShowOutput()
        {
            CodeTextBox.Visibility = Visibility.Collapsed;
            ResultsText.Visibility = Visibility.Visible;
        }
    }
}
