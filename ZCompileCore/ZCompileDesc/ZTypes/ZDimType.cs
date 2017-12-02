using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZCompileDesc.Collections;
using ZCompileDesc.ZMembers;
using ZLangRT.Utils;

namespace ZCompileDesc.ZTypes
{
    public class ZDimType : IZDescType//, IWordDictionary
    {
        public Type MarkType { get; protected set; }
        public Type SharpType { get; private set; }

        public ZDimType(Type type)
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

        Dictionary<string, ZDimItemInfo> _Dims;
        public Dictionary<string, ZDimItemInfo> Dims
        {
            get
            {
                if (_Dims == null)
                {
                    _Dims = new Dictionary<string, ZDimItemInfo>();
                    FieldInfo[] fields = this.SharpType.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                    foreach (FieldInfo fieldInfo in fields)
                    {
                        if (!fieldInfo.IsStatic) continue;
                        if (!ReflectionUtil.IsDeclare(SharpType, fieldInfo)) continue;
                        
                        string propertyValue = fieldInfo.GetValue(null) as string;
                        ZDimItemInfo zd = new ZDimItemInfo(fieldInfo.Name, propertyValue);
                        Dims.Add(fieldInfo.Name, zd);
                        //if(!string.IsNullOrEmpty(propertyValue))
                        //{
                        //    _Dims.Add(fieldInfo.Name, propertyValue);
                        //}
                    }
                }
                return _Dims;
            }
        }

        public override string ToString()
        {
            return this.SharpType.Name + "[" + Dims.Keys.Count + "]";
        }
    }
}
