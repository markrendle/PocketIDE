using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using PocketMsdn.Services;


namespace PocketMsdn
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Bing _bing;

        public MainViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> Items { get; private set; }

        private string _searchTerms = string.Empty;

        public string SearchTerms
        {
            get { return _searchTerms; }
            set
            {
                if (_searchTerms.Equals(value, StringComparison.CurrentCultureIgnoreCase)) return;

                _searchTerms = value;
                if (_bing != null)
                {
                    _bing.CancelAsync();
                    _bing.BingSearchCompleted -= BingSearchCompleted;
                }
                _bing = new Bing();
                _bing.BingSearchCompleted += BingSearchCompleted;
                _bing.SearchAsync(_searchTerms);
            }
        }

        void BingSearchCompleted(object sender, BingSearchCompletedEventArgs e)
        {
            if (!ReferenceEquals(sender, _bing)) return;

            Items.Clear();
            var q = from bingSearchResult in e.Results
                    select new ItemViewModel(bingSearchResult);

            foreach (var itemViewModel in q)
            {
                Items.Add(itemViewModel);
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            this.IsDataLoaded = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}