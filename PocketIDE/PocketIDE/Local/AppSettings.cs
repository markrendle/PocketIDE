using System;
using System.IO.IsolatedStorage;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Marketplace;

namespace PocketIDE.Local
{
    public class AppSettings
    {
        public static readonly AppSettings Default = new AppSettings();

        private const string FontSizeProperty = "FontSize";
        private const string TrialRunCountProperty = "TrialRunCount";

        private static T Get<T>(string name, T defaultValue = default(T))
        {
            return IsolatedStorageSettings.ApplicationSettings.Contains(name)
                       ? (T) IsolatedStorageSettings.ApplicationSettings[name]
                       : defaultValue;
        }

        private static void Set(string name, object value)
        {
            IsolatedStorageSettings.ApplicationSettings[name] = value;
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public double FontSize
        {
            get { return Get<double>(FontSizeProperty, 18); }
            set { Set(FontSizeProperty, value); }
        }

        public int TrialRunCount
        {
            get { return Get<int>(TrialRunCountProperty); }
            set { Set(TrialRunCountProperty, value);}
        }

        public bool IsTrial
        {
            get
            {
#if(DEBUG)
                return false;
#else
                return new LicenseInformation().IsTrial();
#endif
            }
        }
    }
}
