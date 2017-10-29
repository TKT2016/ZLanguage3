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

namespace ZCompileCore.AST
{
    public class ExpDe : Exp, ISetter
    {
        public Token KeyToken { get; set; }
        public Exp LeftExp { get; set; }
        public Exp RightExp { get; set; }

        string propertyName;
        ZMemberInfo ZMember;

        public override Exp Analy( )
        {
            LeftExp = AnalySubExp(LeftExp);

            if (RightExp is ExpEachWord)
            {
                var eachItemExp = AnalyEach();
                Exp newExp = eachItemExp.Analy();
                return newExp;
            }
            else
            {
                ParseRightPropertyName();
            }
            if (!this.AnalyCorrect) return this;

            if (LeftExp.RetType is ZEnumType)
            {
                ErrorE(RightExp.Position, "约定没有属性");
            }
            else if (LeftExp.RetType is ZClassType)
            {
                ZClassType zclass = LeftExp.RetType as ZClassType;
                ZMember = zclass.SearchZMember(propertyName);
                if (ZMember == null)
                {
                    ErrorE(LeftExp.Position, "不存在'{0}'属性", propertyName);
                }
                else
                {
                    RetType = ZMember.MemberZType;
                }
            }
            else
            {
                throw new CompileCoreException();
            }
            return this;
        }

        private bool ParseRightPropertyName()
        {
            if (RightExp is ExpVar)
            {
                ExpVar propertyExp = (RightExp as ExpVar);
                propertyName = propertyExp.VarToken.GetText();
                return true;
            }
            else if (RightExp is ExpType)
            {
                ExpType typeExp = (RightExp as ExpType);
                Token newToken = typeExp.ToSingleToken();
                propertyName = newToken.GetText();
                return true;
            }
            else
            {
                ErrorE(RightExp.Position, "'的'后面的‘{0}’不是属性名称", RightExp.ToString());
            }
            return false;
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
            if (SubjectIsStruct())
            {
                EmitAsVar();
            }
            else
            {
                LeftExp.Emit();
            }
        }

        private bool SubjectIsStruct()
        {
            if (!(LeftExp is ExpVar)) return false;
            ExpVar varexp = LeftExp as ExpVar;
            return (ReflectionUtil.IsStruct(varexp.RetType.SharpType));
        }

        private bool EmitAsVar()
        {
            ExpVar varexp = LeftExp as ExpVar;
            if (ReflectionUtil.IsStruct(varexp.RetType.SharpType))
            {
                if (varexp.VarSymbol is SymbolLocalVar)
                {
                    IL.Emit(OpCodes.Ldloca, (varexp.VarSymbol as SymbolLocalVar).VarBuilder);
                    return true;
                }
                else if (varexp.VarSymbol is SymbolArg)
                {
                    IL.Emit(OpCodes.Ldarga, (varexp.VarSymbol as SymbolArg).ArgIndex);
                    return true;
                }
            }
            return false;
        }

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
            return new Exp[] { LeftExp, RightExp };
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append(LeftExp != null ? LeftExp.ToString() : "");
            buf.Append("的");
            buf.Append(RightExp != null ? RightExp.ToString() : "");
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
