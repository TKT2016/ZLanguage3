using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCompileKit.Tools
{
    public static class ListHelper
    {
        public static List<T> GetSubs<T>(List<T> list,int start)
        {
            List<T> newList = new List<T>();
            if(start<list.Count)
            {
                for(int i=start;i<list.Count;i++)
                {
                    newList.Add(list[i]);
                }
            }
            return newList;
        }
    }
}
