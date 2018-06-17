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
    public class ExpCallUse : ExpCallAnalyedBase
    {
        ZLMethodInfo SearchedMethod;
        List<Exp> newExpArgs;

        public ExpCallUse(ContextExp context, ZMethodCall expProcDesc, ZLMethodInfo zmethod,
            Exp srcExp, List<Exp> argExps):base(context)
        {
            this.ExpProcDesc = expProcDesc;
            this.SearchedMethod = zmethod;
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
            if (SearchedMethod!=null)
            {
                var defArgs = SearchedMethod.ZParams;
                newExpArgs = AnalyArgLambda(defArgs, ArgExps);
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
