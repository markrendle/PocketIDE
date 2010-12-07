using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace PocketMsdn.Services
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
