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
using PocketCSharp.Services;

namespace PocketCSharp
{
    public partial class MsdnDocumentPage : PhoneApplicationPage
    {
        public MsdnDocumentPage()
        {
            InitializeComponent();
            Browser.IsScriptEnabled = true;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            string selectedIndex;
            if (NavigationContext.QueryString.TryGetValue("selectedItem", out selectedIndex))
            {
                int index = int.Parse(selectedIndex);
                var searchResult = App.ViewModel.MsdnViewModel.SearchResults[index];
                DataContext = searchResult;
                var url = searchResult.MsdnUrl;
                if (url.StartsWith("http://msdn.microsoft.com/en-us/library", StringComparison.InvariantCultureIgnoreCase))
                {
                    url = url.Replace("http://msdn.microsoft.com/", "");
                }
                Browser.Source = UriFactory.Create(url);
            }
        }
    }
}