using System;
using System.Net;

namespace PocketIDE
{
    public class CodeSaver
    {
        public void SaveAsync()
        {
            var json = Code.ToBase64Json(App.ViewModel.CodeEditorViewModel.Code, App.ViewModel.CodeEditorViewModel.SaveName);
            var uri = new Uri("http://localhost:81/code/save");
            var webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
            webClient.UploadStringCompleted += UploadStringCompleted;
            webClient.UploadStringAsync(uri, json);
        }

        public event EventHandler SaveCompleted;

        private void UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            SaveCompleted.Raise(this);
        }
    }
}
