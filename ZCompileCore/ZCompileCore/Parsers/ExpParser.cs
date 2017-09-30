using System;
using System.Collections.Generic;
using ZCompileCore.AST;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Reports;
using ZCompileCore.Tools;
using ZCompileDesc.Words;

namespace ZCompileCore.Parsers
{
    public class ExpParser//:ParserBase
    {
        TokenTape tape;

        public Exp Parse(List<Token> tokens, ContextFile fileContext)
        {
            tape = new TokenTape(tokens, fileContext);
            Exp exp = ParseAssign();
            return exp;
        }

        private Exp ParseNameValueExp()
        {
            Exp leftExp = ParseBinaryLogicExp();

            if (tape.Current.Kind == TokenKind.Colon)
            {
                tape.MoveNext();
                Exp rightExp = ParseBinaryLogicExp ();
                if(leftExp is ExpVar)
                {
                    ExpVar leftVarExp = (leftExp as ExpVar);
                    Token varToken = leftVarExp.VarToken;
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

        private Exp ParseBinaryLogicExp()
        {
            Token opToken;
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
            Token opToken;
            Exp resultExpr = parseAddSub();
            //while (CurrentToken.Kind == TokenKind.GT || CurrentToken.Kind == TokenKind.LT || CurrentToken.Kind == TokenKind.GE
            //    || CurrentToken.Kind == TokenKind.LE || CurrentToken.Kind == TokenKind.NE || CurrentToken.Kind == TokenKind.EQ)
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
            Token opToken;
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
            Token opToken;
            Exp resultExpr = parseCall();
            while (tape.Current.Kind == TokenKind.MUL || tape.Current.Kind == TokenKind.DIV)
            {
                opToken = tape.Current;
                tape.MoveNext();
                Exp rightExpr = parseMulDiv();
                resultExpr = new ExpBinary() { LeftExp = resultExpr, OpToken = opToken, RightExp = rightExpr };
            }
            return resultExpr;
        }

        Exp parseCall()
        {
            //report("parseCall");
            List<Exp> exps = new List<Exp>();
            while (tape.Current.Kind == TokenKind.Ident || tape.Current.Kind == TokenKind.LBS || TokenKindHelper.IsLiteral(tape.Current.Kind))//|| CurrentKind == TokenKind.RBS
            {
                Exp term = parseChain();
                exps.Add(term);
            }
            if (exps.Count == 2 && (exps[0] is ExpType) && (exps[1] is ExpBracket))
            {
                ExpNew expNew = new ExpNew();
                expNew.TypeExp = (exps[0] as ExpType);
                expNew.BracketExp = (exps[1] as ExpBracket);
                return expNew;
            }
            else if (exps.Count == 0)
            {
                return null;
            }
            else if (exps.Count == 1 && !(exps[0] is ExpProcNamePart))
            {
                return exps[0];
            }
            else
            {
                ExpCall callExp = new ExpCall();
                callExp.Elements = exps;
                return callExp;
            }
        }

        Exp parseChain()
        {
            //report("parseChain");
            Exp leftExp = parseTerm();
            if (leftExp == null) return null;

            while (tape.Current.Kind != TokenKind.EOF)
            {
                if (tape.Current.Kind == TokenKind.DE)
                {
                    ExpDe deExp = new ExpDe();
                    deExp.KeyToken = tape.Current;
                    deExp.LeftExp = leftExp;
                    tape.MoveNext();
                    Exp rightExp = parseTerm();
                    deExp.RightExp = rightExp;
                    leftExp = deExp;
                }
                else if (tape.Current.Kind == TokenKind.DI)
                {
                    ExpDi diExp = new ExpDi();
                    diExp.KeyToken = tape.Current;
                    diExp.SubjectExp = leftExp;
                    tape.MoveNext();
                    Exp rightExp = parseTerm();
                    diExp.ArgExp = rightExp;
                    leftExp = diExp;
                }
                else
                {
                    return leftExp;
                }
            }
            return leftExp;
        }

        Exp parseTerm()
        {
            //report("parseTerm");
            Exp leftExp = null;
            if (TokenKindHelper.IsLiteral(tape.CurrentKind))// CurrentToken.IsLiteral())
            {
                leftExp = parseLiteral();
            }
            else if (tape.CurrentKind == TokenKind.Each)
            {
                ExpEachWord varExp = new ExpEachWord();
                varExp.EachToken = tape.Current;
                tape.MoveNext();
                return varExp;
            }
            else if (tape.Current.IsTypeName())
            {
                leftExp = parseTypeExp();
                return leftExp;
            }
            else if (tape.Current.IsProcPart())
            {
                leftExp = parseProcNamePart();
                return leftExp;
            }
            else if (tape.Current.IsVarName())
            {
                leftExp = parseVarExp();
                return leftExp;
            }
            else if (tape.Current.Kind == TokenKind.Ident)
            {
                leftExp = parseVarExp();
                return leftExp;
            }
            else if (tape.Current.Kind == TokenKind.LBS)
            {
                leftExp = parseBracket();
                return leftExp;
            }
            else if (tape.Current.Kind == TokenKind.RBS)
            {
                tape.error("多余的右括号");
                tape.MoveNext();
                return null;
            }
            else if (tape.Current.WKind == WordKind.Unkown)
            {
                var currentText = tape.Current.GetText();
                tape.error("无法识别'" + currentText + "'");
                tape.MoveNext();
                return null;
            }
            return leftExp;
        }

        ExpBracket parseBracket()
        {
             //report("parseBracket");
             ExpBracket bracket = new ExpBracket();
             bracket.LeftBracketToken = tape.Current;
             tape.MoveNext();
             if (!tape.isBracketEnd(tape.Current.Kind))
             {
                 while (true)
                 {
                     Exp expr = ParseAssign();// ParseBinaryLogicExp();
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
                     if (tape.CurrentKind == TokenKind.Comma)
                     {
                         tape.MoveNext();
                     }
                 }
             }
            if(tape.CurrentKind== TokenKind.RBS)
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

        ExpProcNamePart parseProcNamePart()
        {
            //report("parseVarExp");
            ExpProcNamePart varExp = new ExpProcNamePart();
            varExp.PartNameToken = tape.Current;
            tape.MoveNext();
            return varExp;
        }

        ExpVar parseVarExp()
        {
            //report("parseVarExp");
            ExpVar varExp = new ExpVar();
            varExp.VarToken = tape.Current;
            tape.MoveNext();
            return varExp;
        }

        ExpType parseTypeExp()
        {
            //report("parseTypeExp");
            ExpType typeExp = new ExpType();
            while (tape.Current.Kind != TokenKind.EOF)
            {
                if (tape.Current.IsTypeName())
                {
                    typeExp.TypeTokens.Add(tape.Current);
                    tape.MoveNext();
                    if(tape.Pre.WKind== WordKind.GenericClassName)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }  
            }
            return typeExp;
        }

        Exp parseLiteral()
        {
            ExpLiteral literalex = new ExpLiteral();
            literalex.LiteralToken = tape.Current;
            tape.MoveNext();
            return literalex;
        }
    }
}
