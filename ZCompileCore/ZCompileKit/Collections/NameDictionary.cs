using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCompileKit.Collections
{
    public class NameDictionary<T> where T : IName
    {
        Dictionary<string,T> table ;

        public NameDictionary()
        {
            table = new Dictionary<string, T>();
        }

        public T this[string name]
        {
            get{
                return Get(name);
            }
            set
            {
                Set(value);
            }
        }

        public void Clear()
        {
            table.Clear();
        }

        public void Set( T t)
        {
            string name = t.Name;
            if (table.ContainsKey(name))
            {
                table[name] = t;
            }
            else
            {
                table.Add(name, t);
            }
        }

        public void Add(T t)
        {
            string name = t.Name;
            table.Add(name, t);
        }

        public bool ContainsKey(string name)
        {
            return table.ContainsKey(name);
        }

        public T Get(string name)
        {
            if (table.ContainsKey(name))
            {
                return table[name];
            }
            else
            {
                return default(T);
            }
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
