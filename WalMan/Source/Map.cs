using System;
using System.Collections.Generic;

namespace WalMan
{
    public class Map<TKey, TValue>
    {
        (TKey key, TValue value)[] KeyValues = Array.Empty<(TKey key, TValue value)>();

        public TValue this[TKey key]
        {
            get
            {
                foreach ((TKey key, TValue value) keyValue in KeyValues)
                    if (EqualityComparer<TKey>.Default.Equals(key, keyValue.key))
                        return keyValue.value;

                throw new KeyNotFoundException();
            }

            set
            {
                if (key == null)
                    throw new NullReferenceException("Key is null.");

                if (HasKey(key))
                    throw new ArgumentException("Key already exists.");

                Array.Resize(ref KeyValues, KeyValues.Length + 1);
                KeyValues[^1] = (key, value);
            }
        }

        public bool HasKey(TKey key)
        {
            foreach ((TKey key, TValue value) keyValue in KeyValues)
                if (EqualityComparer<TKey>.Default.Equals(key, keyValue.key))
                    return true;

            return false;
        }

        public void Clear()
        {
            KeyValues = Array.Empty<(TKey key, TValue value)>();
        }

        public void Fill(TValue[] values, Func<TValue, TKey> KeyGetter)
        {
            Clear();

            foreach(TValue value in values)
                this[KeyGetter(value)] = value;
        }
    }
}
