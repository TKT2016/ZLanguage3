using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZCompileDesc.Words;
using ZCompileDesc.ZMembers;
using ZLangRT.Attributes;
using ZLangRT.Utils;

namespace ZCompileDesc.ZTypes
{
    /// <summary>
    /// Z枚举型(成员只有枚举值)
    /// </summary>
    public class ZEnumType : ZType
    {
        public ZEnumAttribute MarkAttribute { get; protected set; }
        public ZEnumItemInfo[] EnumElements { get; protected set; }

        public ZEnumType(Type type)
        {
            MarkType = type;
            MarkAttribute = AttributeUtil.GetAttribute<ZEnumAttribute>(type);
            if (MarkAttribute.ForType == null)
            {
                SharpType = type;
            }
            else
            {
                SharpType = MarkAttribute.ForType;
            }

            EnumElements = GetEnumElements(MarkType, SharpType);
            AccessAttribute = ReflectionUtil.GetAccessAttributeEnum(type);
        }

        public ZEnumItemInfo SearchValue(string zname)
        {
            foreach (var item in EnumElements)
            {
                if (item.HasZName(zname))
                    return item;
            }
            return null;
        }

        protected ZEnumItemInfo[] GetEnumElements(Type defType, Type sharpType)
        {
            List<ZEnumItemInfo> flist = new List<ZEnumItemInfo>();

            FieldInfo[] fields = ZTypeUtil.GetEnumItems(defType);
            foreach (var field in fields)
            {
                if(AttributeUtil.HasAttribute<ZCodeAttribute>(field))
                {
                    FieldInfo sharpField = this.SharpType.GetField(field.Name);
                    ZEnumItemInfo exField = new ZEnumItemInfo(field,sharpField, this);
                    flist.Add(exField);
                }
            }
            return flist.ToArray();
        }

        //protected override WordDictionary GetWordTable()
        //{
        //    WordDictionary dict = new WordDictionary(this.ZName);
        //    foreach(ZEnumElementInfo zele in EnumElements )
        //    {
        //        WordInfo[] words = zele.GetWordInfos();
        //        foreach (WordInfo word in words)
        //        {
        //            dict.Add(word);
        //        }
        //    }
        //    return dict;
        //}

        public override bool ContainsWord(string text)
        {
            foreach(ZEnumItemInfo zele in EnumElements )
            {
                if (zele.ContainsWord(text))
                {
                    return true;
                }
            }
            return false;
        }

        public override WordInfo SearchWord(string text)
        {
            foreach (ZEnumItemInfo zele in EnumElements)
            {
                if (zele.ContainsWord(text))
                {
                    return zele.SearchWord(text);
                }
            }
            return null;
        }

        public override string ToString()
        {
            return this.MarkType.Name + "-" + this.SharpType.Name;
        }
    }
}
