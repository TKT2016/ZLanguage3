using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.Tools;
using ZCompileDesc.Descriptions;
using ZCompileCore.Contexts;

namespace ZCompileCore.AST.Exps
{
    public abstract class ExpFieldBase : ExpFieldPropertyBase
    {
        protected abstract FieldInfo GetFieldInfo();
        protected abstract bool GetIsStatic();

        public ExpFieldBase(ContextExp expContext)
            : base(expContext)
        {

        }

        public virtual void EmitLoadFielda()
        {
            FieldInfo fieldInfo = GetFieldInfo();
            //EmitHelper.EmitThis(IL, isstatic);
            EmitLoadMain();
            EmitHelper.LoadFielda(IL, fieldInfo);
            base.EmitConv();
        }

        public virtual void EmitGetField()
        {
            //bool isstatic = GetIsStatic();
            FieldInfo fieldInfo = GetFieldInfo();
            EmitLoadMain();
            //EmitHelper.EmitThis(IL, isstatic);
            EmitHelper.LoadField(IL, fieldInfo);
            base.EmitConv();
        }

        public virtual void EmitSetField(Exp valueExp)
        {
            FieldInfo fieldInfo = GetFieldInfo();
            //EmitHelper.EmitThis(IL, false);
            //EmitSymbolHelper.EmitLoad(IL, LambdaThis);
            EmitLoadMain();
            EmitValueExp(valueExp);
            EmitHelper.StormField(IL, fieldInfo);
        }

        public override void EmitGet()
        {
            EmitGetField();
            //if (this.ClassContext is ContextNestedClass)
            //{
            //    EmitGetNestedField();
            //}
            //else
            //{
            //    EmitGetField();
            //}
        }

        public override void EmitSet(Exp valueExp)
        {
            EmitSetField(valueExp);
            //if (this.ClassContext is ContextNestedClass)
            //{
            //    EmitSetNested(valueExp);
            //}
            //else
            //{
            //    EmitSetNestedField(valueExp);
            //}
        }
    }
}
