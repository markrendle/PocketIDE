using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace PocketMsdn.Extensions
{
    class MaybeXElement
    {
        internal static readonly MaybeXElement Empty = new MaybeXElement();

        protected MaybeXElement()
        {
        }

        public virtual string Value
        {
            get { return string.Empty; }
        }

        public virtual MaybeXElement Element(XName name)
        {
            return Empty;
        }

        public virtual IEnumerable<MaybeXElement> Elements(XName name)
        {
            return Enumerable.Empty<MaybeXElement>();
        }

        public static MaybeXElement Create(XElement element)
        {
            return element != null ? new MaybeXElementWithValue(element) : Empty;
        }

        private class MaybeXElementWithValue : MaybeXElement
        {
            private readonly XElement _element;

            public MaybeXElementWithValue(XElement element)
            {
                _element = element;
            }

            public override string Value
            {
                get
                {
                    return _element.Value;
                }
            }

            public override MaybeXElement Element(XName name)
            {
                return Create(_element.Element(name));
            }

            public override IEnumerable<MaybeXElement> Elements(XName name)
            {
                return _element.Elements(name).Select(Create);
            }
        }
    }
}
