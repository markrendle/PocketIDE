using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public class HashSet<T> : ICollection<T>
    {
        private readonly Dictionary<T, short> _myDict = new Dictionary<T, short>();

        public void Add(T item)
        {
            // We don't care for the value in dictionary, Keys matter.
            _myDict.Add(item, 0);
        }

        public void Clear()
        {
            _myDict.Clear();
        }

        public bool Contains(T item)
        {
            return _myDict.ContainsKey(item);
        }

        public void CopyTo(T[] array, int index)
        {
            _myDict.Keys.CopyTo(array, index);
        }

        public bool Remove(T item)
        {
            return _myDict.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _myDict.Keys.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // Properties
        public int Count
        {
            get { return _myDict.Keys.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }
    }
}
