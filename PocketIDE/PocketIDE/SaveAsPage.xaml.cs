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

namespace PocketCSharp
{
    public partial class SaveAsPage : PhoneApplicationPage
    {
        public SaveAsPage()
        {
            InitializeComponent();
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            App.ViewModel.CodeEditorViewModel.SaveName = SaveNameTextBox.Text;
            var saver = new CodeSaver();
            saver.SaveCompleted += SaveCompleted;
            saver.SaveAsync();
        }

        void SaveCompleted(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}