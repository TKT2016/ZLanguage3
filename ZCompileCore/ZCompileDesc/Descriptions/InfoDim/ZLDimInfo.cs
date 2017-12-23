using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZCompileDesc.Collections;
using ZLangRT.Utils;

namespace ZCompileDesc.Descriptions
{
    public class ZLDimInfo : IZLObj
    {
        public Type MarkType { get; protected set; }
        public Type SharpType { get; private set; }

        public ZLDimInfo(Type type)
        {
            MarkType = type;
            SharpType = type;
        }
        
        public string ZName
        {
            get
            {
                return SharpType.Name;
            }
        }

        Dictionary<string, ZLDimItemInfo> _Dims;
        public Dictionary<string, ZLDimItemInfo> Dims
        {
            get
            {
                if (_Dims == null)
                {
                    _Dims = new Dictionary<string, ZLDimItemInfo>();
                    FieldInfo[] fields = this.SharpType.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                    foreach (FieldInfo fieldInfo in fields)
                    {
                        if (!fieldInfo.IsStatic) continue;
                        if (!ReflectionUtil.IsDeclare(SharpType, fieldInfo)) continue;
                        
                        string propertyValue = fieldInfo.GetValue(null) as string;
                        ZLDimItemInfo zd = new ZLDimItemInfo(fieldInfo.Name, propertyValue);
                        Dims.Add(fieldInfo.Name, zd);
                    }
                }
                return _Dims;
            }
        }

        //public bool ZEquals(ZType ztype)
        //{
        //    if(ztype is ZDimType)
        //    {
        //        ZDimType zdim = (ZDimType)ztype;

        //    }
        //    return false;
        //}

        public override string ToString()
        {
            return this.SharpType.Name + "[" + Dims.Keys.Count + "]";
        }
    }
}
