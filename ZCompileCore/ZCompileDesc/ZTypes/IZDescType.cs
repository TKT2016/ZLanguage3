using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCompileDesc.ZTypes
{
    public interface IZDescType
    {
        string ZName { get; }
        Type MarkType { get; }
        Type SharpType { get;}
    }
}
