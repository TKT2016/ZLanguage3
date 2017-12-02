using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.Contexts;

namespace ZCompileCore.AST
{
    public abstract class SectionClassBase : SectionBase
    {
        public ContextClass ClassContext { get; set; }
    }
}
