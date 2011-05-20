using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
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

namespace PocketCSharp
{
    public partial class OpenPage : PhoneApplicationPage
    {
        public OpenPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            string[] localFiles;

            MainListBox.Items.Clear();
            using (var userStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                localFiles =
                    userStore.GetFileNames().Where(
                        s =>
                        s.EndsWith(".cs", StringComparison.CurrentCultureIgnoreCase) &&
                        !s.Equals("__current.cs", StringComparison.CurrentCultureIgnoreCase)).ToArray();
            }

            var webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.Accept] = "application/json";
            webClient.DownloadStringCompleted += ListDownloadStringCompleted;
            webClient.DownloadStringAsync(UriFactory.Create("code/list/" + Code.GetWindowsLiveAnonymousId() + "?nocache=" + Environment.TickCount), localFiles);
        }

        void ListDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null && !string.IsNullOrEmpty(e.Result))
            {
                var reader = new JsonReader();

                var result = reader.Read<IEnumerable<string>>(e.Result);
                PopulateMainListBox(result);
            }
            else
            {
                var localFiles = e.UserState as string[];
                if (localFiles != null && localFiles.Length > 0)
                {
                    PopulateMainListBox(localFiles);
                }
                else
                {
                    MessageBox.Show("No Saved code was found.");
                    NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                }
            }
        }

        private void PopulateMainListBox(IEnumerable<string> result)
        {
            foreach (var name in result)
            {
                MainListBox.Items.Add(name);
            }
        }

        private void MainListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (var userStore = IsolatedStorageFile.GetUserStoreForApplication())
            using (var stream = userStore.OpenFile(MainListBox.SelectedItem.ToString(), FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(stream))
            {
                App.ViewModel.CodeEditorViewModel.Code = reader.ReadToEnd();
            }
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