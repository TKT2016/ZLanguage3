using System;
using System.Collections.Generic;
using ZCompileCore.AST;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Reports;
using ZCompileCore.Tools;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Words;
using ZCompileKit;

namespace ZCompileCore.Parsers
{
    public class ExpCallParser
    {
        ExpTape tape;
        ContextExp expContext;
        List<Exp> newExpList;
        Exp SrcExp;

        public ExpCallParser(List<Exp> exps, ContextExp expContext,Exp srcExp)
        {
            this.expContext = expContext;
            tape = new ExpTape(exps, this.expContext.FileContext);
            SrcExp = srcExp;
        }

        public Exp Parse()
        {
            newExpList = new List<Exp>();
            while (!tape.IsEnd)
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

            ExpCall_Parsed ec = new ExpCall_Parsed(newExpList, this.expContext, SrcExp);
            return ec;
        }

        private Exp ParseItemExp(Exp exp)
        {
            exp.SetContext(expContext);
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
            else if (subExp is ExpType)
            {
                ParseType(subExp);
            }
            else if (subExp is ExpVar)
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
            else
            {
                throw new CompileCoreException();
            }
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
            if (tape.IsEnd)
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
            var expType = firstExp as ExpType;
            tape.MoveNext();
            Exp nextExp = AnalyCurrent();
            if(nextExp is ExpBracket)
            {
                var bracketexp = nextExp as ExpBracket;
                ExpNew expNew = new ExpNew(expType,bracketexp);
                expNew.SetContext(this.expContext);
                Exp expArg = expNew.Analy();
                ParseAsArg(expArg);
            }
            else if ((nextExp is ExpVar) || nextExp is ExpLiteral)
            {
                var bracketexp = new ExpBracket(nextExp);
                bracketexp.SetContext(this.expContext);
                ExpNew expNew = new ExpNew(expType, bracketexp);
                expNew.SetContext(this.expContext);
                bracketexp.AnalyRet();
                ParseAsArg(bracketexp);
            }
            else
            {
                ExpBracket bracket = new ExpBracket(firstExp);
                bracket.SetContext(this.expContext);
                bracket.AnalyRet();
                newExpList.Add(bracket);
                ParseExp(nextExp);
            }
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
            ExpBracket bracket = new ExpBracket(subExp);
            bracket.SetContext(this.expContext);
            Exp exp2 = bracket.Analy();
            newExpList.Add(exp2);
            tape.MoveNext();
        }

    }
}
