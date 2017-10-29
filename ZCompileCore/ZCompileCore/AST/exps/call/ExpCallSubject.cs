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
using ZCompileKit.Tools;
using ZCompileDesc.ZTypes;
using ZCompileDesc.ZMembers;

namespace ZCompileCore.AST
{
    public class ExpCallSubject : ExpCallAnalyedBase
    {
        Exp SubjectExp;
        ZMethodInfo SearchedMethod;
        ZClassType SubjectZType;
        List<Exp> newExpArgs;

        public ExpCallSubject(ContextExp context, Exp SubjectExp, ZCallDesc expProcDesc, Exp srcExp, List<Exp> argExps)
        {
            this.ExpContext = context;
            this.SubjectExp = SubjectExp;
            this.ExpProcDesc = expProcDesc;
            this.SrcExp = srcExp;
            this.ArgExps = argExps;
        }

        public override Exp Analy( )
        {
            if (SubjectExp.RetType is ZEnumType)
            {
                ErrorE(this.Position, "约定类型没有过程");
            }
            else
            {
                SubjectZType =  (SubjectExp.RetType as ZClassType);
                var zmethods = SubjectZType.SearchZMethod(ExpProcDesc);
                //SearchedMethod = SubjectZType.SearchZMethod(ExpProcDesc);
                if (zmethods.Length == 0)
                {
                    ErrorE(this.Position, "没有找到对应的过程");
                }
                else
                {
                    SearchedMethod = zmethods[0];
                    var defArgs = SearchedMethod.ZDesces[0].DefArgs;
                    newExpArgs = AnalyArgLambda(defArgs, ArgExps);

                    this.RetType = SearchedMethod.RetZType;
                }
            }
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
            if (!SearchedMethod.IsStatic)
            {
                SubjectExp.Emit();
            }
        }
    }
}
