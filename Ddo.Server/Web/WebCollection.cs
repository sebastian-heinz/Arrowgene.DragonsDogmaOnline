using System;
using System.Collections.Generic;

namespace Ddo.Server.Web
{
    public class WebCollection<TKey, TValue>
    {
        private Dictionary<TKey, TValue> _collection;
        private Func<TKey, TKey> _keyTransformer;

        public WebCollection() : this(null)
        {
        }

        public WebCollection(Func<TKey, TKey> keyTransformer)
        {
            _collection = new Dictionary<TKey, TValue>();
            _keyTransformer = keyTransformer;
        }

        public ICollection<TKey> Keys => _collection.Keys;

        public void Add(TKey key, TValue value)
        {
            if (_keyTransformer != null)
            {
                key = _keyTransformer(key);
            }

            _collection.Add(key, value);
        }

        public TValue Get(TKey key)
        {
            if (_collection.TryGetValue(key, out TValue value))
            {
                return value;
            }

            return default(TValue);
        }

        public bool ContainsKey(TKey key)
        {
            return _collection.ContainsKey(key);
        }

        public TValue this[TKey key]
        {
            get => _collection[key];
            set => _collection[key] = value;
        }
    }
}
