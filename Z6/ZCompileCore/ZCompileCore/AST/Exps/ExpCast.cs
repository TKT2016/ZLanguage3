using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Lex;
using ZLangRT.Utils;
using ZCompileDesc.Descriptions;
using ZCompileCore;
using ZCompileCore.Tools;
using ZLangRT;
using System;
using ZCompileDesc;
using ZCompileCore.AST.Exps;
using ZCompileCore.Contexts;
using ZCompileDesc.Utils;

namespace ZCompileCore.AST
{
    public class ExpCast : Exp
    {
        private ExpTypeBase _TypeExp;
        public ExpTypeBase TypeExp { get { return _TypeExp; } set { _TypeExp = value; _TypeExp.ParentExp = this; } }

        private Exp _ArgExp;
        public Exp ArgExp { get { return _ArgExp; } set { _ArgExp = value; _ArgExp.ParentExp = this; } }

        Type calcType = typeof(Calculater);
        MethodInfo caseMethod;

        public ExpCast(ContextExp expContext, ExpTypeBase typeExp, Exp argExp)
            : base(expContext)
        {
            SetExpCast(typeExp, argExp);
        }

        public override void SetParent(Exp parentExp)
        {
            ParentExp = parentExp;
            TypeExp.SetParent(this);
            ArgExp.SetParent(this);
        }

        private void SetExpCast(ExpTypeBase typeExp, Exp argExp)
        {
            TypeExp = typeExp;
            ArgExp = argExp;
            caseMethod = calcType.GetMethod("Cast");
        }

        public override Exp Analy( )
        {
            if (this.IsAnalyed) return this;
            if (this.ExpContext == null) throw new CCException();
            this.RetType = TypeExp.RetType;
            IsAnalyed = true;
            return this;
        }

        public override void Emit()
        {
            Type toType = ZTypeUtil.GetTypeOrBuilder(TypeExp.RetType);
            Type argType = ZTypeUtil.GetTypeOrBuilder(ArgExp.RetType);
            if (toType == typeof(int))
            {
                if (argType == typeof(int))
                {
                    ArgExp.Emit();
                    return;
                }
                else if (argType == typeof(float))
                {
                    ArgExp.Emit();
                    IL.Emit(OpCodes.Conv_I);
                    return;
                }
            }
            else if (toType == typeof(float))
            {
                if (argType == typeof(int))
                {
                    ArgExp.Emit();
                    IL.Emit(OpCodes.Conv_R4);
                    return;
                }
                else if (argType == typeof(float))
                {
                    ArgExp.Emit();
                    return;
                }
            }

            caseMethod = MakeCastMethod(TypeExp.RetType);
            ArgExp.RequireType = ZLangBasicTypes.ZOBJECT;
            ArgExp.Emit();
            EmitHelper.CallDynamic(IL, caseMethod);
            base.EmitConv();

        }

        private MethodInfo MakeCastMethod(ZType ztype)
        {
            if(ztype is ZLType)
            {
                return caseMethod.MakeGenericMethod(new Type[] { ((ZLType)ztype).SharpType });
            }
            else
            {
                return caseMethod.MakeGenericMethod(new Type[] { ((ZCClassInfo)ztype).ClassBuilder });
            }
        }

        #region 次要方法属性

        public override Exp[] GetSubExps()
        {
            return new Exp[] { ArgExp };
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append(TypeExp != null ? TypeExp.ToString() : "");
            buf.Append(ArgExp != null ? ArgExp.ToString() : "");
            return buf.ToString();
        }
        
        public override CodePosition Position
        {
            get
            {
                return TypeExp.Position; 
            }
        }
        #endregion
    }
}
