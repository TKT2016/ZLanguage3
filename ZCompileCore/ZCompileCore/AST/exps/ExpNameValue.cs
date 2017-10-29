﻿using ZCompileCore.Lex;
using ZCompileCore.Symbols;
using ZCompileCore.Tools;
using System.Reflection.Emit;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Words;
using System.Reflection;
using ZCompileKit;
using ZCompileKit.Tools;
using ZCompileDesc.ZTypes;

namespace ZCompileCore.AST
{
    public class ExpNameValue : Exp
    {
        public Token NameToken { get; set; }
        public Exp ValueExp { get; set; }
        public string ArgName { get { return NameToken.GetText(); } }

        public ExpNameValue(Token left,Exp right)
        {
            NameToken = left;
            ValueExp = right;
        }

        public override Exp Analy( )
        {
            ValueExp = ValueExp.Analy();
            RetType = ValueExp.RetType;
            return this;
        }

        #region Emit
        public override void Emit()
        {
            ValueExp.Emit();
        }

        #endregion

        #region 覆盖

        public override Exp[] GetSubExps()
        {
            return new Exp[] { ValueExp };
        }

        public override string ToString()
        {
            return NameToken.GetText()+":"+ValueExp.ToString();
        }

        public override CodePosition Position
        {
            get
            {
                return NameToken.Position;
            }
        }
        #endregion
    }
}
