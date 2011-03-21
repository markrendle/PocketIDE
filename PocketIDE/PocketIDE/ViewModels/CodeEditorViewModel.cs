using System;
using System.ComponentModel;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Shell;
using PocketIDE.Local;

namespace PocketIDE.ViewModels
{
    public class CodeEditorViewModel : INotifyPropertyChanged
    {
        private const string DefaultCode = "using System;\rclass Program {\r  public static void Main() {\r    Console.WriteLine(\"IDE in a Phone!\");\r  }\r}";
        private string _code;
        
        public string Code
        {
            get { return _code; }
            set
            {
                if (value != null && _code != value)
                {
                    _code = value;
                    PropertyChanged.Raise(this, "Code");
                }
            }
        }

        public string SaveName { get; set; }

        private string _output;
        public string Output
        {
            get { return _output; }
            set
            {
                if (_output != value)
                {
                    _output = value;
                    PropertyChanged.Raise(this, "Output");
                }
            }
        }

        private double _fontSize = AppSettings.Default.FontSize;
        private bool _isRunning;
        private string _publishName;

        public double FontSize
        {
            get { return _fontSize; }
            set
            {
                if (_fontSize != value)
                {
                    _fontSize = value;
                    PropertyChanged.Raise(this, "FontSize");
                }
            }
        }

        public bool IsRunning
        {
            get {
                return _isRunning;
            }
            set {
                if (_isRunning != value)
                {
                    _isRunning = value;
                    PropertyChanged.Raise(this, "IsRunning");
                }
            }
        }

        public string PublishName
        {
            get {
                return _publishName;
            }
            set {
                if (_publishName != value)
                {
                    _publishName = value;
                    PropertyChanged.Raise(this, "PublishName");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Reset()
        {
            Code = DefaultCode;
            SaveName = string.Empty;
        }
    }
}
