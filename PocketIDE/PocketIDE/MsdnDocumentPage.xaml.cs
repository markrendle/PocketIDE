using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public partial class MsdnDocumentPage : PhoneApplicationPage
    {
        public MsdnDocumentPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            string selectedIndex;
            if (NavigationContext.QueryString.TryGetValue("selectedItem", out selectedIndex))
            {
                int index = int.Parse(selectedIndex);
                var searchResult = App.ViewModel.MsdnViewModel.SearchResults[index];
                DataContext = searchResult;
                var url = "http://pocketide.cloudapp.net/msdn?originalUrl=" + Uri.EscapeUriString(searchResult.MsdnUrl);
                var webClient = new WebClient();
                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DownloadStringCompleted);
                Browser.Source = new Uri(url);
            }
        }

        void DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}