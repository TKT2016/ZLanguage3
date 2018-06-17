using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.Contexts;
using ZCompileCore.Tools;
using ZCompileDesc.Descriptions;

namespace ZCompileCore.AST.Exps
{
    public abstract class ExpFieldPropertyBase : ExpVarBase
    {
        protected ZCFieldInfo LambdaThis;

        public ExpFieldPropertyBase(ContextExp expContext)
            : base(expContext)
        {

        }

        public abstract void EmitGet();

        public override void Emit()
        {
            EmitGet();
        }

    }
}
