using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCompileKit.Tools
{
    /// <summary>
    /// 整理连续状态的类
    /// </summary>
    public  class ContinuousMerger<T>
    {
       public  T[] Source { get; set; }
       public Func<T, bool> InStateFunc { get; set; }
       public Func<T[], T> MergeObjsFunc { get; set; }
        int i = 0;
        int length = 0;
        List<T> list = new List<T>();

        public T[] Merge()
        {
            length= Source.Length;
            if (length == 0 || length == 1) return Source;
            i=0;
            list.Clear();
            T item = default(T);
            while(i<length)
            {
                item = Source[i];
                if(!InStateFunc(item))
                {
                    list.Add(item);
                    i++;
                }
                else
                {
                    //T newObj = item;
                    List<T> tempInItemes = new List<T>();
                    while (i < length && InStateFunc(item))
                    {
                        tempInItemes.Add(item);
                        //T next = getNext();
                        //newObj = MergeObjsFunc(item, next);
                        i++;
                        if (i < length)
                        {
                            item = Source[i];
                        }
                    }
                    //list.Add(newObj);
                    //i++;
                    T newObj = MergeObjsFunc(tempInItemes.ToArray());
                    list.Add(newObj);
                }
            }
            return list.ToArray();
        }

        private bool hasNext()
        {
            if (i < length - 1) return true;
            return false;
        }

        private bool nextIsInState()
        {
            if (!hasNext()) return false;
            T next = getNext();
            return (InStateFunc(next));
        }

        private T getNext()
        {
            return Source[i + 1];
        }
    }
}
