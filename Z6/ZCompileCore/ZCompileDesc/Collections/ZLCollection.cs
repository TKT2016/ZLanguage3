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

        public void Add(IZObj zc)
        {
            if (zc == null) throw new NullReferenceException("不能加入null");
            if(zc is ZLClassInfo)
            {
                ZLClassList.Add((ZLClassInfo)zc);
            }
            else if (zc is ZLEnumInfo)
            {
                ZLEnumList.Add((ZLEnumInfo)zc);
            }
            else if (zc is ZLDimInfo)
            {
                ZLDimList.Add((ZLDimInfo)zc);
            }
            else
            {
                throw new Exception("类型超出范围");
            }
        }

        public void Add(ZLCollection zc)
        {
            if (zc == null) throw new NullReferenceException("不能加入null");
            ZLClassList.AddRange(zc.ZLClassList);
            ZLEnumList.AddRange(zc.ZLEnumList);
            ZLDimList.AddRange(zc.ZLDimList);

            foreach(var zclass in zc.ZLClassList)
            {
                AddClassMethodNamePart(zclass);
            }
        }

        #region 导入过程名称部分
        private void AddClassMethodNamePart(ZLClassInfo zclass)
        {
            foreach (ZLMethodInfo zmethodInfo in zclass.ZMethods)
            {
                ZLMethodDesc[] itemNames = zmethodInfo.ZDescs;
                foreach (var desc in itemNames)
                {
                    ImportMethodDesc(desc);
                }
            }
        }


        private void ImportMethodDesc(ZLMethodDesc desc)
        {
            string[] strparts = desc.GetTextParts();

            foreach (var item in strparts)
            {
                if (item.Length > 1)
                {
                    AddProcNamePart(item, desc);
                }
            }
        }
        
        private Dictionary<string, List<ZLMethodDesc>> ProcNamePartDict = new Dictionary<string, List<ZLMethodDesc>>();
        private void AddProcNamePart(string namePart, ZLMethodDesc methodDesc)
        {
            List<ZLMethodDesc> list = null;
            if (ProcNamePartDict.ContainsKey(namePart))
            {
                list = ProcNamePartDict[namePart];
            }
            else
            {
                list = new List<ZLMethodDesc>();
                ProcNamePartDict.Add(namePart, list);
            }
            list.Add(methodDesc);
        }

        public bool IsProcNamePart(string namePart)
        {
            return ProcNamePartDict.ContainsKey(namePart);
        }

        #endregion

        public void Clear()
        {
            ZLClassList.Clear();
            ZLEnumList.Clear();
            ZLDimList.Clear();
            ProcNamePartDict.Clear();
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

            foreach (var item in ZLEnumList)
            {
                if (item.ZTypeName == className)
                {
                    return item;
                }
            }
            return null;
        }
    }
}
