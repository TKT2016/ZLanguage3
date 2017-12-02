using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Lex;
using ZCompileCore.Symbols;
using ZLangRT.Utils;
using ZCompileDesc.Descriptions;
using ZCompileKit;
using ZCompileKit.Tools;
using ZCompileDesc.ZMembers;
using ZCompileDesc.ZTypes;
using ZCompileCore.ASTExps;
using System;
using ZCompileCore.Tools;

namespace ZCompileCore.AST
{
    public class ExpDe : Exp, ISetter
    {
        public LexToken KeyToken { get; set; }
        public Exp LeftExp { get; set; }
        public LexToken RightToken { get; set; }

        string propertyName;
        ZMemberInfo ZMember;

        public override Exp Analy()
        {
            if (this.ExpContext == null) throw new CCException();
            LeftExp = AnalyLeft();
            if (RightToken != null)
            {
                if (RightToken.Kind == TokenKind.Ident && RightToken.GetText() == ZKeywords.Each)
                {
                    var eachItemExp = AnalyEach();
                    Exp newExp = eachItemExp.Analy();
                    return newExp;
                }
                else
                {
                    ParseRightPropertyName();
                }
                if (LeftExp.RetType is ZEnumType)
                {
                    ErrorF(LeftExp.Position, "约定没有属性");
                }
                else if (LeftExp.RetType is ZClassType)
                {
                    ZClassType zclass = LeftExp.RetType as ZClassType;
                    ZMember = zclass.SearchZMember(propertyName);
                    if (ZMember == null)
                    {
                        ErrorF(LeftExp.Position, "不存在'{0}'属性", propertyName);
                    }
                    else
                    {
                        RetType = ZMember.MemberZType;
                    }
                }
                else
                {
                    throw new CCException();
                }
            }
            else
            {
                ErrorF(LeftExp.Position, "'{0}'的后面缺少属性", LeftExp.ToString());
                return LeftExp;
            }
            return this;
        }

        SymbolDefBase leftStructSymbol;
        bool isNeedEmitLeft=false;
        private Exp AnalyLeft()
        {
            //if (LeftExp.ToString() == "鼠标位置")
            //{
            //    Console.WriteLine("鼠标位置");
            //}
            LeftExp =  AnalySubExp(LeftExp);
            if (LeftExp is ExpTypeBase)
            {
                var leftTypeExp = (LeftExp as ExpTypeBase);
                ZType ltype = leftTypeExp.RetType;
                if(ltype is ZEnumType)
                {
                    ErrorF(LeftExp.Position, "约定类型'{0}'取值不能用'的'", LeftExp.ToString());
                }
                else if (ltype is ZClassType)
                {
                    ExpStaticClassName escn = new ExpStaticClassName(leftTypeExp.GetMainToken(),(ltype as ZClassType));
                    escn.SetContext(this.ExpContext);
                    LeftExp = escn.Analy();
                }
            }

            if (ReflectionUtil.IsStruct(LeftExp.RetType.SharpType))
            {
                if (LeftExp is ExpLocalVar)
                {
                    leftStructSymbol = (LeftExp as ExpLocalVar).LocalVarSymbol;
                }
                else if (LeftExp is ExpArg)
                {
                    leftStructSymbol = (LeftExp as ExpArg).ArgSymbol;
                }
                else
                {
                    isNeedEmitLeft = true;
                    var VarName="LocalDStructIndex_" + LocalDStructIndex;
                    SymbolLocalVar localVarSymbol = new SymbolLocalVar(VarName, LeftExp.RetType);
                    localVarSymbol.LoacalVarIndex = this.ExpContext.ProcContext.CreateLocalVarIndex(VarName);
                    this.ProcContext.AddDefSymbol(localVarSymbol);
                    leftStructSymbol = localVarSymbol;
                    LocalDStructIndex++;
                }
                //throw new CCException();
            }
            return LeftExp;
        }

        private bool ParseRightPropertyName()
        {
            propertyName = RightToken.GetText();
            return true;
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

        public override void Emit( )
        {
            EmitGet();
            base.EmitConv();
        }

        public void EmitGet( )
        {
            if (ZMember is ZPropertyInfo)
            {
                MethodInfo getMethod = (ZMember as ZPropertyInfo).SharpProperty.GetGetMethod();
                EmitSubject();
                EmitHelper.CallDynamic(IL, getMethod);
            }
            else
            {
                EmitSubject();
                EmitHelper.LoadField(IL, (ZMember as ZFieldInfo).SharpField);
            }
        }
        
        public void EmitSet(Exp valueExp)
        {
            if (ZMember is ZPropertyInfo)
            {
                MethodInfo setMethod = (ZMember as ZPropertyInfo).SharpProperty.GetSetMethod();
                EmitSubject();
                valueExp.Emit();
                EmitHelper.CallDynamic(IL, setMethod);
            }
            else
            {
                EmitSubject();
                valueExp.Emit();
                EmitHelper.StormField(IL, (ZMember as ZFieldInfo).SharpField);
            }
        }

        private void EmitSubject( )
        {
            //if (LeftExp.ToString() == "鼠标位置")
            //{
            //    Console.WriteLine("鼠标位置");
            //}
            if (SubjectIsStruct())
            {
                EmitLoadStruct();
            }
            else
            {
                LeftExp.Emit();
            }
        }

        private bool SubjectIsStruct()
        {
            if (!(LeftExp is ExpVarBase)) return false;
            ExpVarBase varexp = LeftExp as ExpVarBase;
            return (ReflectionUtil.IsStruct(varexp.RetType.SharpType));
        }

        private bool EmitLoadStruct()
        {
            ExpVarBase varexp = LeftExp as ExpVarBase;
            if (ReflectionUtil.IsStruct(varexp.RetType.SharpType))
            {
                if (isNeedEmitLeft)
                {
                    LeftExp.Emit();
                    if (leftStructSymbol is SymbolLocalVar)
                    {
                        EmitHelper.StormVar(IL, (leftStructSymbol as SymbolLocalVar).VarBuilder);
                        return true;
                    }
                    else if (leftStructSymbol is SymbolArg)
                    {
                        EmitHelper.StormArg(IL , (leftStructSymbol as SymbolArg).ArgIndex);
                        return true;
                    }
                }
                if (leftStructSymbol is SymbolLocalVar)
                {
                    IL.Emit(OpCodes.Ldloca, (leftStructSymbol as SymbolLocalVar).VarBuilder);
                    return true;
                }
                else if (leftStructSymbol is SymbolArg)
                {
                    IL.Emit(OpCodes.Ldarga, (leftStructSymbol as SymbolArg).ArgIndex);
                    return true;
                }
                //else if (varexp is ExpSuperProperty)
                //{
                //    SymbolLocalVar syslocal = new SymbolLocalVar("LocalDStructIndex_" + LocalDStructIndex, varexp.RetType);
                //    varexp.Emit();
                //    //EmitSymbolHelper.EmitLoad(IL, syslocal);
                //    //EmitSymbolHelper.EmitLoad(IL, syslocal);
                //    EmitHelper.StormVar(IL, syslocal.VarBuilder);
                //    IL.Emit(OpCodes.Ldloca, syslocal.VarBuilder);
                //    return true;
                //}
                throw new CCException();
            }
            return false;
        }

        static int LocalDStructIndex = 1;

        #region 次要方法属性

        public bool CanWrite
        {
            get
            {
                return ZMember.CanWrite;
            }
        }

        public override Exp[] GetSubExps()
        {
            return new Exp[] { LeftExp };//return new Exp[] { LeftExp, RightExp };
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
