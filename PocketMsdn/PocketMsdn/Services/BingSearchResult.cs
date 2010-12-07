using System;
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
    public class BingSearchResult
    {
        private readonly string _title;
        private readonly string _description;
        private readonly string _msdnUrl;

        public BingSearchResult(string title, string description, string displayUrl)
        {
            _title = title;
            _msdnUrl = displayUrl;
            _description = description.Contains("...") ? description.Substring(0, description.IndexOf("...")).Trim() : description;
        }

        public string MsdnUrl
        {
            get { return _msdnUrl; }
        }

        public string Description
        {
            get { return _description; }
        }

        public string Title
        {
            get { return _title; }
        }
    }
}
