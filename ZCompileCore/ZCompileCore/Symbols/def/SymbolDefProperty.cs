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
    public class SymbolDefProperty : SymbolDefMember
    {
        //public bool IsAssigned { get; set; }

        public SymbolDefProperty(string name, ZType propertyType, bool isStatic)
            :base(name,propertyType,isStatic)
        {
            CanRead = true;
            CanWrite = true;
        }

        public PropertyInfo Property { get; set; }

        //public override bool CanWrite
        //{
        //    get
        //    {
        //        return Property.CanWrite;
        //    }
        //    set
        //    {
        //        throw new CompileException();
        //    }
        //}
    }
}
