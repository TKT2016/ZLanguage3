using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileDesc.Descriptions;

namespace ZCompileDesc.Collections
{
    public class ZLCollection
    {
        private List<ZLClassInfo> ZLClassList = new List<ZLClassInfo>();
        private List<ZLEnumInfo> ZLEnumList = new List<ZLEnumInfo>();
        private List<ZLDimInfo> ZLDimList = new List<ZLDimInfo>();

        public ZLClassInfo[] ZClasses
        {
            get
            {
                return ZLClassList.ToArray();
            }
        }

        public ZLEnumInfo[] ZEnums
        {
            get
            {
                return ZLEnumList.ToArray();
            }
        }

        public ZLDimInfo[] ZDims
        {
            get
            {
                return ZLDimList.ToArray();
            }
        }

        public void Add(ZLClassInfo zc)
        {
            ZLClassList.Add(zc);
        }

        public void Add(ZLEnumInfo zc)
        {
            ZLEnumList.Add(zc);
        }

        public void Add(ZLDimInfo zc)
        {
            ZLDimList.Add(zc);
        }

        public void Add(ZLCollection zc)
        {
            ZLClassList.AddRange(zc.ZLClassList);
            ZLEnumList.AddRange(zc.ZLEnumList);
            ZLDimList.AddRange(zc.ZLDimList);
        }

        public void Clear()
        {
            ZLClassList.Clear();
            ZLEnumList.Clear();
            ZLDimList.Clear();
        }

        public ZLType Get(string className)
        {
            foreach(var  item in ZLClassList)
            {
                if(item.ZClassName==className)
                {
                    return item;
                }
            }
            return null;
        }
    }
}
