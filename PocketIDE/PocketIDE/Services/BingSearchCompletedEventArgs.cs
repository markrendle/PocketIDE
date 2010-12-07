using System;
using System.Collections.Generic;

namespace PocketIDE.Services
{
    public class BingSearchCompletedEventArgs : EventArgs
    {
        private readonly IEnumerable<BingSearchResult> _results;

        public BingSearchCompletedEventArgs(IEnumerable<BingSearchResult> results)
        {
            _results = results;
        }

        public IEnumerable<BingSearchResult> Results
        {
            get { return _results; }
        }
    }
}
