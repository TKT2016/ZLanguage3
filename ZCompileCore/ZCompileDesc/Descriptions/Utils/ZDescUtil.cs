using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLangRT.Utils;

namespace ZCompileDesc.Descriptions.Utils
{
    public static class ZDescUtil
    {
        public static bool ZEqualsDesc(ZLConstructorDesc zmc1, ZNewCall zmc2)
        {
            return ZEqualsIBracket(zmc1.ZBracketDesc, zmc2.ZDesc);
        }

        public static bool ZEqualsDesc(ZLMethodDesc zmc1, ZCMethodDesc zmc2)
        {
            if (!ZDescUtil.ZEqualsIPartsCount(zmc1, zmc2))
            {
                return false;
            }
            if (!ZDescUtil.ZEqualsIPartsText(zmc1, zmc2))
            {
                return false;
            }
            if (!ZDescUtil.ZEqualsIPartsParameters(zmc1, zmc2))
            {
                return false;
            }
            return true;
        }

        public static bool ZEqualsDesc(ZLMethodDesc zmc1, ZMethodCall zmc2)
        {
            if (!ZDescUtil.ZEqualsIPartsCount(zmc1, zmc2))
            {
                return false;
            }
            if (!ZDescUtil.ZEqualsIPartsText(zmc1, zmc2))
            {
                return false;
            }
            if (!ZDescUtil.ZEqualsIPartsParameters(zmc1, zmc2))
            {
                return false;
            }
            return true;
        }

        public static bool ZEqualsDesc(ZCMethodDesc zmc1, ZMethodCall zmc2)
        {
            if (!ZDescUtil.ZEqualsIPartsCount(zmc1, zmc2))
            {
                return false;
            }
            if (!ZDescUtil.ZEqualsIPartsText(zmc1, zmc2))
            {
                return false;
            }
            if (!ZDescUtil.ZEqualsIPartsParameters(zmc1, zmc2))
            {
                return false;
            }
            return true;
        }

        public static bool ZEqualsDesc(ZCMethodDesc zmc1, ZCMethodDesc zmc2)
        {
            if (!ZDescUtil.ZEqualsIPartsCount(zmc1, zmc2))
            {
                return false;
            }
            if (!ZDescUtil.ZEqualsIPartsText(zmc1, zmc2))
            {
                return false;
            }
            if (!ZDescUtil.ZEqualsIPartsParameters(zmc1, zmc2))
            {
                return false;
            }
            return true;
        }

        public static bool ZEqualsIPartsCount(IParts ip1, IParts ip2)
        {
            return ip1.GetPartCount() == ip2.GetPartCount();
        }

        public static bool ZEqualsIPartsText(IParts ip1, IParts ip2)
        {
            int size = ip1.GetPartCount();
            object[] parts1 = ip1.GetParts();
            object[] parts2 = ip2.GetParts();
            for (int i = 0; i < size; i++)
            {
                var item1 = parts1[i];
                var item2 = parts2[i];
                if (item1 is string)
                {
                    if (!(item2 is string)) return false;
                    var str1 = item2 as string;
                    var str2 = item1 as string;
                    if (str1 != str2) return false;
                }
            }
            return true;
        }

        public static bool ZEqualsIPartsParameters(IParts ip1, IParts ip2)
        {
            int size = ip1.GetPartCount();
            object[] parts1 = ip1.GetParts();
            object[] parts2 = ip2.GetParts();
            for (int i = 0; i < size; i++)
            {
                var item1 = parts1[i];
                var item2 = parts2[i];
                if (item1 is IBracket)
                {
                    if (!(item2 is IBracket)) return false;
                    var b1 = item2 as IBracket;
                    var b2 = item1 as IBracket;
                    return ZEqualsIBracketCount(b1, b2);
                }
            }
            return true;
        }

        public static bool ZEqualsIBracket(IBracket ip1, IBracket ip2)
        {
            if (!ZEqualsIBracketCount(ip1, ip2)) return false;
            int size = ip1.GetParametersCount();
            IParameter[] parameters1 = ip1.GetParameters();
            IParameter[] parameters2 = ip2.GetParameters();
            for (int i = 0; i < size; i++)
            {
                var p1 = parameters1[i];
                var p2 = parameters2[i];

                ZTypeCompareEnum eqResult = ZEquals(p1, p2);
                 if(!(eqResult== ZTypeCompareEnum.EQ || eqResult== ZTypeCompareEnum.SuperOf))
                 {
                     return false;
                 }
            }
            return true;
        }


        public static bool ZEqualsIBracketCount(IBracket ip1, IBracket ip2)
        {
            return ip1.GetParametersCount() == ip2.GetParametersCount();
        }


        public static ZTypeCompareEnum Compare(ZType z1, ZType z2)
        {
            if (z1 is ZLEnumInfo || z2 is ZLEnumInfo) return ZTypeCompareEnum.NEQ;
            if (z1 is ZCClassInfo)
            {
                if (z2 is ZCClassInfo)
                {
                    return Compare((ZCClassInfo)z1, (ZCClassInfo)z2);
                }
                else
                {
                    return Compare((ZCClassInfo)z1, (ZLClassInfo)z2);
                }
            }
            else
            {
                if (z2 is ZCClassInfo)
                {
                    return Compare((ZLClassInfo)z1, (ZCClassInfo)z2);
                }
                else
                {
                    return Compare((ZLClassInfo)z1, (ZLClassInfo)z2);
                }
            }
        }

        private static ZTypeCompareEnum ZEquals(IParameter p1, IParameter p2)
        {
            if(!(ZEqualsName(p1,p2)))
            {
                return ZTypeCompareEnum.NEQ;
            }
            if (p1.IsCallArg())
            {
                 throw new Exception("调用参数只能在第二个");
            }
            ZTypeCompareEnum eqResult = Compare(p1.GetZParamType(),p2.GetZParamType());
            return eqResult;
        }

        private static bool ZEqualsName(IParameter p1, IParameter p2)
        {
            if (p1.IsCallArg() == false && p2.IsCallArg() == false)
            {
                if (p1.GetZParamName() != p2.GetZParamName())
                {
                    return false;
                }
            }

            return true;
        }

        private static ZTypeCompareEnum Compare(ZCClassInfo z1, ZCClassInfo z2)
        {
            return ZTypeCompareEnum.NEQ;
        }

        private static ZTypeCompareEnum Compare(ZLClassInfo z1, ZLClassInfo z2)
        {
            return Compare(z1.SharpType, z2.SharpType);
        }

        private static ZTypeCompareEnum Compare(ZLClassInfo z1, ZCClassInfo z2)
        {
            ZTypeCompareEnum result = Compare(z1, z2.BaseZClass);
            if (result == ZTypeCompareEnum.EQ) return ZTypeCompareEnum.SuperOf;
            else if (result == ZTypeCompareEnum.SuperOf) return ZTypeCompareEnum.SuperOf;
            else return ZTypeCompareEnum.NEQ;
        }

        private static ZTypeCompareEnum Compare(ZCClassInfo z1, ZLClassInfo z2)
        {
            ZTypeCompareEnum result = Compare(z1.BaseZClass, z2);
            if (result == ZTypeCompareEnum.EQ) return ZTypeCompareEnum.ExtendsOf;
            else if (result == ZTypeCompareEnum.SuperOf) return ZTypeCompareEnum.NEQ;
            else if (result == ZTypeCompareEnum.ExtendsOf) return ZTypeCompareEnum.ExtendsOf;
            else return ZTypeCompareEnum.NEQ;
        }

        private static ZTypeCompareEnum Compare(Type t1, Type t2)
        {
            if (t1 == t2) return ZTypeCompareEnum.EQ;
            if (ReflectionUtil.IsExtends(t1, t1)) return ZTypeCompareEnum.ExtendsOf;
            if (ReflectionUtil.IsExtends(t2, t2)) return ZTypeCompareEnum.SuperOf;
            return ZTypeCompareEnum.NEQ;
        }

        
    }
}
