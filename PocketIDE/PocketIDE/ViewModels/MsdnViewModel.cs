using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using PocketCSharp.Services;

namespace PocketCSharp.ViewModels
{
    public class MsdnViewModel
    {
        private Bing _bing;
        private readonly Timer _timer;

        private readonly ObservableCollection<BingSearchResult> _searchResults = new ObservableCollection<BingSearchResult>();

        private string _searchTerms = string.Empty;

        public MsdnViewModel()
        {
            _timer = new Timer(TimerCallback);
        }

        public string SearchTerms
        {
            get { return _searchTerms; }
            set
            {
                if (_searchTerms.Equals(value, StringComparison.CurrentCultureIgnoreCase)) return;

                _searchTerms = value;
                _timer.Change(200, Timeout.Infinite);
            }
        }

        public ObservableCollection<BingSearchResult> SearchResults
        {
            get { return _searchResults; }
        }

        void TimerCallback(object state)
        {
            if (_bing != null)
            {
                _bing.CancelAsync();
                _bing.BingSearchCompleted -= BingSearchCompleted;
            }
            _bing = new Bing();
            _bing.BingSearchCompleted += BingSearchCompleted;
            _bing.SearchAsync(_searchTerms);
        }

        void BingSearchCompleted(object sender, BingSearchCompletedEventArgs e)
        {
            if (!ReferenceEquals(sender, _bing)) return;

            _searchResults.Clear();
            e.Results.ToList().ForEach(r => _searchResults.Add(r));
        }
    }
}
