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
using JsonFx.Json;
using JsonFx.Serialization;
using JsonFx.Serialization.Resolvers;
using PocketIDE.ViewModels;

namespace PocketIDE
{
    public class CodeRunner
    {
//        private static readonly Uri CodeRunUri = new Uri("http://pocketide.cloudapp.net/code/run");
        private static readonly Uri CodeRunUri = UriFactory.Create("code/run");
        private readonly CodeEditorViewModel _codeEditorViewModel;

        public CodeRunner(CodeEditorViewModel codeEditorViewModel)
        {
            _codeEditorViewModel = codeEditorViewModel;
        }

        public void Run()
        {
            _codeEditorViewModel.IsRunning = true;
            var json = Code.ToBase64Json(_codeEditorViewModel.Code);
            var webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
            webClient.Headers[HttpRequestHeader.Accept] = "application/json";
            webClient.UploadStringCompleted += UploadStringCompleted;
            webClient.UploadStringAsync(CodeRunUri, json);
        }

        void UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButton.OK);
                return;
            }
            var reader =
                new JsonReader(
                    new DataReaderSettings(
                        new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.PascalCase)));

            var result = reader.Read<RunResult>(e.Result);
            _codeEditorViewModel.Output = !string.IsNullOrEmpty(result.CompileError) ? result.CompileError : result.Output;
            _codeEditorViewModel.IsRunning = false;
        }
    }

    public class RunResult
    {
        public string Output { get; set; }
        public string CompileError { get; set; }
    }
}
