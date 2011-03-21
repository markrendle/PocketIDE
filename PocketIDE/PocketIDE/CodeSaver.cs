using System;
using System.Net;

namespace PocketCSharp
{
    public class CodeSaver
    {
        public void SaveAsync()
        {
            var json = Code.ToBase64Json(App.ViewModel.CodeEditorViewModel.Code, App.ViewModel.CodeEditorViewModel.SaveName);
            var uri = UriFactory.Create("code/save");
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
