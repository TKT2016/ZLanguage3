using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileDesc.Collections;
using ZLangRT.Attributes;
using ZLangRT.Utils;

namespace ZCompileDesc.ZTypes
{
    public interface ZType : IZDescType//, IWordDictionary
    {
        //public Type MarkType { get; protected set; }
        //public Type SharpType { get; protected set; }
        AccessAttributeEnum AccessAttribute { get; }

        //public virtual string ZName { get { return MarkType.Name; } protected set { } }
        bool IsMarkSelf { get; }

        //bool ContainsWord(string text);
        //WordInfo SearchWord(string text);

    }
    //public abstract class ZType : IZDescType, IWordDictionary
    //{
    //    public Type MarkType { get; protected set; }
    //    public Type SharpType { get; protected set; }
    //    public AccessAttributeEnum AccessAttribute { get; protected set; }

    //    public virtual string ZName { get { return MarkType.Name; } protected set { } }
    //    public virtual bool IsMarkSelf { get { return MarkType == SharpType; } }

    //    public abstract bool ContainsWord(string text);
    //    public abstract WordInfo SearchWord(string text); 
       
    //}
}
