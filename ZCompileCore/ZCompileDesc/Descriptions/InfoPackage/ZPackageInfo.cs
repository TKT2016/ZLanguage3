using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileDesc.Collections;
using ZCompileDesc.Utils;
using ZLangRT;

namespace ZCompileDesc.Descriptions
{
    public class ZPackageInfo :IZTypeDictionary
    {
        public string Name { get; set; }
        public List<ZLDimInfo> DimTypes { get; protected set; }
        public List<ZLEnumInfo> EnumTypes { get; protected set; }
        public List<ZLClassInfo> ClassTypes { get; protected set; }

        public ZPackageInfo(string name)
        {
            Name = name;
            Init();
        }

        protected void Init()
        {
            DimTypes = new List<ZLDimInfo>();
            EnumTypes = new List<ZLEnumInfo>();
            ClassTypes = new List<ZLClassInfo>();
        }

        public bool ContainsZDescType(string zname)
        {
            return ContainsZType(zname) 
                // || ZDescTypeListHelper.Contains(zname, DimTypes.ToArray())
                ;
        }

        public ZLType[] SearchZDescType(string zname)
        {
            List<ZLType> ztypes = new List<ZLType>();
            ztypes.AddRange(SearchZType(zname));
            ztypes.AddRange(ZDescTypeListHelper.Search(zname, ClassTypes.ToArray()));
            return ztypes.ToArray();
        }

        public bool ContainsZType(string zname)
        {
            return ZTypeListHelper.Contains(zname, EnumTypes.ToArray()) || ZTypeListHelper.Contains(zname,ClassTypes.ToArray());
        }

        public ZLType SearchZTypeOne(string zname)
        {
            ZLType ztype = null;
            ztype = EnumerableHelper.SearchOne<ZLEnumInfo>(EnumTypes, (ZLEnumInfo zenum) => zenum.ZTypeName == zname);
            if (ztype == null)
            {
                ztype = EnumerableHelper.SearchOne<ZLClassInfo>(ClassTypes, (ZLClassInfo zclass) => zclass.GetZClassName() == zname);
            }
            return ztype;
        }

        public ZLType[] SearchZType(string zname)
        {
            ZLType ztype = SearchZTypeOne(zname);
            if (ztype == null)
            {
                return new ZLType[] { };
            }
            else
            {
                return new ZLType[] { ztype };
            }
        }

        public void AddZDimType(ZLDimInfo zdimType)
        {
            DimTypes.Add(zdimType);
        }

        public void AddZDescType(ZLType descType)
        {
            if (descType is ZLEnumInfo)
            {
                ZLEnumInfo zenum = descType as ZLEnumInfo;
                EnumTypes.Add(zenum);
            }
            else if (descType is ZLClassInfo)
            {
                ZLClassInfo zclass = descType as ZLClassInfo;
                ClassTypes.Add(zclass);
            }
            else
            {
                throw new ZyyRTException();
            }
        }

        public override string ToString()
        {
            return string.Format("{0}(dim[{1},enum:{2},class:{3})", Name, this.DimTypes.Count, this.EnumTypes.Count, this.ClassTypes.Count);
        }
    }
}
