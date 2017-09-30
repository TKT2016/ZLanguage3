using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileDesc.Collections;
using ZCompileDesc.Utils;
using ZCompileDesc.Words;
using ZCompileDesc.ZTypes;

namespace ZCompileDesc.Descriptions
{
    public class ZPackageDescList : List<ZPackageDesc>, IWordDictionary, IZTypeDictionary
    {
        public bool Contains(string packageName)
        {
            return GetZPackage(packageName) != null;
        }

        public ZPackageDesc GetZPackage(string packageName)
        {
            foreach(var zdesc in this)
            {
                if(zdesc.Name== packageName)
                {
                    return zdesc;
                }
            }
            return null;
        }

        public bool ContainsWord(string text)
        {
            return IWordDictionaryHelper.ArrayContainsWord(text, this); 
        }

        public WordInfo SearchWord(string text)
        {
            return IWordDictionaryHelper.EnumerableSearchWord(text, this); 
        }

        public bool ContainsZType(string zname)
        {
            return ZTypeListHelper.Contains(zname, this.ToArray());
        }

        public ZType[] SearchZType(string zname)
        {
            ZType[] data = ZTypeListHelper.Search(zname, this.ToArray());
            return data;
        }

        public bool ContainsZDescType(string zname)
        {
           foreach(var item in this)
           {
               if (item.ContainsZDescType(zname))
                   return true;
           }
           return false;
        }

        public IZDescType[] SearchZDescType(string zname)
        {
            List<IZDescType> ztypes = new List<IZDescType>();
            foreach (var item in this)
            {
                IZDescType[] desctypes = item.SearchZDescType(zname);
                ztypes.AddRange(desctypes);
            }
            return ztypes.ToArray();
        }
    }
}
