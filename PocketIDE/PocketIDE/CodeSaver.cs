using System;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Windows;
using JsonFx.Json;
using JsonFx.Serialization;
using JsonFx.Serialization.Resolvers;

namespace PocketCSharp
{
    public class CodeSaver
    {
        public void SaveLocal()
        {
            if (!App.ViewModel.CodeEditorViewModel.SaveName.EndsWith(".cs", StringComparison.CurrentCultureIgnoreCase))
            {
                App.ViewModel.CodeEditorViewModel.SaveName += ".cs";
            }
            using (var userStore = IsolatedStorageFile.GetUserStoreForApplication())
            using (var stream = userStore.OpenFile(App.ViewModel.CodeEditorViewModel.SaveName, FileMode.CreateNew, FileAccess.Write))
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(App.ViewModel.CodeEditorViewModel.Code);
                writer.Flush();
            }
        }

        public void SaveAsync()
        {
            var json = Code.ToBase64Json(App.ViewModel.CodeEditorViewModel.Code, App.ViewModel.CodeEditorViewModel.SaveName);
            var uri = UriFactory.Create("code/save");
            var webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
            webClient.UploadStringCompleted += UploadStringCompleted;
            webClient.UploadStringAsync(uri, json);
        }

        public event EventHandler<SaveCompletedEventArgs> SaveCompleted;

        private void UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                SaveCompleted.Raise(this, () => new SaveCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
            else
            {
                SaveCompleted.Raise(this, () => new SaveCompletedEventArgs(ServiceResponse.FromJson(e.Result).Message));
            }
        }
    }

    public class SaveCompletedEventArgs : AsyncCompletedEventArgs
    {
        private readonly string _message;

        public SaveCompletedEventArgs(string message)
        {
            _message = message;
        }

        public SaveCompletedEventArgs(Exception error, bool cancelled, object userState)
            : base(error, cancelled, userState)
        {

        }

        public string Message
        {
            get { return _message; }
        }
    }

    public class ServiceResponse
    {
        public ServiceResponse()
        {

        }

        public ServiceResponse(string message)
        {
            Message = message;
        }

        public string Message { get; set; }

        public static ServiceResponse FromJson(string json)
        {
            try
            {
                var reader =
                    new JsonReader(
                        new DataReaderSettings(
                            new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.PascalCase)));


                var result = reader.Read<ServiceResponse>(json);
                return new ServiceResponse(result.Message);
            }
            catch (Exception ex)
            {
                return new ServiceResponse(ex.Message);
            }
        }
    }
}
