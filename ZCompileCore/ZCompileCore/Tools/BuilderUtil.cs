using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ZCompileCore.Tools
{
    public static class BuilderUtil
    {
        public static MethodAttributes GetMethodAttr(bool isStatic)
        {
            if (isStatic)
                return MethodAttributes.Public | MethodAttributes.Static;
            else
                return MethodAttributes.Public;
        }

        public static FieldAttributes GetFieldAttr(bool isStatic)
        {
            if (isStatic)
                return FieldAttributes.Private | FieldAttributes.Static;
            else
                return FieldAttributes.Private;
        }
    }
}
