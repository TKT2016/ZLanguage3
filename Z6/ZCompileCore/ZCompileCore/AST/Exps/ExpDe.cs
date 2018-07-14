using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Lex;
using ZLangRT.Utils;
using ZCompileDesc.Descriptions;
using ZCompileCore;
using ZCompileCore.Tools;
using ZCompileCore.AST.Exps;
using System;
using ZCompileDesc.Utils;
using ZCompileCore.Contexts;

namespace ZCompileCore.AST
{
    public class ExpDe : Exp , IEmitSet
    {
        public LexTokenText KeyToken { get; set; }
        private Exp _SubjectExp;
        public Exp SubjectExp { get { return _SubjectExp; } set { _SubjectExp = value; _SubjectExp.ParentExp = this; } }
        public LexTokenText RightToken { get; set; }
        private string propertyName;
        private IIdent memberSymbol;
        private ZCLocalVar tempLocal;

        public ExpDe(ContextExp expContext)
            : base(expContext)
        {

        }

        public override void SetParent(Exp parentExp)
        {
            ParentExp = parentExp;
            SubjectExp.SetParent(this);
        }

        public override Exp Analy()
        {
            if (this.IsAnalyed) return this;
            if (this.ExpContext == null) throw new CCException();
           
            SubjectExp = AnalyLeft();
            if (RightToken == null)
            {
                Errorf(SubjectExp.Position, "'{0}'的后面缺少属性", SubjectExp.ToString());
                return SubjectExp;
            }

            if (RightToken.IsKind(TokenKindKeyword.Each))
            {
                var eachItemExp = AnalyEach();
                Exp newExp = eachItemExp.Analy();
                return newExp;
            }
            else
            {
                AnalyRight();
            }
            IsAnalyed = true;
            return this;
        }

        private void AnalyRight()
        {
            propertyName = RightToken.Text;
            
            if (SubjectExp.RetType is ZLEnumInfo)
            {
                Errorf(SubjectExp.Position, "约定没有属性");
                return;
            }
            else if (SubjectExp.RetType is ZLClassInfo)
            {
                AnalyMember(SubjectExp.RetType as ZLClassInfo);
            }
            else if (SubjectExp.RetType is ZCClassInfo)
            {
                AnalyMember(SubjectExp.RetType as ZCClassInfo);
            }

            if (memberSymbol == null)
            {
                Errorf(SubjectExp.Position, "{0}不存在成员'{1}'", SubjectExp.ToString(), propertyName);
            }
            else if (IsNeedTempLocal())
            {
                AnalyPropertyTempLocal();
            }
        }

        private bool IsNeedTempLocal()
        {
            if (!SubjectExp.RetType.IsStruct) return false;
            if(SubjectExp is ExpPropertyDef ||SubjectExp is ExpUseProperty ||SubjectExp is ExpPropertySuper)
            {
                return true;
            }
            return false;
        }

        private void AnalyPropertyTempLocal()
        {
            int tempIndex = this.ExpContext.ProcContext.CreateTempIndex();
            var tempName = "@property_temp" + tempIndex;
            tempLocal = new ZCLocalVar(tempName, SubjectExp.RetType,true);
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
            //this.ExpContext.Stmt.HasEach = true;
            StmtCall callStmt = this.ExpContext.Stmt as StmtCall;
            ExpEach eachExp = new ExpEach(this.ExpContext, this.SubjectExp);
            callStmt.SetEachExp(eachExp);
            eachExp.Analy();
            ExpEachItem itemExp = eachExp.GetItemExp();
            return itemExp;
        }

        private Exp AnalyLeft()
        {
            var newExp = AnalySubExp(SubjectExp);
            if (newExp is ExpTypeBase)
            {
                var leftTypeExp = (SubjectExp as ExpTypeBase);
                ZType ltype = leftTypeExp.RetType;
                if (ltype is ZLEnumInfo)
                {
                    Errorf(SubjectExp.Position, "约定类型'{0}'取值不能用'的'", SubjectExp.ToString());
                }
                else if (ltype is ZLClassInfo)
                {
                    ExpStaticClassName escn = new ExpStaticClassName(this.ExpContext,leftTypeExp.GetMainToken(), (ltype as ZLClassInfo));
                    //escn.SetContextExp(this.ExpContext);
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
            if(SubjectExp.RetType.IsStruct)
            {
                if(IsNeedTempLocal())
                {
                    SubjectExp.Emit();
                    EmitSymbolHelper.EmitStorm(IL, this.tempLocal);
                    EmitSymbolHelper.EmitLoada(IL, this.tempLocal);
                }
                else
                {
                    if(SubjectExp is ExpLocalVar)
                    {
                        ((ExpLocalVar)SubjectExp).EmitLoadLocala();
                    }
                    else if(SubjectExp is ExpArg)
                    {
                        ((ExpArg)SubjectExp).EmitLoadArga();
                    }
                    else if(SubjectExp is ExpFieldDef)
                    {
                        ((ExpFieldDef)SubjectExp).EmitLoadFielda();
                    }
                    else if(SubjectExp is ExpFieldSuper)
                    {
                        ((ExpFieldSuper)SubjectExp).EmitLoadFielda();
                    }
                    else if(SubjectExp is ExpUseField)
                    {
                        ((ExpUseField)SubjectExp).EmitLoadFielda();
                    }
                    else
                    {
                        SubjectExp.Emit();
                    }
                }
            }
            else
            {
                SubjectExp.Emit();
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
            return new Exp[] { SubjectExp };
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append(SubjectExp != null ? SubjectExp.ToString() : "");
            buf.Append("的");
            buf.Append(RightToken != null ? RightToken.Text : "");
            return buf.ToString();
        }
        
        public override CodePosition Position
        {
            get
            {
                return SubjectExp.Position; 
            }
        }
        #endregion
    }
}

