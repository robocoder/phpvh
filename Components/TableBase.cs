using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Serialization;

namespace Components
{
    [Serializable]
    public abstract class TableBase<TKey, TElement>
        where TKey : IEquatable<TKey>
    {
        [XmlIgnore]
        public abstract List<TElement> Items { get; protected set; }

        protected abstract TKey GetKey(TElement element);

        protected abstract TElement CreateElement(TKey key);

        private Func<TElement, bool> GetEquals(TKey key)
        {
            return x => GetKey(x).Equals(key);
        }

        [XmlIgnore]
        public TElement this[TKey key]
        {
            get { return Items.FirstOrDefault(GetEquals(key)); }
            set
            {
                if (!Items.Any(GetEquals(key)))
                {
                    Items.Add(CreateElement(key));
                }
                else
                {
                    ThrowKeyExists(key);
                }
            }
        }

        private void ThrowKeyExists(TKey key)
        {
            throw new InvalidOperationException(
                        string.Format("{0} already exists in table", key));
        }

        public void Add(TElement element)
        {
            var key = GetKey(element);
            if (this[key] != null)
            {
                ThrowKeyExists(key);
            }

            Items.Add(element);
        }

        public TableBase()
        {
            Items = new List<TElement>();
        }
    }
}
