using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZLangRT.Attributes;
using ZLangRT.Utils;
using ZCompileDesc.Utils;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Collections;

namespace ZCompileDesc.ZMembers 
{
    public class ZConstructorInfo //: IWordDictionary
    {
        public ConstructorInfo Constructor { get; protected set; }
        public ZConstructorDesc ZDesc { get; protected set; }
        public AccessAttributeEnum AccessAttribute { get; protected set; }

        internal ZConstructorInfo( )
        {

        }

        public ZConstructorInfo(ConstructorInfo constructorInfo)
        {
            Constructor = constructorInfo;
            Init();
        }

        protected void Init()
        {
            ZDesc = ProcDescHelper.CreateZConstructorDesc(Constructor);
            AccessAttribute = GetAccessAttributeEnum(Constructor);
        }

        public virtual bool HasZConstructorDesc(ZNewDesc procDesc)
        {
            return ZDesc.ZEquals(procDesc);
        }

        internal static AccessAttributeEnum GetAccessAttributeEnum(ConstructorInfo constructor)
        {
            if (constructor == null) return AccessAttributeEnum.Private;
            if (constructor.IsPublic)
            {
                return AccessAttributeEnum.Public;
            }
            else if (constructor.IsPrivate)
            {
                return AccessAttributeEnum.Private;
            }
            else if (constructor.IsFamily)
            {
                return AccessAttributeEnum.Internal;
            }
            else if (constructor.IsFamilyOrAssembly)
            {
                return AccessAttributeEnum.Protected;
            }
            else
            {
                return AccessAttributeEnum.Private;
            }
        }
    }
}
