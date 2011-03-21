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

namespace PocketIDE.ViewModels
{
    public class PublishViewModel : INotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                PropertyChanged.Raise(this, "Name");
            }
        }

        private string _url = string.Empty;
        public string Url
        {
            get { return _url; }
            set
            {
                _url = value;
                PropertyChanged.Raise(this, "Url");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
