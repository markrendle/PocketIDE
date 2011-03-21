using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace PocketIDE
{
    public class PublishCompletedEventArgs : AsyncCompletedEventArgs
    {
        private readonly string _url;

        public PublishCompletedEventArgs(string url)
        {
            _url = url;
        }

        public PublishCompletedEventArgs(Exception error, bool cancelled, object userState) : base(error, cancelled, userState)
        {
        }

        public string Url
        {
            get { return _url; }
        }
    }
}
