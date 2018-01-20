using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Lex;

using ZLangRT.Utils;
using ZCompileDesc.Descriptions;
using ZCompileKit;
using ZCompileKit.Tools;
using ZLangRT;
using System;
using ZCompileDesc;
using ZCompileCore.ASTExps;

namespace ZCompileCore.AST
{
    public class ExpCast : Exp
    {
        public ExpTypeBase TypeExp { get; set; }
        public Exp ArgExp { get; set; }

        Type calcType = typeof(Calculater);
        MethodInfo caseMethod;

        public ExpCast(ExpTypeBase typeExp, Exp argExp)
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

        public override void Emit( )
        {
            caseMethod = MakeCastMethod(TypeExp.RetType);// caseMethod.MakeGenericMethod(new Type[] { TypeExp.RetType.SharpType });
            ArgExp.RequireType= ZLangBasicTypes.ZOBJECT;// (typeof(object);
            ArgExp.Emit();
            EmitHelper.CallDynamic(IL, caseMethod);
            //IL.Emit(OpCodes.Unbox_Any);
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
