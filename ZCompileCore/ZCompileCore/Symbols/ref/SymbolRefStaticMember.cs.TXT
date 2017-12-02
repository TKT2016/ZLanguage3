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
using ZCompileDesc.ZMembers;

namespace ZCompileCore.Symbols
{
    public class SymbolRefStaticMember : SymbolRefBase
    {
        //public bool IsStatic { get; protected set; }
        //public bool IsAssigned { get;protected set; }
        public ZMemberInfo ZMember { get; protected set; }

        public SymbolRefStaticMember(string name, ZMemberInfo memberInfo)
        {
            this.SymbolName = name;
            SymbolZType = memberInfo.MemberZType;
            ZMember = memberInfo;
            //IsAssigned = true;
            //IsStatic = true;

            CanRead = memberInfo.CanRead;
            CanWrite = memberInfo.CanWrite;
        }
    }
}
