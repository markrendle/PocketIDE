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
using JsonFx.Json;
using JsonFx.Serialization;
using JsonFx.Serialization.Resolvers;
using Microsoft.Phone.Controls;

namespace PocketIDE
{
    public partial class OpenPage : PhoneApplicationPage
    {
        public OpenPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            var webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.Accept] = "application/json";
            webClient.DownloadStringCompleted += ListDownloadStringCompleted;
            webClient.DownloadStringAsync(UriFactory.Create("code/list/" + Code.GetWindowsLiveAnonymousId() + "?nocache=" + Environment.TickCount));
        }

        void ListDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            ProgressBar.IsIndeterminate = false;
            if (!string.IsNullOrEmpty(e.Result))
            {
                var reader = new JsonReader();

                var result = reader.Read<IEnumerable<string>>(e.Result);
                foreach (var name in result)
                {
                    MainListBox.Items.Add(name);
                }
            }
            else
            {
                MessageBox.Show("No Saved code was found.");
                NavigationService.GoBack();
            }
        }

        private void MainListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var name = App.ViewModel.CodeEditorViewModel.SaveName = Regex.Replace(MainListBox.SelectedItem.ToString(), @"\.cs$", "");
            var webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.Accept] = "application/json";
            webClient.DownloadStringCompleted += OpenDownloadStringCompleted;
            webClient.DownloadStringAsync(UriFactory.Create("code/open/" + Code.GetWindowsLiveAnonymousId() + "/" + name + "?nocache=" + Environment.TickCount));
        }

        private void OpenDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            var reader = new JsonReader(new DataReaderSettings(
                                            new ConventionResolverStrategy(
                                                ConventionResolverStrategy.WordCasing.PascalCase)));
            var program = reader.Read<Program>(e.Result);
            App.ViewModel.CodeEditorViewModel.Code = Encoding.UTF8.GetString(Convert.FromBase64String(program.Code));
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }

    internal static class EncodingEx
    {
        public static string GetString(this Encoding encoding, byte[] bytes)
        {
            return encoding.GetString(bytes, 0, bytes.Length);
        }
    }
}