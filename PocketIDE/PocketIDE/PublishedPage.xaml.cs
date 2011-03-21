using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
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
using Microsoft.Phone.Tasks;
using PocketCSharp.ViewModels;

namespace PocketCSharp
{
    public partial class PublishedPage : PhoneApplicationPage
    {
        private readonly PublishViewModel _viewModel = App.ViewModel.PublishViewModel;

        public PublishedPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            DataContext = _viewModel;
            if (!string.IsNullOrEmpty(_viewModel.Name))
            {
                var publisher = new CodePublisher();
                publisher.PublishCompleted += PublishCompleted;
                publisher.PublishAsync();
                PublishingPanel.Visibility = Visibility.Visible;
                PublishedPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                LoadUrl();
                PublishingPanel.Visibility = Visibility.Collapsed;
                PublishedPanel.Visibility = Visibility.Visible;
                ProgressBar.IsIndeterminate = false;
            }
            base.OnNavigatedTo(e);
        }

        void PublishCompleted(object sender, PublishCompletedEventArgs e)
        {
            _viewModel.Name = null;
            _viewModel.Url = e.Url;
            ProgressBar.IsIndeterminate = false;
            PublishingPanel.Visibility = Visibility.Collapsed;
            PublishedPanel.Visibility = Visibility.Visible;
        }

        private void CloseButtonClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void EmailButtonClick(object sender, RoutedEventArgs e)
        {
            SaveUrl();
            var emailComposeTask = new EmailComposeTask
            {
                Body = UrlTextBlock.Text,
                Subject = App.ViewModel.CodeEditorViewModel.PublishName,
            };
            emailComposeTask.Show();
        }

        private void SmsButtonClick(object sender, RoutedEventArgs e)
        {
            SaveUrl();
            var smsComposeTask = new SmsComposeTask
            {
                Body = UrlTextBlock.Text,
            };
            smsComposeTask.Show();
        }

        private void SaveUrl()
        {
            IsolatedStorageSettings.ApplicationSettings["PublishUrl"] = _viewModel.Url;
        }

        private void LoadUrl()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("PublishUrl"))
            {
                _viewModel.Url = IsolatedStorageSettings.ApplicationSettings["PublishUrl"].ToString();
            }
        }
    }
}