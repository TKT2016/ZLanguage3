using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST;
using ZCompileCore.Tools;
using ZLangRT;
using ZCompileDesc.Descriptions;
using ZCompileDesc.ZTypes;
using ZCompileDesc.ZMembers;

namespace ZCompileCore.Symbols
{
    public class SymbolDefMember :SymbolDefBase
    {
        public bool IsStatic { get; protected set; }
        public bool HasDefaultValue { get; set; }

        public SymbolDefMember(string name, ZType memberType, bool isStatic)
        {
            this.SymbolName = name;
            SymbolZType = memberType;
            HasDefaultValue = true;
            IsStatic = isStatic;
        }

        public static SymbolDefMember Create(string name,ZMemberInfo zmember)
        {
            if(zmember is ZPropertyInfo)
            {
                ZPropertyInfo zp = zmember as ZPropertyInfo;
                SymbolDefProperty symbol = new SymbolDefProperty(name, zp.MemberZType, zp.IsStatic);
                symbol.Property = zp.SharpProperty;
                return symbol;
            }
            //if (zmember is ZEnumElementInfo)
            //{
            //    ZEnumElementInfo zp = zmember as ZEnumElementInfo;
            //    SymbolDefEnumElement symbol = new SymbolDefEnumElement(name, zp);
            //    return symbol;
            //}
            else
            {
                ZFieldInfo zf = zmember as ZFieldInfo;
                SymbolDefField symbol = new SymbolDefField(name, zf.MemberZType, zf.IsStatic);
                symbol.Field = zf.SharpField;
                return symbol;
            }
        }

    }
}
