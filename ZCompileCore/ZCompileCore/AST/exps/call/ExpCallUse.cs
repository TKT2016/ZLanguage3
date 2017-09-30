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
using ZCompileDesc.ZMembers;

namespace ZCompileCore.AST
{
    public class ExpCallUse : ExpCallAnalyedBase
    {
        ZMethodInfo SearchedMethod;
        List<Exp> newExpArgs;

        public ExpCallUse(ContextExp context, ZCallDesc expProcDesc, ZMethodInfo zmethod, Exp srcExp, List<Exp> argExps)
        {
            this.ExpContext = context;
            this.ExpProcDesc = expProcDesc;
            this.SearchedMethod = zmethod;
            this.SrcExp = srcExp;
            this.ArgExps = argExps;
        }

        public override Exp Analy( )
        {
            if (SearchedMethod!=null)
            {
                var defArgs = SearchedMethod.ZDesces[0].DefArgs;
                newExpArgs = AnalyArgLambda(defArgs, ArgExps);
                //AnalyArgLambda(SearchedMethod.ZDesces[0], ArgExps);
                //ArgExps = newExpArg;
            }
            RetType = SearchedMethod.RetZType;
            return this;
        }

        public override void Emit()
        {
            EmitSubject();
            EmitArgsExp(newExpArgs, SearchedMethod); 
            EmitHelper.CallDynamic(IL, SearchedMethod.SharpMethod);
            EmitConv(); 
        }

        private void EmitSubject()//必须为Static
        {
            return;
        }
    }
}
