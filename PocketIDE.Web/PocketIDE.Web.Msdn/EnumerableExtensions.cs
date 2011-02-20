using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PocketIDE.Web.Msdn
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Append<T>(this IEnumerable<T> source, T newItem)
        {
            foreach (var item in source)
            {
                yield return item;
            }
            yield return newItem;
        }

        public static IEnumerable<Tuple<T1,T2>> Append<T1,T2>(this IEnumerable<Tuple<T1,T2>> source, T1 newItem1, T2 newItem2)
        {
            foreach (var item in source)
            {
                yield return item;
            }
            yield return Tuple.Create(newItem1, newItem2);
        }
    }
}