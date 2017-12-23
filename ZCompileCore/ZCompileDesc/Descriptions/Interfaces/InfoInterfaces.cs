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
        //bool ZEquals(ZType ztype); 
        string ZTypeName { get; }
        AccessAttrEnum GetAccessAttr();
        bool IsStruct { get; }
    }
    public interface ICompling : IZObj { }
    public interface ICompleted : IZObj { }
    public interface ZLType : ICompleted, ZType
    {
        Type MarkType { get; }
        Type SharpType { get; }
        //string ZTypeName { get; }
    }

    public interface ZCType : ICompling, ZType
    {
       

    }

    ///// <summary>
    ///// Z语言根节点
    ///// </summary>
    //public interface IZLCRoot
    //{

    //}

    ///// <summary>
    ///// 正在编译的对象
    ///// </summary>
    //public interface ICompling : IZLCRoot
    //{
       
    //}

    ///// <summary>
    ///// 导入或已经编译好的对象
    ///// </summary>
    //public interface IZLObj : IZLCRoot
    //{

    //}

    ///// <summary>
    ///// Z语言类型(编译中或编译好的)
    ///// </summary>
    //public interface ZType : IZLCRoot
    //{

    //}

    ///// <summary>
    ///// Z语言导入或已经编译好的对象
    ///// </summary>
    //public interface IZLType : ZType , IZLObj
    //{

    //}

    ///// <summary>
    ///// Z语言导入或已经编译好的可以在程序中编程的对象
    ///// </summary>
    //public interface IZLProgrammeType :ZType, IZLType
    //{

    //}

    ///// <summary>
    ///// Z语言导入或已经编译好的只能在程序中做辅助的对象
    ///// </summary>
    //public interface IZLReferenceType : IZLType
    //{

    //}

    ///// <summary>
    ///// Z语言类型(编译中或编译好的)
    ///// </summary>
    //public interface ZType : IZLCRoot
    //{

    //}
}
