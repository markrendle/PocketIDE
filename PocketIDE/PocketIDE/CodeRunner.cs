using System;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PocketIDE.ViewModels;

namespace PocketIDE
{
    public class CodeRunner
    {
        private static readonly Uri CodeRunUri = new Uri("http://pocketide.cloudapp.net/Run/Create");
        private readonly CodeEditorViewModel _codeEditorViewModel;

        public CodeRunner(CodeEditorViewModel codeEditorViewModel)
        {
            _codeEditorViewModel = codeEditorViewModel;
        }

        public void Run()
        {
            var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(_codeEditorViewModel.Code));
            var json = string.Format(@"{{""code"":""{0}""}}", encoded);
            var webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
            webClient.UploadStringCompleted += UploadStringCompleted;
            webClient.UploadStringAsync(CodeRunUri, json);
        }

        void UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            _codeEditorViewModel.Output = e.Result;
        }
    }
}
