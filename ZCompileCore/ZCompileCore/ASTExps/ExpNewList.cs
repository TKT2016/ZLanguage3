using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Tools;
using ZLangRT.Utils;
using ZCompileDesc.Descriptions;
using Z语言系统;
using ZCompileKit.Tools;
using ZCompileDesc.ZMembers;
using ZCompileCore.ASTExps;

namespace ZCompileCore.AST
{
    public class ExpNewList:Exp
    {
        //public ExpTypeUnsure TypeExp { get; set; }
        public ExpTypeBase TypeExp { get; set; }
        public ExpBracket ArgExp { get; set; }

        public ExpNewList(ContextExp context, ExpTypeBase typeExp, ExpBracket argExp)
        {
            this.ExpContext = context;
            TypeExp = typeExp;
            ArgExp = argExp;
        }

        //public ExpNewList(ContextExp context, ExpTypeUnsure typeExp, ExpBracket argExp)
        //{
        //    this.ExpContext = context;
        //    TypeExp = typeExp;
        //    ArgExp = argExp;
        //}

        public override Exp[] GetSubExps()
        {
            return new Exp[] { TypeExp, ArgExp };
        }

        public override Exp Analy( )
        {
            this.RetType = TypeExp.RetType;
            return this;
        }

        public override void Emit()
        {
            var zlistType = TypeExp.RetType.SharpType;
            var Constructor = zlistType.GetConstructor(new Type[]{});// ConstructorDesc.Constructor;
            LocalBuilder varLocal = IL.DeclareLocal(zlistType);
            EmitHelper.NewObj(IL, Constructor);
            IL.Emit(OpCodes.Stloc, varLocal);

            MethodInfo addMethod = zlistType.GetMethod(CompileConst.ZListAddMethodName);//"Add");
            ZMethodInfo exAddMethodInfo = new ZMethodInfo(addMethod);

            foreach (var exp in ArgExp.InneExps)
            {
                EmitHelper.LoadVar(IL, varLocal);//il.Emit(OpCodes.Ldloc, varLocal);
                exp.Emit();
                EmitHelper.CallDynamic(IL, exAddMethodInfo.SharpMethod);
            }
            EmitHelper.LoadVar(IL, varLocal);//il.Emit(OpCodes.Ldloc, varLocal);
            base.EmitConv();
        }

        public override string ToString()
        {
            return TypeExp.ToString() + ArgExp.ToString();
        }
    }
}
