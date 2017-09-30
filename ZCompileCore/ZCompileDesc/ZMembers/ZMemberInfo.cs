using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZCompileDesc.Collections;
using ZCompileDesc.Words;
using ZCompileDesc.ZTypes;
using ZLangRT.Attributes;
using ZLangRT.Utils;

namespace ZCompileDesc.ZMembers
{
    public abstract class ZMemberInfo: IWordDictionary
    {
        public bool IsStatic { get; protected set; }
        public abstract ZType MemberZType { get;}
        public string[] ZNames { get; protected set; }
        public bool CanRead { get; protected set; }
        public bool CanWrite { get; protected set; }
        public AccessAttributeEnum AccessAttribute { get; protected set; }

        public virtual bool HasZName(string zname)
        {
            foreach(var item in ZNames)
            {
                if (item == zname)
                    return true;
            }
            return false;
        }

        public abstract string SharpMemberName { get; }
        //public abstract WordInfo[] GetWordInfos();

        public virtual bool ContainsWord(string text)
        {
            return this.HasZName(text);
        }

        public virtual WordInfo SearchWord(string text)
        {
            if(!HasZName(text)) return null;
            WordInfo info = new WordInfo(text, WordKind.MemberName,this);
            return info;
        }
    }
}
