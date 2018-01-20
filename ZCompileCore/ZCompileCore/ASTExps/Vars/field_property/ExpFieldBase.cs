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
    public abstract class ExpFieldBase : ExpFieldPropertyBase
    {
        protected abstract FieldInfo GetFieldInfo();
        protected abstract bool GetIsStatic();

        public virtual void EmitLoadFielda()
        {
            bool isstatic = GetIsStatic();
            FieldInfo fieldInfo = GetFieldInfo();
            EmitHelper.EmitThis(IL, isstatic);
            EmitHelper.LoadFielda(IL, fieldInfo);
            base.EmitConv();
        }

        public virtual void EmitGetField()
        {
            bool isstatic = GetIsStatic();
            FieldInfo fieldInfo = GetFieldInfo();

            EmitHelper.EmitThis(IL, isstatic);
            EmitHelper.LoadField(IL, fieldInfo);
            base.EmitConv();
        }

        public virtual void EmitGetNestedField()
        {
            FieldInfo fieldInfo = GetFieldInfo();

            EmitHelper.EmitThis(IL, false);
            EmitSymbolHelper.EmitLoad(IL, LambdaThis);
            EmitHelper.LoadField(IL, fieldInfo);
            base.EmitConv();
        }

        public virtual void EmitSetNested(Exp valueExp)
        {
            FieldInfo fieldInfo = GetFieldInfo();
            EmitHelper.EmitThis(IL, false);
            EmitSymbolHelper.EmitLoad(IL, LambdaThis);
            EmitValueExp(valueExp);
            EmitHelper.StormField(IL, fieldInfo);
        }

        public virtual void EmitSetNestedField(Exp valueExp)
        {
            bool isstatic = GetIsStatic();
            FieldInfo fieldInfo = GetFieldInfo();
            EmitHelper.EmitThis(IL, isstatic);
            EmitValueExp(valueExp);
            EmitHelper.StormField(IL, fieldInfo);
        }

        public override void EmitGet()
        {
            if (IsNested)
            {
                EmitGetNestedField();
            }
            else
            {
                EmitGetField();
            }
        }

        public override void EmitSet(Exp valueExp)
        {
            if (IsNested)
            {
                EmitSetNested(valueExp);
            }
            else
            {
                EmitSetNestedField(valueExp);
            }
        }
    }
}
