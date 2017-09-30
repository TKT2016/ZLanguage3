using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZCompileDesc.Collections;
using ZCompileDesc.Words;
using ZLangRT.Utils;

namespace ZCompileDesc.ZTypes
{
    public class ZDimType : IZDescType, IWordDictionary
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

        Dictionary<string, string> _Dims;
        public Dictionary<string, string> Dims
        {
            get
            {
                if (_Dims == null)
                {
                    _Dims = new Dictionary<string, string>();
                    FieldInfo[] fields = this.SharpType.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                    foreach (FieldInfo fieldInfo in fields)
                    {
                        if (!fieldInfo.IsStatic) continue;
                        if (!ReflectionUtil.IsDeclare(SharpType, fieldInfo)) continue;
                        
                        string propertyValue = fieldInfo.GetValue(null) as string;
                        if(!string.IsNullOrEmpty(propertyValue))
                        {
                            _Dims.Add(fieldInfo.Name, propertyValue);
                        }
                    }
                }
                return _Dims;
            }
        }


        #region IWordDictionary实现
        public bool ContainsWord(string text)
        {
            return Dims.ContainsKey(text);
        }

        public WordInfo SearchWord(string text)
        {
            if (!ContainsWord(text)) return null;
            WordInfo info1 = new WordInfo(text, WordKind.DimName);
            return info1;
        }
        #endregion

        public override string ToString()
        {
            return this.SharpType.Name + "[" + Dims.Keys.Count + "]";
        }
    }
}
