using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
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
            webClient.DownloadStringCompleted += ListDownloadStringCompleted;
            webClient.DownloadStringAsync(new Uri("http://pocketide.cloudapp.net/code/list?nocache=" + Environment.TickCount));
        }

        void ListDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            foreach (var name in e.Result.Split(';'))
            {
                MainListBox.Items.Add(name);
            }
        }

        private void MainListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var name = App.ViewModel.CodeEditorViewModel.SaveName = Regex.Replace(MainListBox.SelectedItem.ToString(), @"\.cs$", "");
            var webClient = new WebClient();
            webClient.DownloadStringCompleted += OpenDownloadStringCompleted;
            webClient.DownloadStringAsync(new Uri("http://pocketide.cloudapp.net/code/open/" + name + "?nocache=" + Environment.TickCount));
        }

        private void OpenDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            App.ViewModel.CodeEditorViewModel.Code = e.Result;
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }
}