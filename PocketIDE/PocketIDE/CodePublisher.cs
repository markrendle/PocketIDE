using System;
using System.Net;

namespace PocketCSharp
{
    public class CodePublisher
    {
        public void PublishAsync()
        {
            var json = Code.ToBase64Json(App.ViewModel.CodeEditorViewModel.Code, App.ViewModel.CodeEditorViewModel.PublishName);
            var uri = UriFactory.Create("code/publish");
            var webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
            webClient.Headers[HttpRequestHeader.Accept] = "application/json";
            webClient.UploadStringCompleted += UploadStringCompleted;
            webClient.UploadStringAsync(uri, json);
        }

        public event EventHandler<PublishCompletedEventArgs> PublishCompleted;

        private void UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            var argFunc = (e.Error != null || e.Cancelled)
                                                          ? (Func<PublishCompletedEventArgs>)(() =>
                                                            new PublishCompletedEventArgs(e.Error, e.Cancelled,
                                                                                          e.UserState))
                                                          : () => new PublishCompletedEventArgs(e.Result.Replace("\\", "").Replace("\"", ""));
            PublishCompleted.Raise(this, argFunc);
        }
    }
}
