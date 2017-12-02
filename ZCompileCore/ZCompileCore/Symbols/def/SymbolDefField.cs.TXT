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

namespace ZCompileCore.Symbols
{
    public class SymbolDefField : SymbolDefMember 
    {
        public SymbolDefField(string name, ZType fieldZType, bool isStatic)
            :base(name,fieldZType,isStatic)
        {
            CanRead = true;
            CanWrite = true;
        }

        public FieldInfo Field { get; set; }

        //public override bool CanWrite
        //{
        //    get
        //    {
        //        return !Field.IsInitOnly;
        //    }
        //    set
        //    {
        //        throw new CompileException();
        //    }
        //}
    }
}
