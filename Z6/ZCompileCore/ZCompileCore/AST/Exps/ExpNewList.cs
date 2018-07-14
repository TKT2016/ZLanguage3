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
using ZCompileCore.AST.Exps;

namespace ZCompileCore.AST
{
    public class ExpNewList:Exp
    {
        //public ExpTypeUnsure TypeExp { get; set; }
        private ExpTypeBase _TypeExp;
        public ExpTypeBase TypeExp { get { return _TypeExp; } set { _TypeExp = value; _TypeExp.ParentExp = this; } }

        private Exp _ArgExp;
        public Exp ArgExp { get { return _ArgExp; } set { _ArgExp = value; _ArgExp.ParentExp = this; } }

        public ExpNewList(ContextExp context, ExpTypeBase typeExp, ExpBracket argExp):base(context)
        {
            TypeExp = typeExp;
            ArgExp = argExp;
        }

        public override void SetParent(Exp parentExp)
        {
            ParentExp = parentExp;
            TypeExp.SetParent(this);
            ArgExp.SetParent(this);
        }

        public override Exp[] GetSubExps()
        {
            return new Exp[] { TypeExp, ArgExp };
        }

        public override Exp Analy( )
        {
            if (this.IsAnalyed) return this;
            this.RetType = TypeExp.RetType;
            IsAnalyed = true;
            return this;
        }

        public override void Emit()
        {
            var zlistType = ((ZLType)TypeExp.RetType).SharpType;
            var Constructor = zlistType.GetConstructor(new Type[]{});// ConstructorDesc.Constructor;
            LocalBuilder varLocal = IL.DeclareLocal(zlistType);
            EmitHelper.NewObj(IL, Constructor);
            IL.Emit(OpCodes.Stloc, varLocal);

            MethodInfo addMethod = zlistType.GetMethod(ZLangUtil.ZListAddMethodName);//"Add");
            //ZLMethodInfo exAddMethodInfo = new ZLMethodInfo(addMethod);

            foreach (var exp in ArgExp.GetSubExps())
            {
                EmitHelper.LoadVar(IL, varLocal);//il.Emit(OpCodes.Ldloc, varLocal);
                exp.Emit();
                EmitHelper.CallDynamic(IL, addMethod); //EmitHelper.CallDynamic(IL, exAddMethodInfo.SharpMethod);
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
