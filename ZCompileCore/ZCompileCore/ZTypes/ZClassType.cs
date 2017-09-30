using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Utils;
using ZLangRT.Tags;
using ZLangRT.Descs;

namespace ZCompileCore.ZTypes
{
    /// <summary>
    /// Z类型(成员只有属性和方法，还有构造函数)
    /// </summary>
    public abstract class ZClassType:ZType
    {
        public ZClassType ParentMapping { get; set; }
        public abstract ExPropertyInfo SearchExProperty(string name);
        public abstract TKTConstructorDesc SearchConstructor(TKTConstructorDesc desc);
        public abstract TKTProcDesc SearchProc(TKTProcDesc procDesc);

        public abstract ExPropertyInfo[] GetPropertyInfoes();
        public abstract TKTConstructorDesc[] GetConstructors();
        public abstract TKTProcDesc[] GetProces();
    }
}
