using System;
using System.Collections.Generic;
using ZCompileCore.AST;
using ZCompileCore.Contexts;
using ZCompileCore.ASTExps;
using ZCompileCore.Lex;
using ZCompileCore.Reports;
using ZCompileCore.Tools;

namespace ZCompileCore.Parsers
{
    public class ExpParser
    {
        TokenTape tape;

        public Exp Parse(List<LexToken> tokens, ContextFile fileContext)
        {
            //if (tokens.Count>5 && tokens[1].GetText() == "X坐标" && tokens[2].GetText() == ">")
            //{
            //    Console.WriteLine("X坐标>战场参数的长度");
            //}
            tape = new TokenTape(tokens, fileContext);
            Exp exp = ParseAssign();
            return exp;
        }

        private Exp ParseAssign()
        {
            Exp leftExp = ParseNameValueExp();

            if (tape.Current.Kind == TokenKind.Assign || tape.Current.Kind == TokenKind.AssignTo)
            {
                ExpAssign assignExp = new ExpAssign();
                assignExp.IsAssignTo = (tape.Current.Kind == TokenKind.AssignTo);
                tape.MoveNext();
                Exp rightExp = ParseBinaryLogicExp();

                if (!assignExp.IsAssignTo)
                {
                    assignExp.ToExp = leftExp;
                    assignExp.ValueExp = rightExp;
                }
                else
                {
                    assignExp.ToExp = rightExp;
                    assignExp.ValueExp = leftExp ;
                }
                return assignExp;
            }
            else
            {
                return leftExp;
            }
        }

        private Exp ParseNameValueExp()
        {
            Exp leftExp = ParseBinaryLogicExp();

            if (tape.Current.Kind == TokenKind.Colon)
            {
                tape.MoveNext();
                Exp rightExp = ParseBinaryLogicExp();
                if (leftExp is ExpVarBase)
                {
                    ExpVarBase leftVarExp = (leftExp as ExpVarBase);
                    LexToken varToken = leftVarExp.VarToken;
                    ExpNameValue expNameValue = new ExpNameValue(varToken, rightExp);
                    return expNameValue;
                }
                else
                {
                    tape.error("调用过程时指定的参数名称只能是标识符");
                    return rightExp;
                }
            }
            else
            {
                return leftExp;
            }
        }

        private Exp ParseBinaryLogicExp()
        {
            LexToken opToken;
            Exp resultExpr = parseCompareExpr();
            while (tape.CurrentKind == TokenKind.AND || tape.CurrentKind == TokenKind.OR)
            {
                opToken = tape.Current;
                tape.MoveNext();
                Exp rightExpr = parseCompareExpr();
                resultExpr = new ExpBinary() { LeftExp = resultExpr, OpToken = opToken, RightExp = rightExpr };
            }
            return resultExpr;
        }

        protected Exp parseCompareExpr()
        {
            LexToken opToken;
            Exp resultExpr = parseAddSub();
            while (TokenKindHelper.IsCompareOp(tape.CurrentKind))
            {
                opToken = tape.Current;
                tape.MoveNext();
                Exp rightExpr = parseAddSub();
                resultExpr = new ExpBinary() { LeftExp = resultExpr, OpToken = opToken, RightExp = rightExpr };
            }
            return resultExpr;
        }

        public Exp parseAddSub()
        {
            LexToken opToken;
            Exp resultExpr = parseMulDiv();
            while (tape.CurrentKind == TokenKind.ADD || tape.CurrentKind == TokenKind.SUB)
            {
                opToken = tape.Current;
                tape.MoveNext();
                Exp rightExpr = parseAddSub();
                resultExpr = new ExpBinary() { LeftExp = resultExpr, OpToken = opToken, RightExp = rightExpr };
            }
            return resultExpr;
        }

        Exp parseMulDiv()
        {
            //report("parseMulDiv");
            LexToken opToken;
            Exp resultExpr = ParseChain();
            while (tape.Current.Kind == TokenKind.MUL || tape.Current.Kind == TokenKind.DIV)
            {
                opToken = tape.Current;
                tape.MoveNext();
                Exp rightExpr = parseMulDiv();
                resultExpr = new ExpBinary() { LeftExp = resultExpr, OpToken = opToken, RightExp = rightExpr };
            }
            return resultExpr;
        }

        private Exp ParseChain()
        {
            ExpChain expChain = new ExpChain();
            //Console.WriteLine(tape.Current.GetText());
            //if (tape.Current.GetText().IndexOf("速度")!=-1)
            //{      
            //    Console.WriteLine("速度");
            //}
            while (tape.Current.Kind != TokenKind.EOF)
            {
                if (tape.Current.Kind == TokenKind.LBS)
                {
                    ExpBracket subExp = parseBracket();
                    expChain.Add(subExp);
                }
                else if (tape.Current.Kind == TokenKind.RBS)
                {
                    break;
                    //tape.error("多余的右括号");
                    //tape.MoveNext();
                }
                else if (tape.Current.IsLiteral)
                {
                    ExpLiteral subExp = parseLiteral();
                    expChain.Add(subExp);
                }
                else if (tape.Current.Kind== TokenKind.Ident
                    || tape.Current.Kind == TokenKind.DE
                    || tape.Current.Kind == TokenKind.DI
                    )
                {
                    LexToken tok = tape.Current;
                    expChain.Add(tok);
                    tape.MoveNext();
                }
                else
                {
                    break;
                }
            }
            if(expChain.SubCount==0)
            {
                return null;
            }
            return expChain;
        }

        private ExpLiteral parseLiteral()
        {
            ExpLiteral literalex = new ExpLiteral();
            literalex.LiteralToken = tape.Current;
            tape.MoveNext();
            return literalex;
        }

        private ExpBracket parseBracket()
        {
            //report("parseBracket");
            ExpBracket bracket = new ExpBracket();
            bracket.LeftBracketToken = tape.Current;
            tape.MoveNext();
            if (!tape.isBracketEnd(tape.Current.Kind))
            {
                while (true)
                {
                    Exp expr = ParseAssign();
                    if (expr != null)
                    {
                        bracket.InneExps.Add(expr);
                    }
                    if (tape.isBracketEnd(tape.CurrentKind))
                    {
                        break;
                    }
                    if (tape.CurrentKind != TokenKind.Comma)
                    {
                        tape.error("多余的表达式元素");
                        tape.MoveNext();
                        while (!(tape.isBracketEnd(tape.CurrentKind)) && tape.CurrentKind != TokenKind.Comma)
                        {
                            tape.MoveNext();
                        }
                    }
                    else if (tape.CurrentKind == TokenKind.Comma)
                    {
                        tape.MoveNext();
                    }
                }
            }
            if (tape.CurrentKind == TokenKind.RBS)
            {
                bracket.RightBracketToken = tape.Current;
                tape.MoveNext();
            }
            else
            {
                tape.error("括号不匹配");
            }
            return bracket;
        }

    }
}
