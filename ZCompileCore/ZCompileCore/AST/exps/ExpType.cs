using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Symbols;
using ZLangRT.Utils;
using ZCompileDesc.Descriptions;
using ZCompileDesc;
using ZCompileDesc.Utils;
using ZCompileDesc.ZTypes;

namespace ZCompileCore.AST
{
    public class ExpType:Exp
    {
        public List<Token> TypeTokens { get; set; }

        public ExpType()
        {
            TypeTokens = new List<Token>();
        }

        public override void Emit()
        {
            base.Emit();
        }

        public Token ToSingleToken()
        {
            string newText = string.Join("", TypeTokens.Select(P => P.GetText()));
            Token firstToken = TypeTokens[0];
            Token newToken = new Token(newText, firstToken.Kind, firstToken.Line, firstToken.Col);
            return newToken;
        }

        Token mainTypeToken;
        ZType mainZType;
        int tsize;

        public override Exp Analy( )
        {
            tsize = TypeTokens.Count;
            mainTypeToken = TypeTokens[tsize-1];
            mainZType = SearchZType(mainTypeToken);
            if(mainZType==null)
            {
                return this; 
            }

            if (mainZType.SharpType.IsGenericType)
            {
                AnalyGeneric();     
            }
            else
            {
                AnalyNormal();
            }
            return this;
        }

        private void AnalyGeneric()
        {
            RetType = mainZType;
            int count = GenericUtil.GetGenericTypeArgCount(mainZType.SharpType);
            if (tsize > count + 1)
            {
                ErrorE(mainTypeToken.Position, "泛型类型'{0}'声明类型过多", mainTypeToken.GetText());
            }
            else if (tsize < count + 1)
            {
                ErrorE(mainTypeToken.Position, "泛型类型'{0}'缺少参数类型声明", mainTypeToken.GetText());
            }
            else
            {
                if(count==1)
                {
                    AnalyGeneric_1();
                }
                else if (count == 2)
                {
                    AnalyGeneric_2();
                }
                else
                {
                    throw new ZLibRTException("Z语言不支持泛型参数超过2的泛型");
                }
            }
        }

        private void AnalyGeneric_2()
        {
            Token genericArgTypeToken1 = TypeTokens[0];
            Token genericArgTypeToken2 = TypeTokens[1];
            ZType genericArgZType1 = SearchZType(genericArgTypeToken1);
            ZType genericArgZType2 = SearchZType(genericArgTypeToken2);

            if (genericArgZType1 != null && genericArgZType2 != null)
            {
                Type newType = mainZType.SharpType.MakeGenericType(genericArgZType1.SharpType, genericArgZType2.SharpType);
                ZType newZtype = ZTypeManager.RegNewGenericType(newType);
                RetType = newZtype;
            }
        }

        private void AnalyGeneric_1()
        {
            Token genericArgTypeToken = TypeTokens[0];
            ZType genericArgZType = SearchZType(genericArgTypeToken);
            if (genericArgZType != null)
            {
                Type newType = mainZType.SharpType.MakeGenericType(genericArgZType.SharpType);
                ZType newZtype = ZTypeManager.RegNewGenericType(newType);
                RetType = newZtype;
            }
        }

        private void AnalyNormal()
        {
            RetType = mainZType;
            if(tsize!=1)
            {
                ErrorE(mainTypeToken.Position, "类型'{0}'不是泛型类型", mainTypeToken.GetText());
            }
        }

        private ZType SearchZType(Token token)
        {
            string typeName = token.GetText();
            var ztypes = this.ExpContext.FileContext.SearchZDescType(typeName);
            ZType ztype = ztypes[0] as ZType;
            if(ztype==null)
            {
                ErrorE(token.Position, "类型'{0}'不存在", token.GetText());
            }
            return ztype;
        }

        public override Exp[] GetSubExps()
        {
            return new Exp[] { };
        }

        #region 覆盖

        public override string ToString()
        {
            return string.Join("", TypeTokens.Select(p=>p.GetText()));
        }

        public override CodePosition Position
        {
            get
            {
                return TypeTokens[0].Position;
            }
        }
        #endregion
    }
}
