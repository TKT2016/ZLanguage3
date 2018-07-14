using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCompileCore.CommonCollections
{
    public class PairDict<K,V>
    {
        Dictionary<K, V> kvdict;
        Dictionary<V, K> vkdict;

        public PairDict()
        {
            kvdict = new Dictionary<K, V>();
            vkdict = new Dictionary<V, K>();
        }

        public bool Add(K k,V v)
        {
            if (kvdict.ContainsKey(k) || vkdict.ContainsKey(v))
                return false;
            kvdict.Add(k, v);
            vkdict.Add(v, k);
            return true;
        }

        public bool ContainsK(K k)
        {
            return kvdict.ContainsKey(k);
        }

        public bool ContainsV(V v)
        {
            return vkdict.ContainsKey(v);
        }

        public V GetV(K k)
        {
            return kvdict[k];
        }

        public K GetK(V v)
        {
            return vkdict[v];
        }

        public int Count()
        {
            return kvdict.Count;
        }

        public List<K> Keys
        {
            get
            {
                return kvdict.Keys.ToList();
            }
        }
    }
}
