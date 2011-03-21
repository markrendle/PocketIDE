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
using Microsoft.Phone.Tasks;

namespace PocketIDE
{
    public partial class PublishPage : PhoneApplicationPage
    {
        public PublishPage()
        {
            InitializeComponent();
            PublishButton.IsEnabled = !string.IsNullOrEmpty(PublishNameTextBox.Text);
        }

        private void PublishButtonClick(object sender, RoutedEventArgs e)
        {
            App.ViewModel.PublishViewModel.Name = PublishNameTextBox.Text;
            NavigationService.Navigate(new Uri("/PublishedPage.xaml", UriKind.Relative));
        }


        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void PublishNameTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            PublishButton.IsEnabled = !string.IsNullOrEmpty(PublishNameTextBox.Text);
        }

    }
}