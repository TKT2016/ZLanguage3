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

namespace ZCompileCore.ASTExps
{
    public class ExpCallUse : ExpCallAnalyedBase
    {
        ZLMethodInfo SearchedMethod;
        List<Exp> newExpArgs;

        public ExpCallUse(ContextExp context, ZMethodCall expProcDesc, ZLMethodInfo zmethod, Exp srcExp, List<Exp> argExps)
        {
            this.ExpContext = context;
            this.ExpProcDesc = expProcDesc;
            this.SearchedMethod = zmethod;
            this.SrcExp = srcExp;
            this.ArgExps = argExps;
        }

        public override Exp Analy( )
        {
            if (this.IsAnalyed) return this;
            if (SearchedMethod!=null)
            {
                var defArgs = SearchedMethod.ZParams;//.ZDesces[0].DefArgs;
                newExpArgs = AnalyArgLambda(defArgs, ArgExps);
                //AnalyArgLambda(SearchedMethod.ZDesces[0], ArgExps);
                //ArgExps = newExpArg;
            }
            RetType = SearchedMethod.RetZType;
            IsAnalyed = true;
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
