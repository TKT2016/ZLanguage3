using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.Tools;
using ZCompileDesc.Descriptions;
using ZCompileKit.Tools;

namespace ZCompileCore.ASTExps
{
    public abstract class ExpPropertyBase : ExpFieldPropertyBase
    {
        protected abstract MethodInfo GetGetMethod();
        protected abstract MethodInfo GetSetMethod();
        protected abstract bool GetIsStatic();

        public virtual void EmitGetProperty()
        {
            bool isstatic = GetIsStatic();
            MethodInfo getMethod = GetGetMethod();
            EmitHelper.EmitThis(IL, isstatic);
            EmitHelper.CallDynamic(IL, getMethod);
            base.EmitConv();
        }

        public virtual void EmitGetNestedProperty()
        {
            bool isstatic = GetIsStatic();
            MethodInfo getMethod = GetGetMethod();

            EmitHelper.EmitThis(IL, false);
            EmitSymbolHelper.EmitLoad(IL, LambdaThis);
            EmitHelper.CallDynamic(IL, getMethod);
            base.EmitConv();
        }

        public virtual void EmitSetProperty(Exp valueExp)
        {
            bool isstatic = GetIsStatic();
            MethodInfo setMethod = GetSetMethod();

            EmitHelper.EmitThis(IL, isstatic);
            EmitValueExp(valueExp);
            EmitHelper.CallDynamic(IL, setMethod);
        }

        public virtual void EmitSetNestedProperty(Exp valueExp)
        {
            bool isstatic = GetIsStatic();
            MethodInfo setMethod = GetSetMethod();

            EmitHelper.EmitThis(IL, false);
            EmitSymbolHelper.EmitLoad(IL, LambdaThis);
            EmitValueExp(valueExp);
            EmitHelper.CallDynamic(IL, setMethod);
        }

        public override void EmitGet()
        {
            if (IsNested)
            {
                EmitGetNestedProperty();
            }
            else
            {
                EmitGetProperty();
            }
        }

        public override void EmitSet(Exp valueExp)
        {
            if (IsNested)
            {
                 EmitSetNestedProperty(valueExp);
            }
            else
            {
                EmitSetProperty(valueExp);
            }
        }
    }
}
