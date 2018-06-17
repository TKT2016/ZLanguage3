using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileDesc.Collections;
using ZCompileDesc.Utils;

namespace ZCompileDesc.Descriptions
{
    public class ZPackageDescList : List<ZPackageInfo>, IZTypeDictionary
    {
        public bool Contains(string packageName)
        {
            return GetZPackage(packageName) != null;
        }

        public ZPackageInfo GetZPackage(string packageName)
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

        public bool ContainsZType(string zname)
        {
            return ZTypeListHelper.Contains(zname, this.ToArray());
        }

        public ZLType[] SearchZType(string zname)
        {
            ZLType[] data = ZTypeListHelper.Search(zname, this.ToArray());
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

        public ZLType[] SearchZDescType(string zname)
        {
            List<ZLType> ztypes = new List<ZLType>();
            foreach (var item in this)
            {
                ZLType[] desctypes = item.SearchZDescType(zname);
                ztypes.AddRange(desctypes);
            }
            return ztypes.ToArray();
        }
    }
}
