using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using PocketCSharp.ViewModels;

namespace PocketCSharp
{
    public partial class HelpPage : PhoneApplicationPage
    {
        private readonly MsdnViewModel _viewModel;
        public HelpPage()
        {
            InitializeComponent();
            DataContext = _viewModel = App.ViewModel.MsdnViewModel;
            _viewModel.SearchResults.CollectionChanged += SearchResultsCollectionChanged;
        }

        void SearchResultsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ProgressBar.IsIndeterminate = e.Action == NotifyCollectionChangedAction.Reset;
        }

        private void SearchTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            ProgressBar.IsIndeterminate = true;
            _viewModel.SearchTerms = SearchTextBox.Text;
        }

        private void MainListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected index is -1 (no selection) do nothing
            if (MainListBox.SelectedIndex == -1)
                return;

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/MsdnDocumentPage.xaml?selectedItem=" + MainListBox.SelectedIndex, UriKind.Relative));

            // Reset selected index to -1 (no selection)
            MainListBox.SelectedIndex = -1;
        }
    }
}