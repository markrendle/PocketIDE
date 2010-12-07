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
using PocketIDE.ViewModels;

namespace PocketIDE
{
    public partial class MsdnSearchPage : PhoneApplicationPage
    {
        private readonly MsdnViewModel _viewModel;
        public MsdnSearchPage()
        {
            InitializeComponent();
            DataContext = _viewModel = App.ViewModel.MsdnViewModel;
        }

        private void SearchTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
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