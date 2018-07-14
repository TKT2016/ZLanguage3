using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLangRT.Utils;

namespace ZCompileDesc.Descriptions
{
    public interface IZObj {  }
    public interface IZLObj : IZObj { }
    public interface ZType : IZObj
    {
        string ZTypeName { get; }
        AccessAttrEnum GetAccessAttr();
        bool IsStruct { get; }
        bool IsRuntimeType { get; }
    }
    public interface ICompling : IZObj { }
    public interface ICompleted : IZObj { }
    public interface ZLType : ICompleted, ZType
    {
        Type MarkType { get; }
        Type SharpType { get; }
        
    }

    public interface ZCType : ICompling, ZType
    {
       

    }

}
