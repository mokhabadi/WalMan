using System;
using System.Collections;
using System.Collections.Generic;

namespace WalMan
{
    public class Map<TKey, TValue> : IEnumerable
    {
        (TKey key, TValue value)[] KeyValues = Array.Empty<(TKey key, TValue value)>();

        public IEnumerator GetEnumerator() => KeyValues.GetEnumerator();

        public void Add(TKey key, TValue value)
        {
            Add((key, value));
        }

        public void Add((TKey key, TValue value) keyValue)
        {
            if (keyValue.key == null)
                throw new NullReferenceException("Key is null.");

            if (HasKey(keyValue.key))
                throw new ArgumentException("Key already exists.");

            Array.Resize(ref KeyValues, KeyValues.Length + 1);
            KeyValues[^1] = keyValue;
        }

        public TValue this[TKey key]
        {
            get
            {
                foreach ((TKey key, TValue value) keyValue in KeyValues)
                    if (EqualityComparer<TKey>.Default.Equals(key, keyValue.key))
                        return keyValue.value;

                throw new KeyNotFoundException();
            }
        }

        public bool HasKey(TKey key)
        {
            foreach ((TKey key, TValue value) keyValue in KeyValues)
                if (EqualityComparer<TKey>.Default.Equals(key, keyValue.key))
                    return true;

            return false;
        }
    }
}
