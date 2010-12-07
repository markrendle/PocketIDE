using System;
using PocketMsdn.Extensions;

namespace System.Xml.Linq
{
    static class XmlLinqExtensions
    {
        public static string GetValueOrDefault(this XElement element)
        {
            return element != null ? element.Value : string.Empty;
        }

        public static MaybeXElement MaybeElement(this XElement element, XName name)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (element == null)
            {
                return MaybeXElement.Empty;
            }
            return MaybeXElement.Create(element.Element(name));
        }
    }
}
