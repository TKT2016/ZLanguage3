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

using ZCompileCore.AST.Exps;
using ZCompileCore.AST;

namespace ZCompileCore.AST.Exps
{
    public class ExpTypeUnsure: ExpTypeBase
    {
        public List<LexToken> TypeTokens { get; set; }

        public ExpTypeUnsure(ContextExp expContext, List<LexToken> toks)
            : base(expContext)
        {
            TypeTokens = toks;
        }

        public ExpTypeUnsure(ContextExp expContext)
            : base(expContext)
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
            string newText = string.Join("", TypeTokens.Select(P => P.Text));
            LexToken firstToken = TypeTokens[0];
            LexTokenText newToken = new LexTokenText( firstToken.Line, firstToken.Col,newText);//firstToken.Kind,
            return newToken;
        }

         LexToken mainTypeToken;
         ZType mainZType;
         int tsize;
         ExpTypeNone noneType; 

         public override Exp Analy()
         {
             if (this.IsAnalyed) return this;
             if (this.ExpContext == null) throw new CCException();
             if (TypeTokens.Count==1)
             {
                 var expStaticClassName = ParseExpStaticClass(TypeTokens[0]);
                 if(expStaticClassName!=null )
                 {
                     return expStaticClassName.Analy();
                 }
             }
             noneType = new ExpTypeNone(this.ExpContext, this);
            tsize = TypeTokens.Count;
            if (tsize == 0) return noneType;
            mainTypeToken = TypeTokens[tsize - 1];
            mainZType = SearchZType(mainTypeToken);
            if (mainZType == noneType)
            {
                return noneType;
            }

            IsAnalyed = true;
            if(ZTypeUtil.IsGenericType(mainZType))
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
             string name = tok.Text;
             var ztypes = this.ExpContext.FileContext.ImportUseContext.SearchImportType(name);
             if (ztypes.Length == 0) return null;
             ZLClassInfo ztype = ztypes[0] as ZLClassInfo;
             if (ztype == null) return null;
             if (!ztype.IsStatic) return null;
             ExpStaticClassName expStatic = new ExpStaticClassName(this.ExpContext, tok, ztype);
             //expStatic.SetContextExp(this.ExpContext);
             return expStatic;
         }

        private Exp AnalyGeneric()
        {
            int count = GenericUtil.GetGenericTypeArgCount(ZTypeUtil.GetTypeOrBuilder(mainZType));// (mainZType.SharpType);
            if (tsize > count + 1)
            {
                Errorf(mainTypeToken.Position, "泛型类型'{0}'声明类型过多", mainTypeToken.Text);
            }
            else if (tsize < count + 1)
            {
                Errorf(mainTypeToken.Position, "泛型类型'{0}'缺少参数类型声明", mainTypeToken.Text);
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
                ExpTypeThree twoExp = new ExpTypeThree( this.ExpContext, TypeTokens[0], TypeTokens[1], TypeTokens[2], genericArgZType1, genericArgZType2, mainZType, newZClass);
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
                ExpTypeTwo twoExp = new ExpTypeTwo(this.ExpContext, TypeTokens[0], TypeTokens[1], genericArgZType, mainZType, newZClass);
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
            string text = mainTypeToken.Text;
            if(this.ProcContext.IsCompilingClassName(text))
            {
                ExpTypeCompiling etc = new ExpTypeCompiling(this.ExpContext,  mainTypeToken);
                //etc.SetContextExp(this.ExpContext);
                return etc.Analy();
            }
            else if (this.ProcContext.IsImportClassName(text))
            {
                ExpTypeSingle ets = new ExpTypeSingle(this.ExpContext, mainTypeToken);
                //ets.SetContextExp(this.ExpContext);
                return ets.Analy();
            }
            else
            {
                Errorf(mainTypeToken.Position, "找不到'{0}'类型", text);
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
            string typeName = token.Text;
            var ztypes = this.ExpContext.FileContext.ImportUseContext.SearchImportType(typeName);
            ZType ztype = ztypes[0] as ZType;
            if(ztype==null)
            {
                Errorf(token.Position, "类型'{0}'不存在", token.Text);
            }
            return ztype;
        }

        public override void SetParent(Exp parentExp)
        {
            ParentExp = parentExp;
        }

        public override Exp[] GetSubExps()
        {
            return new Exp[] { };
        }

        #region 覆盖

        public override string ToString()
        {
            return string.Join("", TypeTokens.Select(p => p.Text));
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
