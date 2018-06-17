using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Tools;
using ZLangRT;
using ZCompileDesc.Descriptions;

namespace ZCompileCore.AST.Exps
{
    public class ExpCallSubject : ExpCallAnalyedBase
    {
        Exp SubjectExp;
        ZLMethodInfo SearchedMethod;
        ZLClassInfo SubjectZType;
        List<Exp> newExpArgs;

        public ExpCallSubject(ContextExp context, Exp SubjectExp, ZMethodCall expProcDesc, Exp srcExp, List<Exp> argExps)
            : base(context)
        {
            this.SubjectExp = SubjectExp;
            this.ExpProcDesc = expProcDesc;
            this.SrcExp = srcExp;
            this.ArgExps = argExps;
            foreach (Exp sub in ArgExps)
            {
                sub.ParentExp = this;
            }
        }

        public override Exp Analy( )
        {
            if (this.IsAnalyed) return this;
            if (this.ExpContext == null) throw new CCException();
            if (SubjectExp.RetType is ZLEnumInfo)
            {
                Errorf(this.Position, "约定类型没有过程");
            }
            else
            {
                SubjectZType =  (SubjectExp.RetType as ZLClassInfo);
                var zmethods = SubjectZType.SearchZMethod(ExpProcDesc);
                if (zmethods.Length == 0)
                {
                    Errorf(this.Position, "没有找到对应的过程");
                }
                else
                {
                    SearchedMethod = zmethods[0];
                    var defArgs = SearchedMethod.ZParams;
                    newExpArgs = AnalyArgLambda(defArgs, ArgExps);

                    this.RetType = SearchedMethod.RetZType;
                }
            }
            IsAnalyed = true;
            return this;
        }

        public override void Emit( )
        {
            EmitSubject();
            EmitArgsExp(newExpArgs, SearchedMethod.SharpMethod);
            EmitHelper.CallDynamic(IL,SearchedMethod.SharpMethod);
            EmitConv();
        }

        private void EmitSubject()
        {
            if (!SearchedMethod.GetIsStatic())
            {
                SubjectExp.Emit();
            }
        }

        public override void SetParent(Exp parentExp)
        {
            ParentExp = parentExp;

            this.SrcExp.SetParent(this);
            foreach (var item in ArgExps)
            {
                item.SetParent(this);
            }
        }
    }
}
