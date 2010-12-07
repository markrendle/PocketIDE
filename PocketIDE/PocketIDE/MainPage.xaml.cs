using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
        }

        void UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            Console.WriteLine(e.Result);
            App.ViewModel.CodeEditorViewModel.Output = e.Result;
        }

        private void MsdnButtonClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MsdnSearchPage.xaml", UriKind.Relative));
        }

        private void RunButtonClick(object sender, EventArgs e)
        {
            App.ViewModel.CodeEditorViewModel.Output = "Running...";
            App.ViewModel.CodeEditorViewModel.Code = CodeTextBox.Text;
            var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(App.ViewModel.CodeEditorViewModel.Code));
            var json = string.Format(@"{{""code"":""{0}""}}", encoded);
            var webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
            webClient.UploadStringCompleted += UploadStringCompleted;
            webClient.UploadStringAsync(new Uri("http://pocketide.cloudapp.net/Run/Create"), json);
            NavigationService.Navigate(new Uri("/BuildRunOutput.xaml", UriKind.Relative));
        }

        private void FontSizeUpButtonClick(object sender, EventArgs e)
        {
            CodeTextBox.FontSize += 2.0;
        }

        private void FontSizeDownButtonClick(object sender, EventArgs e)
        {
            CodeTextBox.FontSize -= 2.0;
        }
    }
}