using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileDesc.Collections;
using ZCompileDesc.Descriptions;

namespace ZCompileDesc.Utils
{
    public static class ZDescTypeListHelper
    {
        //public static bool Contains(string zname, params IZTypeDictionary[] dictList)
        //{
        //    foreach (var item in dictList)
        //    {
        //        if (item.ContainsZType( zname))
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        public static bool Contains(string zname, params ZLType[] dictList)
        {
            foreach (var item in dictList)
            {
                if (item.ZTypeName==zname)//(text))
                {
                    return true;
                }
            }
            return false;
        }

        public static ZLType[] Search(string zname, params ZLType[] dictList)
        {
            List<ZLType> words = new List<ZLType>();
            foreach (ZLType item in dictList)
            {
                if (item.ZTypeName == zname)//(text))
                {
                    words.Add(item);
                }
            }
            return words.ToArray();
        }

        //public static ZType[] Search(string zname, params IZTypeDictionary[] dictList)
        //{
        //    List<ZType> words = new List<ZType>();
        //    foreach (IZTypeDictionary item in dictList)
        //    {
        //        words.AddRange(item.SearchZType(zname));
        //    }
        //    return words.ToArray();
        //}
    }
}
