using System;
using System.IO;
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

namespace PocketIDE.Local
{
    public class CurrentCodePersister
    {
        public string Load()
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!store.FileExists("__current.cs")) return null;

                using (var file = store.OpenFile("__current.cs", FileMode.Open, FileAccess.Read))
                using (var reader = new StreamReader(file))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public void Save(string code)
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            using (var file = store.OpenFile("__current.cs", FileMode.Create))
            using (var writer = new StreamWriter(file))
            {
                writer.Write(code);
            }
        }

    }
}
