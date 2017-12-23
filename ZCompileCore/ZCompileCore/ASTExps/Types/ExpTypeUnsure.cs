using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;

using ZLangRT.Utils;
using ZCompileDesc.Descriptions;
using ZCompileDesc;
using ZCompileDesc.Utils;

using ZCompileCore.ASTExps;
using ZCompileCore.AST;

namespace ZCompileCore.ASTExps
{
    public class ExpTypeUnsure: ExpTypeBase
    {
        public List<LexToken> TypeTokens { get; set; }

        public ExpTypeUnsure(List<LexToken> toks)
        {
            TypeTokens = toks;

        }
        public ExpTypeUnsure()
        {
            TypeTokens = new List<LexToken>();
        }

        public override LexToken GetMainToken()
        {
            tsize = TypeTokens.Count;
            return TypeTokens[tsize - 1]; ;
        }

        public override void Emit()
        {
            base.Emit();
        }

        public LexToken ToSingleToken()
        {
            string newText = string.Join("", TypeTokens.Select(P => P.GetText()));
            LexToken firstToken = TypeTokens[0];
            LexToken newToken = new LexToken(newText, firstToken.Kind, firstToken.Line, firstToken.Col);
            return newToken;
        }

         LexToken mainTypeToken;
         ZType mainZType;
         int tsize;
         ExpTypeNone noneType; 

         public override Exp Analy()
         {
             if (this.ExpContext == null) throw new CCException();
             //ExpStaticClassName expStaticClassName = null;
             if (TypeTokens.Count==1)
             {
                 var expStaticClassName = ParseExpStaticClass(TypeTokens[0]);
                 if(expStaticClassName!=null )
                 {
                     return expStaticClassName.Analy();
                 }
             }
             noneType = new ExpTypeNone(this);
            tsize = TypeTokens.Count;
            if (tsize == 0) return noneType;
            mainTypeToken = TypeTokens[tsize - 1];
            mainZType = SearchZType(mainTypeToken);
            if (mainZType == noneType)
            {
                return noneType;
            }

            if(ZTypeUtil.IsGenericType(mainZType))// (mainZType.SharpType.IsGenericType)
            {
                return AnalyGeneric();     
            }
            else
            {
                return AnalyNormal();
            }
        }

         private ExpStaticClassName ParseExpStaticClass(LexToken tok)
         {
             string name = tok.GetText();
             var ztypes = this.ExpContext.FileContext.ImportUseContext.SearchImportType(name);
             if (ztypes.Length == 0) return null;
             ZLClassInfo ztype = ztypes[0] as ZLClassInfo;
             if (ztype == null) return null;
             if (!ztype.IsStatic) return null;
             ExpStaticClassName expStatic = new ExpStaticClassName(tok, ztype);
             expStatic.SetContext(this.ExpContext);
             return expStatic;
         }

        private Exp AnalyGeneric()
        {
            int count = GenericUtil.GetGenericTypeArgCount(ZTypeUtil.GetTypeOrBuilder(mainZType));// (mainZType.SharpType);
            if (tsize > count + 1)
            {
                ErrorF(mainTypeToken.Position, "泛型类型'{0}'声明类型过多", mainTypeToken.GetText());
            }
            else if (tsize < count + 1)
            {
                ErrorF(mainTypeToken.Position, "泛型类型'{0}'缺少参数类型声明", mainTypeToken.GetText());
            }
            else
            {
                if(count==1)
                {
                   return AnalyGeneric_1();
                }
                else if (count == 2)
                {
                   return AnalyGeneric_2();
                }
                else
                {
                    throw new ZLibRTException("Z语言不支持泛型参数超过2的泛型");
                }
            }
            return noneType;
        }

        private Exp AnalyGeneric_2()
        {
            LexToken genericArgTypeToken1 = TypeTokens[0];
            LexToken genericArgTypeToken2 = TypeTokens[1];
            ZType genericArgZType1 = SearchZType(genericArgTypeToken1);
            ZType genericArgZType2 = SearchZType(genericArgTypeToken2);

            if (genericArgZType1 != null && genericArgZType2 != null)
            {
                //Type newType = MakeGenericType(mainZType, genericArgZType1, genericArgZType2);
                //ZType newZtype = ZTypeManager.RegNewGenericType(newType);
                //RetType = newZtype;
                ZLClassInfo newZClass = ZTypeManager.MakeGenericType((ZLClassInfo)mainZType, genericArgZType1, genericArgZType2);
                ExpTypeThree twoExp = new ExpTypeThree(TypeTokens[0], TypeTokens[1], TypeTokens[2], genericArgZType1, genericArgZType2, mainZType, newZClass);
                return twoExp.Analy();
            }
            return noneType;
        }

        private Exp AnalyGeneric_1()
        {
            LexToken genericArgTypeToken = TypeTokens[0];
            ZType genericArgZType = SearchZType(genericArgTypeToken);
            if (genericArgZType != null)
            {
                //Type newType = MakeGenericType(mainZType, genericArgZType); 
                //ZType newZtype = ZTypeManager.RegNewGenericType(newType);
                ZLClassInfo newZClass = ZTypeManager.MakeGenericType((ZLClassInfo)mainZType, genericArgZType);
                ExpTypeTwo twoExp = new ExpTypeTwo(TypeTokens[0], TypeTokens[1], genericArgZType, mainZType, newZClass);
                return twoExp.Analy();
            }
            return noneType;
        }

        //private Type MakeGenericType(ZType mainType,params ZType[] argZTypes)
        //{
        //    if(mainType is ZLType)
        //    {
        //        var args = argZTypes.Select(U=>ZTypeUtil.GetTypeOrBuilder(U)).ToArray();
        //        return ((ZLType)mainZType).SharpType.MakeGenericType(args);
        //    }
        //    else
        //    {
        //        throw new CCException();
        //    }
        //}

        private Exp AnalyNormal()
        {
            string text = mainTypeToken.GetText();
            if(this.ProcContext.IsCompilingClassName(text))
            {
                ExpTypeCompiling etc = new ExpTypeCompiling(mainTypeToken);
                etc.SetContext(this.ExpContext);
                return etc.Analy();
            }
            else if (this.ProcContext.IsImportClassName(text))
            {
                ExpTypeSingle ets = new ExpTypeSingle(mainTypeToken);
                ets.SetContext(this.ExpContext);
                return ets.Analy();
            }
            else
            {
                ErrorF(mainTypeToken.Position, "找不到'{0}'类型", text);
                return new ExpErrorType(this.ExpContext, mainTypeToken);
            }
            //ExpTypeOne oneExp = new ExpTypeOne(mainTypeToken,mainZType);
            //if(tsize!=1)
            //{
            //    ErrorE(mainTypeToken.Position, "类型'{0}'不是泛型类型", mainTypeToken.GetText());
            //}
            //return oneExp.Analy();
        }

        private  ZType SearchZType(LexToken token)
        {
            string typeName = token.GetText();
            var ztypes = this.ExpContext.FileContext.ImportUseContext.SearchImportType(typeName);
            ZType ztype = ztypes[0] as ZType;
            if(ztype==null)
            {
                ErrorF(token.Position, "类型'{0}'不存在", token.GetText());
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
