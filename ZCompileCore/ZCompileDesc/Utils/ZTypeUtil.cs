using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZLangRT.Attributes;
using ZLangRT.Utils;
using ZCompileDesc.Utils;
using ZCompileDesc.Descriptions;
using ZLangRT;
using System.Reflection.Emit;
using Z语言系统;

namespace ZCompileDesc.Utils
{
    public static class ZTypeUtil
    {
        public static Type GetTypeOrBuilder(ZType ztype)
        {
            if (ztype == null) throw new NullReferenceException();
            if (ztype is ZLType)
            {
                Type type = ((ZLType)ztype).SharpType;
                return type;
            }
            else
            {
                TypeBuilder type = ((ZCClassInfo)ztype).ClassBuilder;
                return type;
            }
        }

        public static bool IsGenericType(ZType ztype)
        {
            if (ztype == null) throw new NullReferenceException();
            if (ztype is ZLType)
            {
                Type type = ((ZLType)ztype).SharpType;
                return type.IsGenericType;
            }
            return false;
        }

        public static bool IsBool(ZType ztype)
        {
            if (ztype == null) throw new NullReferenceException();
            if (ztype is ZLType)
            {
                Type type = ((ZLType)ztype).SharpType;
                return type == typeof(bool);
            }
            return false;
        }

        public static bool IsInt(ZType ztype)
        {
            if (ztype == null) throw new NullReferenceException();
            if (ztype is ZLType)
            {
                Type type = ((ZLType)ztype).SharpType;
                return type == typeof(int);
            }
            return false;
        }

        public static bool IsFloat(ZType ztype)
        {
            if (ztype == null) throw new NullReferenceException();
            if (ztype is ZLType)
            {
                Type type = ((ZLType)ztype).SharpType;
                return type==typeof(float);
            }
            return false;
        }

        public static bool IsListClass(ZType ztype)
        {
            if (ztype == null) throw new NullReferenceException();
            if (ztype is ZLType)
            {
                Type type = ((ZLType)ztype).SharpType;
                return IsListClass( type);
            }
            return false;
        }

        public static bool IsListClass(Type type)
        {
            if (type == null) throw new NullReferenceException();
            if (!type.Name.StartsWith(ZLangUtil.ZListClassZName)) return false;
            if (type.Namespace != ZLangUtil.LangPackageName) return false;
            return true;
        }

        public static bool IsVoid(ZType ztype)
        {
            if (ztype == null) throw new NullReferenceException();
            if (!(ztype is ZLType)) return false;
            ZLType zltype = (ZLType)ztype;
            return zltype.SharpType == typeof(void);
        }

        public static bool IsExtends(ZType subType, ZType supType)
        {
            if (subType == null) throw new NullReferenceException();
            if (supType == null) throw new NullReferenceException();
            if (supType is ZLType)
            {
                Type type = ((ZLType)supType).SharpType;
                return IsExtends(subType,type);
            }
            return false;
        }

        public static bool IsExtends(ZType subType,Type supType)
        {
            if (subType == null) throw new NullReferenceException();
            if (supType == null) throw new NullReferenceException();
            if (subType is ZLType)
            {
                Type type = ((ZLType)subType).SharpType;
                return ReflectionUtil.IsExtends(type, supType);
            }
            return false;
        }

        public static bool IsFn(ZType ztype)
        {
            if (ztype == null) throw new NullReferenceException();
            if (ztype is ZLType)
            {
                Type type = ((ZLType)ztype).SharpType;
                return ZLambda.IsFn(type);
            }
            return false;
        }

        public static bool IsAction(ZType ztype)
        {
            if (ztype == null) throw new NullReferenceException();
            if (ztype is ZLType)
            {
                Type type = ((ZLType)ztype).SharpType;
                return type == typeof(Action);
            }
            return false;
        }

        public static bool IsConditionFn(ZType ztype)
        {
            if (ztype == null) throw new NullReferenceException();
            if (ztype is ZLType)
            {
                Type type = ((ZLType)ztype).SharpType;
                return type == typeof(Func<bool>);
            }
            return false;
        }
    }
}
