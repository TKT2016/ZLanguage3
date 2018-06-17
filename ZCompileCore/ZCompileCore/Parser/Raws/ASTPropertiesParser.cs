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
    class ASTPropertiesParser
    {
        TokenTape tape;
        ContextFile fileContext;

        public List<PropertyASTRaw> Parse(IEnumerable<LexToken> tokens, ContextFile fileContext)
        {
            this.fileContext = fileContext;
            List<LexToken> tokens2 = new List<LexToken>(tokens);
            tape = new TokenTape(tokens2, fileContext);
            List<PropertyASTRaw> asts = ParseDimList();
            return asts;
        }

        private List<PropertyASTRaw> ParseDimList()
        {
            List<PropertyASTRaw> list = new List<PropertyASTRaw>();
            while (tape.HasCurrent && !tape.Current.IsKind(TokenKindSymbol.EOF))
            {
                if (tape.Current.IsKind(TokenKindKeyword.Ident))
                {
                    var pname = ParseProperty();
                    if (pname != null)
                    {
                        list.Add(pname);
                    }
                }
                else if (tape.Current.IsKind(TokenKindSymbol.Comma))
                {
                    tape.MoveNext();
                }
                else
                {
                    tape.error(string.Format("'{0}'不是正确的属性", tape.Current.Text));
                    tape.MoveNext();
                }         
            }
            return list;
        }

        private PropertyASTRaw ParseProperty()
        {
            PropertyASTRaw property = new PropertyASTRaw();
            //property.FileContext = this.fileMY.FileContext;
            property.NameToken = (LexTokenText)tape.Current;
            tape.MoveNext();
            if (tape.HasCurrent && tape.Current.IsKind(TokenKindSymbol.Assign))
            {
                tape.MoveNext();
                if (tape.HasCurrent)
                {
                    ExpRaw exp = ParseRawExpPropertyValue();
                    if (exp != null)
                    {
                        property.ValueExp = exp;
                    }
                }
            }
            return property;
        }

        private ExpRaw ParseRawExpPropertyValue()
        {
            ExpRaw rexp = new ExpRaw();
            int line = tape.Current.Line;
            while (tape.HasCurrent )
            {
                if (!tape.Current.IsKind(TokenKindSymbol.Comma,TokenKindSymbol.Semi))
                {
                    rexp.RawTokens.Add(tape.Current);
                    tape.MoveNext();
                }
                else
                {
                    break;
                }
            }
            return rexp;
        }
    }
}
