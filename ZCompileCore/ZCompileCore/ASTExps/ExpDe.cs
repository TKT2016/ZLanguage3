using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Lex;
using ZLangRT.Utils;
using ZCompileDesc.Descriptions;
using ZCompileKit;
using ZCompileKit.Tools;
using ZCompileCore.ASTExps;
using System;
using ZCompileCore.Tools;
using ZCompileDesc.Utils;

namespace ZCompileCore.AST
{
    public class ExpDe : Exp, ISetter
    {
        public LexToken KeyToken { get; set; }
        public Exp LeftExp { get; set; }
        public LexToken RightToken { get; set; }
        private string propertyName;
        private IIdent memberSymbol;
        private ZCLocalVar tempLocal;

        public override Exp Analy()
        {
            if (this.ExpContext == null) throw new CCException();
            LeftExp = AnalyLeft();
            if (RightToken == null)
            {
                ErrorF(LeftExp.Position, "'{0}'的后面缺少属性", LeftExp.ToString());
                return LeftExp;
            }


            if (RightToken.Kind == TokenKind.Ident && RightToken.GetText() == ZKeywords.Each)
            {
                var eachItemExp = AnalyEach();
                Exp newExp = eachItemExp.Analy();
                return newExp;
            }

            AnalyRight();
            return this;
        }

        private void AnalyRight()
        {
            propertyName = RightToken.GetText();
            
            if (LeftExp.RetType is ZLEnumInfo)
            {
                ErrorF(LeftExp.Position, "约定没有属性");
                return;
            }
            else if (LeftExp.RetType is ZLClassInfo)
            {
                AnalyMember(LeftExp.RetType as ZLClassInfo);
            }
            else if (LeftExp.RetType is ZCClassInfo)
            {
                AnalyMember(LeftExp.RetType as ZCClassInfo);
            }

            if (memberSymbol == null)
            {
                ErrorF(LeftExp.Position, "{0}不存在成员'{1}'", LeftExp.ToString(), propertyName);
            }
            else if (IsNeedTempLocal())
            {
                AnalyPropertyTempLocal();
            }
        }

        private bool IsNeedTempLocal()
        {
            if (!LeftExp.RetType.IsStruct) return false;
            if(LeftExp is ExpDefProperty ||LeftExp is ExpUseProperty ||LeftExp is ExpSuperProperty)
            {
                return true;
            }
            return false;
        }

        private void AnalyPropertyTempLocal()
        {
            int tempIndex = this.ExpContext.ProcContext.CreateTempIndex();
            var tempName = "@property_temp" + tempIndex;
            tempLocal = new ZCLocalVar(tempName, LeftExp.RetType);
            tempLocal.LoacalVarIndex = this.ExpContext.ProcContext.CreateLocalVarIndex(tempLocal.ZName);
            this.ProcContext.AddLocalVar(tempLocal);
        }

        private void AnalyMember(ZCClassInfo zclass)
        {
            memberSymbol = zclass.SearchProperty(propertyName);
            if (memberSymbol != null)
            {
                RetType = ((ZAPropertyInfo)memberSymbol).GetZPropertyType();
            }
        }

        private void AnalyMember(ZLClassInfo zclass)
        {
            memberSymbol = zclass.SearchProperty(propertyName);
            if (memberSymbol != null)
            {
                RetType = ((ZLPropertyInfo)memberSymbol).GetZPropertyType();
                return;
            }

            memberSymbol = zclass.SearchField(propertyName);
            if (memberSymbol != null)
            {
                RetType = ((ZLFieldInfo)memberSymbol).GetZFieldType();
            }
        }

        private ExpEachItem AnalyEach()
        {
            this.ExpContext.Stmt.HasEach = true;
            StmtCall callStmt = this.ExpContext.Stmt as StmtCall;
            ExpEach eachExp = new ExpEach(this.ExpContext, this.LeftExp);
            callStmt.SetEachExp(eachExp);
            eachExp.Analy();
            ExpEachItem itemExp = eachExp.GetItemExp();
            return itemExp;
        }

        private Exp AnalyLeft()
        {
            var newExp = AnalySubExp(LeftExp);
            if (newExp is ExpTypeBase)
            {
                var leftTypeExp = (LeftExp as ExpTypeBase);
                ZType ltype = leftTypeExp.RetType;
                if (ltype is ZLEnumInfo)
                {
                    ErrorF(LeftExp.Position, "约定类型'{0}'取值不能用'的'", LeftExp.ToString());
                }
                else if (ltype is ZLClassInfo)
                {
                    ExpStaticClassName escn = new ExpStaticClassName(leftTypeExp.GetMainToken(), (ltype as ZLClassInfo));
                    escn.SetContext(this.ExpContext);
                    newExp = escn.Analy();
                }
            }
            return newExp;
            //else if (LeftExp is ExpLocalVar)
            //{
            //    memberSymbol = (LeftExp as ExpLocalVar).LocalVarSymbol;
            //}
            //else if (LeftExp is ExpArg)
            //{
            //    memberSymbol = (LeftExp as ExpArg).ArgSymbol;
            //}
            //else
            //{
            //    //isNeedEmitLeft = true;
            //    //var VarName = "LocalDStructIndex_" + LocalDStructIndex;
            //    //ZCLocalVar localVarSymbol = new ZCLocalVar(VarName, LeftExp.RetType);
            //    //localVarSymbol.LoacalVarIndex = this.ExpContext.ProcContext.CreateLocalVarIndex(VarName);
            //    //this.ProcContext.AddLocalVar(localVarSymbol);
            //    //memberSymbol = localVarSymbol;
            //    //LocalDStructIndex++;
            //}
        }

        public override void Emit( )
        {
            EmitGet();
            base.EmitConv();
        }

        public void EmitSet(Exp valueExp)
        {
            EmitLeft();
            valueExp.Emit();
            EmitSetMember();
        }

        public void EmitGet( )
        {
            EmitLeft();
            EmitGetMember();
        }

        private void EmitGetMember()
        {
            if(memberSymbol is ZLFieldInfo)
                {
                    EmitSymbolHelper.EmitLoad(IL,(ZLFieldInfo)memberSymbol);
                }
                else if(memberSymbol is ZCFieldInfo)
                {
                   EmitSymbolHelper.EmitLoad(IL,(ZCFieldInfo)memberSymbol);
                }
                else if(memberSymbol is ZLPropertyInfo)
                {
                     EmitSymbolHelper.EmitLoad(IL,(ZLPropertyInfo)memberSymbol);
                }
                else if(memberSymbol is ZCPropertyInfo)
                {
                     EmitSymbolHelper.EmitLoad(IL,(ZCPropertyInfo)memberSymbol);
                }
            else
            {
                throw new CCException();
            }
        }

        private void EmitSetMember()
        {
            if(memberSymbol is ZLFieldInfo)
                {
                    EmitSymbolHelper.EmitStorm(IL,(ZLFieldInfo)memberSymbol);
                }
                else if(memberSymbol is ZCFieldInfo)
                {
                   EmitSymbolHelper.EmitStorm(IL,(ZCFieldInfo)memberSymbol);
                }
                else if(memberSymbol is ZLPropertyInfo)
                {
                     EmitSymbolHelper.EmitStorm(IL,(ZLPropertyInfo)memberSymbol);
                }
                else if(memberSymbol is ZCPropertyInfo)
                {
                     EmitSymbolHelper.EmitStorm(IL,(ZCPropertyInfo)memberSymbol);
                }
            else
            {
                throw new CCException();
            }
        }

        private void EmitLeft()
        {
            if(LeftExp.RetType.IsStruct)
            {
                if(IsNeedTempLocal())
                {
                    LeftExp.Emit();
                    EmitSymbolHelper.EmitStorm(IL, this.tempLocal);
                    EmitSymbolHelper.EmitLoada(IL, this.tempLocal);
                }
                else
                {
                    if(LeftExp is ExpLocalVar)
                    {
                        ((ExpLocalVar)LeftExp).EmitLoadLocala();
                    }
                    else if(LeftExp is ExpArg)
                    {
                        ((ExpArg)LeftExp).EmitLoadArga();
                    }
                    else if(LeftExp is ExpDefField)
                    {
                        ((ExpDefField)LeftExp).EmitLoadFielda();
                    }
                    else if(LeftExp is ExpSuperField)
                    {
                        ((ExpSuperField)LeftExp).EmitLoadFielda();
                    }
                    else if(LeftExp is ExpUseField)
                    {
                        ((ExpUseField)LeftExp).EmitLoadFielda();
                    }
                    else
                    {
                        LeftExp.Emit();
                    }
                }
            }
            else
            {
                LeftExp.Emit();
            }
        }
            
        #region 次要方法属性

        public bool CanWrite
        {
            get
            {
                return this.memberSymbol.GetCanWrite();
            }
        }

        public override Exp[] GetSubExps()
        {
            return new Exp[] { LeftExp };
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append(LeftExp != null ? LeftExp.ToString() : "");
            buf.Append("的");
            buf.Append(RightToken != null ? RightToken.GetText() : "");
            return buf.ToString();
        }
        
        public override CodePosition Position
        {
            get
            {
                return LeftExp.Position; 
            }
        }
        #endregion
    }
}

