using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace WalMan.Source
{
    internal class ListDictionary<Tkey, TValue> : IDictionary<Tkey, TValue>
    {
        readonly List<Tkey> keys = new();
        readonly List<TValue> values = new();
        readonly List<KeyValuePair<Tkey, TValue>> keyValuePairs = new();

        public TValue this[Tkey key]
        {
            get
            {
                int i = keys.IndexOf(key);

                if (i < 0)
                    throw new KeyNotFoundException();

                return values[i];
            }
            set
            {
                int i = keys.IndexOf(key);

                if (i < 0)
                    Add(key, value);
                else
                {
                    values[i] = value;
                    keyValuePairs[i] = new KeyValuePair<Tkey, TValue>(key, value);
                }

            }
        }

        public ICollection<Tkey> Keys => keys;

        public ICollection<TValue> Values => values;

        public int Count => keys.Count;

        public bool IsReadOnly => false;

        public void Add(Tkey key, TValue value)
        {
            Add(new KeyValuePair<Tkey, TValue>(key, value));
        }

        public void Add(KeyValuePair<Tkey, TValue> item)
        {
            if (keys.IndexOf(item.Key) >= 0)
                throw new ArgumentException("An element with the same key already exists in the Dictionary");

            keys.Add(item.Key);
            values.Add(item.Value);
            keyValuePairs.Add(item);
        }

        public void Clear()
        {
            keys.Clear();
            values.Clear();
            keyValuePairs.Clear();
        }

        public bool Contains(KeyValuePair<Tkey, TValue> item)
        {
            return keyValuePairs.IndexOf(item) >= 0;
        }

        public bool ContainsKey(Tkey key)
        {
            return keys.IndexOf(key) >= 0;
        }

        public void CopyTo(KeyValuePair<Tkey, TValue>[] array, int arrayIndex)
        {
            for (int i = 0; i < keyValuePairs.Count; i++)
                array[i + arrayIndex] = keyValuePairs[i];
        }

        public IEnumerator<KeyValuePair<Tkey, TValue>> GetEnumerator()
        {
            return keyValuePairs.GetEnumerator();
        }

        public bool Remove(Tkey key)
        {
            int i = keys.IndexOf(key);

            if (i < 0)
                return false;

            keys.RemoveAt(i);
            values.RemoveAt(i);
            keyValuePairs.RemoveAt(i);
            return true;
        }

        public bool Remove(KeyValuePair<Tkey, TValue> item)
        {
            return Remove(item.Key);
        }

        public bool TryGetValue(Tkey key, [MaybeNullWhen(false)] out TValue value)
        {
            int i = keys.IndexOf(key);

            if (i < 0)
            {
                value = default;
                return false;
            }

            value = values[i];
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
