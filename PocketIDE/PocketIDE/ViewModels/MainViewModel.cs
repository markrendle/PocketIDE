using System;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace PocketIDE.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly MsdnViewModel _msdnViewModel = new MsdnViewModel();
        private readonly CodeEditorViewModel _codeEditorViewModel = new CodeEditorViewModel();
        private readonly PublishViewModel _publishViewModel = new PublishViewModel();

        public PublishViewModel PublishViewModel
        {
            get { return _publishViewModel; }
        }

        public MainViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();
        }

        public CodeEditorViewModel CodeEditorViewModel
        {
            get { return _codeEditorViewModel; }
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> Items { get; private set; }

        private string _sampleProperty = "Sample Runtime Property Value";
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        public MsdnViewModel MsdnViewModel
        {
            get { return _msdnViewModel; }
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

        public void LoadData()
        {
            
        }
    }
}