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

namespace PocketIDE.ViewModels
{
    public class CodeEditorViewModel : INotifyPropertyChanged
    {
        public CodeEditorViewModel()
        {
            Code = "using System;\rclass Program {\r  public static void Main() {\r    Console.WriteLine(\"IDE in a Phone!\");\r  }\r}";
        }

        public string Code { get; set; }

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

        private double _fontSize = 18;
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

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
