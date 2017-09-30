using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCompileDesc.Collections
{
    public class NamingList<T>
    {
        Dictionary<string, T> nameValueDict;
        Dictionary<T, string> valueNameDict;
        Dictionary<int, T> indexDict;

        public NamingList()
        {
            nameValueDict = new Dictionary<string, T>();
            valueNameDict = new Dictionary<T, string>();
            indexDict = new Dictionary<int, T>();
        }

        public void Add(string name,T t)
        {
            nameValueDict.Add(name, t);
            valueNameDict.Add(t, name);
            var index = indexDict.Count;
            indexDict.Add(index, t);
        }

        public int Count
        {
            get
            {
                return indexDict.Count;
            }
        }

        public List<string> Names
        {
            get{
                return nameValueDict.Keys.ToList();
            }
        }

        public List<T> Values
        {
            get
            {
                return indexDict.Values.ToList();
            }
        }

        public bool ContainsName(string name)
        {
            return nameValueDict.ContainsKey(name);
        }

        public T Get(int i)
        {
            return indexDict[i];
        }

        public T Get(string name)
        {
            return nameValueDict[name];
        }

        public string GetName(T t)
        {
            return valueNameDict[t];
        }
    }
}
