using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCompileCore.CommonCollections
{
    public class KeyListDictionary<K,T>
    {
        Dictionary<K,List<T>> table ;

        public KeyListDictionary()
        {
            table = new Dictionary<K, List<T>>();
        }

        public List<T> this[K name]
        {
            get{
                return Get(name);
            }
        }

        public void Clear()
        {
            table.Clear();
        }

        public void Add(K name,T t)
        {
            if(table.ContainsKey(name))
            {
                List<T> list = table[name];
                if(list.IndexOf(t)==-1)
                    list.Add(t);
            }
            else
            {
                List<T> list = new List<T>();
                list.Add(t);
                table.Add(name, list);
            }
        }

        public bool ContainsKey(K name)
        {
            return table.ContainsKey(name);
        }

        public List<T> Get(K name)
        {
            if (table.ContainsKey(name))
            {
                return table[name];
            }
            else
            {
                return new List<T>();
            }
        }

        public List<T> ValuesToList( )
        {
            List<T> list = new List<T>();
            foreach(var key in table.Keys)
            {
                List<T> sub = table[key];
                if (sub.Count > 0)
                {
                    list.AddRange(sub);
                }
            }
            return list;
        }

        public int Count
        {
            get
            {
                return table.Keys.Count;
            }
        }
    }
}
