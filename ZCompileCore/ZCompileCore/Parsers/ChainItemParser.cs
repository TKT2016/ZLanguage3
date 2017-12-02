using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileKit.Collections;
using ZNLP;

namespace ZCompileCore.Parsers
{
    public class ChainItemParser
    {
        ContextExp Context;
        ContextProc ProcContext;
        public ChainItemParser(ContextExp context)
        {
            Context = context;
            ProcContext = context.ProcContext;
        }

        public WordCompilePart ParseCompilePart(object obj)
        {
            if (obj is Exp) return WordCompilePart.exp;
            if (obj is LexToken)
            {
                LexToken tok = (LexToken)obj;
                if (tok.IsLiteral) return WordCompilePart.literal;

                string text = tok.GetText();
                if (tok.Kind== TokenKind.DE || text=="的")
                {
                    return WordCompilePart.de;
                }
                else if (tok.Kind == TokenKind.DI || text == "第")
                {
                    return WordCompilePart.di;
                }
                else if (this.ProcContext.IsDefLocal(text))
                {
                    return WordCompilePart.localvar;
                }
                else if (this.ProcContext.ContainArg(text))
                {
                    return WordCompilePart.arg;
                }
                else if (this.ProcContext.IsPropertyDef(text))
                {
                    return WordCompilePart.property_this;
                }
                else if (this.ProcContext.IsPropertyBase(text))
                {
                    return WordCompilePart.property_base;
                }
                else if (this.ProcContext.IsUseEnumItem(text))
                {
                    return WordCompilePart.enumitem_use;
                }
                else if (this.ProcContext.IsUseProperty(text))
                {
                    return WordCompilePart.property_use;
                }
                else if (this.ProcContext.IsCompilingClassName(text))
                {
                    return WordCompilePart.tname_this;
                }
                else if (this.ProcContext.IsImportClassName(text))
                {
                   return WordCompilePart.tname_import;
                }
                else
                {
                    return WordCompilePart.str;
                }
            }
            throw new CCException();
        }

        public WordSpeechPart ParseSpeechPart(object obj)
        {
            throw new CCException();
        }
    }
}
