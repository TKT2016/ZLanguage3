using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST;
using ZCompileCore.ASTRaws;
using ZCompileCore.CommonCollections;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;

namespace ZCompileCore.Parsers.Raws
{
    class ASTProcNameParser
    {
        TokenTape tape;
        ContextFile fileContext;
        SectionProcRaw ast;

        public SectionProcRaw Parse(IEnumerable<LexToken> tokens, ContextFile fileContext, SectionProcRaw procAST)
        {
            this.fileContext = fileContext;
            this.ast = procAST;

            List<LexToken> tokens2 = new List<LexToken>(tokens);
            tape = new TokenTape(tokens2.ToArray(), fileContext); 
            ast.NamePart = ParseProcName();
            tape.Match(TokenKindSymbol.Colon);
            ast.RetToken = ParseRetProc();

            return ast;
        }

        private LexTokenText ParseRetProc()
        {
            if (tape.HasCurrent && tape.Current.IsKind(TokenKindKeyword.Ident))
            {
                LexTokenText headToken = (LexTokenText)tape.Current;
                tape.MoveNext();
                //if (tape.CurrentKind == TokenKindSymbol.NewLine)
                //{
                //    SkipNewLine();
                //}
                return headToken;
            }
            return null;
        }

        private ProcNameRaw ParseProcName()
        {
            ProcNameRaw procName = new ProcNameRaw();
            while (tape.HasCurrent)
            {
                if (tape.Current.IsKind( TokenKindSymbol.LBS))
                {
                    var bck = ParseProcBracket();
                    if (bck != null)
                    {
                        procName.NameTerms.Add(bck);
                    }
                }
                else if (tape.Current.IsKind(TokenKindKeyword.Ident))
                {
                    ProcNameRaw.NameText pt = new ProcNameRaw.NameText();
                    pt.TextToken = (LexTokenText)tape.Current;
                    procName.NameTerms.Add(pt);
                    tape.MoveNext();
                }
                else if (tape.Current.IsKind(TokenKindSymbol.Colon))
                {
                    break;
                }
                else
                {
                    tape.error("错误的过程名称");
                    break;
                }
            }
            return procName;
        }

        private ProcNameRaw.NameBracket ParseProcBracket()
        {
            ProcNameRaw.NameBracket result = new ProcNameRaw.NameBracket();
            //result.FileContext = this.fileMY.FileContext;
            result.LeftBracketToken = (LexTokenSymbol)tape.Current;
            tape.MoveNext();
            //tape.Match(TokenKindSymbol.LBS);
            while (!tape.Current.IsKind( TokenKindSymbol.RBS))
            {
                if (tape.Current.IsKind(TokenKindKeyword.Ident))
                {
                    ProcNameRaw.ProcParameter arg = ParseProcArg();
                    if (arg != null)
                    {
                        result.Parameters.Add(arg);
                    }
                    //arg.ArgToken = tape.Current;
                    //tape.MoveNext();
                }
                else if (tape.Current.IsKind(TokenKindSymbol.Comma))
                {
                    tape.MoveNext();
                }
                else
                {
                    tape.error("定义过程的括号中不应该出现");
                    tape.MoveNext();
                }
            }
            result.RightBracketToken = (LexTokenSymbol)tape.Current;
            tape.MoveNext();
            //tape.Match(TokenKindSymbol.RBS);
            return result;
        }

        private ProcNameRaw.ProcParameter ParseProcArg()
        {
            ProcNameRaw.ProcParameter arg = new ProcNameRaw.ProcParameter();
            //arg.FileContext = this.fileMY.FileContext;
            if (tape.Current.IsKind(TokenKindKeyword.Ident))
            {
                arg.ParameterToken = (LexTokenText)tape.Current;
                tape.MoveNext();
            }
            if (arg.ParameterToken == null) return null;
            return arg;
        }
    }
}
