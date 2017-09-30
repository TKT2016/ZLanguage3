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
using ZCompileDesc.Words;

namespace ZCompileDesc.ZMembers 
{
    public class ZConstructorInfo: IWordDictionary
    {
        public ConstructorInfo Constructor { get; private set; }
        public ZConstructorDesc ZDesc { get; protected set; }
        public AccessAttributeEnum AccessAttribute { get; protected set; }

        public ZConstructorInfo(ConstructorInfo constructorInfo)
        {
            Constructor = constructorInfo;
            Init();
        }

        protected void Init()
        {
            //if (Constructor.DeclaringType.Name=="矩形")
            //{
            //    Console.WriteLine("ZCompileDesc.ZMembers.ZConstructorInfo.Init");
            //}
            ZDesc = ProcDescHelper.CreateZConstructorDesc(Constructor);
            AccessAttribute = GetAccessAttributeEnum(Constructor);
        }


        public bool ContainsWord(string text)
        {
            var paramsArr = this.Constructor.GetParameters();
            foreach (var item in paramsArr)
            {
                if(item.Name==text)
                {
                    return true;
                }
            }
            return false;
        }

        public WordInfo SearchWord(string text)
        {
            if (!ContainsWord(text)) return null;
            WordInfo newWord = new WordInfo(text, WordKind.ParamName, this);
            return newWord;
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
