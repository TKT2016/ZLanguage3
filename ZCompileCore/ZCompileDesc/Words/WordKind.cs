using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCompileDesc.Words
{
    public enum WordKind
    {
        OK=0,
        Unkown=1,
        TypeName=2,
        GenericClassName=4,
        EnumElement=8,
        MemberName=16,
        ArgName=32,
        VarName = 64,
        DimName=128,
        ProcNamePart=256,
        ParamName=512
    }
}
