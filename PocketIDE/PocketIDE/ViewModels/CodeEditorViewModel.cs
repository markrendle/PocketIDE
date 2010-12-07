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
    public class CodeEditorViewModel : INotifyPropertyChanged
    {
        public CodeEditorViewModel()
        {
            Code = @"using System;
class Program {
  public static void Main() {
    Console.WriteLine(""IDE in a Phone!"");
  }
}";
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

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
