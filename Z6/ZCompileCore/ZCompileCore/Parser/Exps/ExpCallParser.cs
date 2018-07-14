using System;
using System.Collections.Generic;
using ZCompileCore.AST;
using ZCompileCore.AST.Exps;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Reports;
using ZCompileCore.Tools;
using ZCompileDesc.Descriptions;
using ZCompileCore;

namespace ZCompileCore.Parsers.Exps
{
    public class ExpCallParser
    {
        ExpTape tape;
        ContextExp expContext;
        List<Exp> newExpList;
        Exp SrcExp;

        public ExpCallParser( ContextExp expContext,Exp srcExp)
        {
            this.expContext = expContext;
            SrcExp = srcExp;
        }

        public Exp Parse(List<Exp> exps)
        {
            tape = new ExpTape(exps, this.expContext.FileContext);
            newExpList = new List<Exp>();
            while (tape.HasCurrent)
            {
                Exp subExp = AnalyCurrent();
                ParseExp(subExp);
            }

            if (newExpList.Count == 1)
            {
                Exp exp = newExpList[0];
                if (exp is ExpBracket)
                {
                    ExpBracket eb = exp as ExpBracket;
                    return eb.UnBracket();
                }
                else if (!(exp is ExpProcNamePart))
                {
                    return exp;
                }
            }

            ExpCall_Parsed ec = new ExpCall_Parsed(this.expContext, newExpList, SrcExp);
            return ec;
        }

        private Exp ParseItemExp(Exp exp)
        {
            //exp.SetContextExp(expContext);
            Exp subExp = exp.Analy();
            return subExp;
        }

        private void ParseExp(Exp subExp)
        {
            if (subExp == null)
            {

            }
            else if (subExp is ExpProcNamePart)
            {
                ParseNamePart(subExp);
            }
            else if (subExp is ExpBracket)
            {
                ParseBracket(subExp);
            }
            else if (subExp is ExpTypeUnsure)
            {
                ParseType(subExp);
            }
            else if (subExp is ExpVarBase)
            {
                ParseAsArg(subExp);
            }
            else if (subExp is ExpLiteral)
            {
                ParseAsArg(subExp);
            }
            else if (subExp is ExpDe)
            {
                ParseAsArg(subExp);
            }
            else if (subExp is ExpDi)
            {
                ParseAsArg(subExp);
            }
            else if (subExp is ExpEachItem)
            {
                ParseAsArg(subExp);
            }
            else if (subExp is ExpTypeBase)
            {
                ParseAsExpTypeBase(subExp);
            }
            else if (subExp is ExpNew)
            {
                ParseAsArg(subExp);
            }
            else
            {
                throw new CCException();
            }
        }

        private void ParseAsExpTypeBase(Exp subExp)
        {
            tape.MoveNext();
            return;
        }

        private Exp AnalyCurrent()
        {
            Exp exp = tape.Current;
            if (exp != null)
            {
                Exp subExp = ParseItemExp(exp);
                return subExp;
            }
            return null;
        }

        private void ParseType(Exp subExp)
        {
            if (tape.HasCurrent)
            {
                ParseAsArg(subExp);
            }
            else
            {
                ParseNew(subExp);
            }
        }

        private void ParseNew(Exp firstExp)
        {
            var expType = firstExp as ExpTypeUnsure;
            tape.MoveNext();
            Exp nextExp = AnalyCurrent();
            if((nextExp is ExpBracket) )
            {
                var bracketexp = nextExp as ExpBracket;
                AnalyNewExp(expType, bracketexp);
            }
            else if ((nextExp is ExpVarBase) || nextExp is ExpLiteral)
            {
                var bracketexp = new ExpBracketWrapOne(this.expContext, nextExp, false);
                //bracketexp.SetContextExp(this.expContext);
                bracketexp.AnalyRet();
                AnalyNewExp(expType, bracketexp);
            }
            else
            {
                ExpBracket bracket = new ExpBracketWrapOne(this.expContext, firstExp,false);
                //bracket.SetContextExp(this.expContext);
                bracket.AnalyRet();
                newExpList.Add(bracket);
                ParseExp(nextExp);
            }
        }

        private ExpNew AnalyNewExp(ExpTypeUnsure expType, ExpBracket expBracket)
        {
            ExpNew expNew = new ExpNew(this.expContext,expType, expBracket);
           // expNew.SetContextExp(this.expContext);
            Exp expArg = expNew.Analy();
            ParseAsArg(expArg);
            return expNew;
        }

        private void ParseBracket(Exp subExp)
        {
            var bracketexp = subExp as ExpBracket;
            Exp exp2 = bracketexp.Analy();
            newExpList.Add(exp2);
            tape.MoveNext();
        }

        private void ParseNamePart(Exp subExp)
        {
            Exp exp2 = subExp.Analy();
            newExpList.Add(exp2);
            tape.MoveNext();
        }

        private void ParseAsArg(Exp subExp)
        {
            ExpBracket bracket = new ExpBracketWrapOne(this.expContext, subExp, false);
            //bracket.SetContextExp(this.expContext);
            Exp exp2 = bracket.Analy();
            newExpList.Add(exp2);
            tape.MoveNext();
        }

    }
}
