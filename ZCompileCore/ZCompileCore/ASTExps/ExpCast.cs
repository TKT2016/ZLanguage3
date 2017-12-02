using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Lex;
using ZCompileCore.Symbols;
using ZLangRT.Utils;
using ZCompileDesc.Descriptions;
using ZCompileKit;
using ZCompileKit.Tools;
using ZCompileDesc.ZMembers;
using ZCompileDesc.ZTypes;
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
            if (this.ExpContext == null) throw new CCException();
            this.RetType = TypeExp.RetType;
            return this;
        }

        public override void Emit( )
        {
            caseMethod = caseMethod.MakeGenericMethod(new Type[] { TypeExp.RetType.SharpType });
            ArgExp.RequireType= ZLangBasicTypes.ZOBJECT;// (typeof(object);
            ArgExp.Emit();
            EmitHelper.CallDynamic(IL, caseMethod);
            //IL.Emit(OpCodes.Unbox_Any);
            base.EmitConv();
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
