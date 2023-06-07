using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared
{
    public class MultiKeyMultiValueDictionary<K1, K2, V>
    {
        protected readonly Dictionary<K1, Dictionary<K2, List<V>>> _map;

        public MultiKeyMultiValueDictionary()
        {
            _map = new Dictionary<K1, Dictionary<K2, List<V>>>();
        }

        public bool Has(K1 key1, K2 key2)
        {
            if (!_map.ContainsKey(key1))
            {
                return false;
            }

            Dictionary<K2, List<V>> subDictionary = _map[key1];
            if (!subDictionary.ContainsKey(key2))
            {
                return false;
            }

            return true;
        }

        public List<V> Get(K1 key1, K2 key2)
        {
            if (!_map.ContainsKey(key1))
            {
                return new List<V>();
            }

            Dictionary<K2, List<V>> subDictionary = _map[key1];
            if (!subDictionary.ContainsKey(key2))
            {
                return new List<V>();
            }

            return new List<V>(subDictionary[key2]);
        }

        public void Add(K1 key1, K2 key2, V value)
        {
            Dictionary<K2, List<V>> subDictionary;
            if (_map.ContainsKey(key1))
            {
                subDictionary = _map[key1];
            }
            else
            {
                subDictionary = new Dictionary<K2, List<V>>();
                _map.Add(key1, subDictionary);
            }

            List<V> subDictionaryValueList;
            if (subDictionary.ContainsKey(key2))
            {
                subDictionaryValueList = subDictionary[key2];
            }
            else
            {
                subDictionaryValueList = new List<V>();
                subDictionary.Add(key2, subDictionaryValueList);
            }

            subDictionaryValueList.Add(value);
        }

        public void AddRange(K1 key1, K2 key2, List<V> values)
        {
            Dictionary<K2, List<V>> subDictionary;
            if (_map.ContainsKey(key1))
            {
                subDictionary = _map[key1];
            }
            else
            {
                subDictionary = new Dictionary<K2, List<V>>();
                _map.Add(key1, subDictionary);
            }

            List<V> subDictionaryValueList;
            if (subDictionary.ContainsKey(key2))
            {
                subDictionaryValueList = subDictionary[key2];
            }
            else
            {
                subDictionaryValueList = new List<V>();
                subDictionary.Add(key2, subDictionaryValueList);
            }

            subDictionaryValueList.AddRange(values);
        }

        public void Clear()
        {
            _map.Clear();
        }
    }


}