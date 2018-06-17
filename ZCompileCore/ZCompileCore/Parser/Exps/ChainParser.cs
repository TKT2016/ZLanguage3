using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST;
using ZCompileCore.Contexts;
using ZCompileCore.AST.Exps;
using ZCompileCore.Lex;
using ZCompileCore.CommonCollections;
using ZCompileNLP;
using System.Diagnostics;

namespace ZCompileCore.Parsers.Exps
{
    public class ChainParser
    {
        ContextExp ExpContext;
        ChainItemParser parser = new ChainItemParser();
    
        public Exp Parse(IEnumerable<object> elements, ContextExp context)
        {
            ExpContext = context;

            object[] objs = parser.ParseItems(elements, context);
            int objSize = objs.Length;
           
            if (objSize  == 0)
            {
                return null;
            }
            else if (objSize == 1)
            {
                return Parse1(objs[0]);
            }
            else if (objSize == 2 && (objs[0] is ExpTypeBase) && IsArg(objs[1]))
            {
                return ParseToExpNew((ExpTypeBase)objs[0], (Exp)objs[1]);
            }
            else
            {
                ExpCall callExp = new ExpCall(this.ExpContext,objs.Select(p => (Exp)p));
                return callExp.Analy();
            }
        }

        private Exp ParseToExpNew(ExpTypeBase expType, Exp exp2)
        {
            ExpBracket bracketBracket = WarpExp(exp2);
            if (bracketBracket.IsExpBracketTagNew())
            {
                bracketBracket = bracketBracket.AnalyToTagNew();
            }
            ExpNew newexp = new ExpNew(exp2.ExpContext,expType, bracketBracket);
            //newexp.SetContextExp(exp2.ExpContext);
            Exp exp3 = newexp.Analy();
            return exp3;
        }

        private Exp Parse1(object data)
        {
            if (data is ExpProcNamePart)
            {
                ExpProcNamePart dataExp = (ExpProcNamePart)data;
                LexToken tok = dataExp.PartNameToken;
                string text = tok.Text;
                var ProcContext = this.ExpContext.ProcContext;
                if (ProcContext.IsThisMethodSingle(text))
                {
                    ExpCallSingleThis pptExp = new ExpCallSingleThis(this.ExpContext,tok);
                    //pptExp.SetContextExp(this.expContext);
                    return pptExp.Analy();
                }
                else if (ProcContext.IsSuperMethodSingle(text))
                {
                    ExpCallSingleSuper pptExp = new ExpCallSingleSuper(this.ExpContext, tok);
                    //pptExp.SetContextExp(this.expContext);
                    return pptExp.Analy();
                }
                else if (ProcContext.IsUseMethodSingle(text))
                {
                    ExpCallSingleUse pptExp = new ExpCallSingleUse(this.ExpContext, tok);
                    //pptExp.SetContextExp(this.expContext);
                    return pptExp.Analy();
                }
                else if (ProcContext.IsUsedProperty(text))
                {
                    ExpUseProperty eupExp = new ExpUseProperty(this.ExpContext, tok);
                    dataExp.CopyFieldsToExp(eupExp);
                    Exp exp2 = eupExp.Analy();
                    return exp2;
                }
                else if (ProcContext.IsUsedField(text))
                {
                    ExpUseField eufExp = new ExpUseField(this.ExpContext,tok);
                    dataExp.CopyFieldsToExp(eufExp);
                    Exp exp2 = eufExp.Analy();
                    return exp2;
                }
                else if (ProcContext.IsUsedEnumItem(text))
                {
                    ExpUseEnumItem eueExp = new ExpUseEnumItem(this.ExpContext, tok);
                    dataExp.CopyFieldsToExp(eueExp);
                    Exp exp2 = eueExp.Analy();
                    return exp2;
                }
                else
                {
                    /* 在赋值表达式中会处理ExpErrorToken */
                    return new ExpErrorToken(dataExp.ExpContext, tok);
                }
            }
            else if (data is Exp)
            {
                Exp exp2 = (Exp)data;
                //exp2.SetContextExp(this.expContext);
                return exp2.Analy();
            }
            else
            {
                return null;
            }
        }

        private bool IsArg(object obj)
        {
            if (!(obj is Exp)) return false;
            if ((obj is ExpError)) return false;
            if ((obj is ExpProcNamePart)) return false;
            return true;
        }

        private ExpBracket WarpExp(Exp exp)
        {
            if (exp is ExpBracket) return exp as ExpBracket;
            ExpBracketWrapOne exp2 = new ExpBracketWrapOne(this.ExpContext,exp, true);
            return exp2;
        }
        
    }
}
