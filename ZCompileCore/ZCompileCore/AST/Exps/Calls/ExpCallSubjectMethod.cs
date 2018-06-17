using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Tools;
using ZCompileDesc.Descriptions;

namespace ZCompileCore.AST.Exps
{
    /// <summary>
    /// 表达式调用方法
    /// </summary>
    public class ExpCallSubjectMethod : ExpCallBase
    {
        private Exp _SubjectAnalyedExp;
        private Exp SubjectAnalyedExp { get { return _SubjectAnalyedExp; } set { _SubjectAnalyedExp = value; _SubjectAnalyedExp.ParentExp = this; } }
        LexToken MethodToken;
        ZMethodCall CallDesc;
        string MethodName;
        ZLMethodInfo Method;

        public ExpCallSubjectMethod( ContextExp expContext,Exp subjectAnalyedExp, LexToken token)
            : base(expContext)
        {
            SubjectAnalyedExp = subjectAnalyedExp;
            MethodToken = token;
        }

        public override Exp Analy()
        {
            if (this.IsAnalyed) return this;
            MethodName = MethodToken.Text;
            CallDesc = new ZMethodCall();
            CallDesc.Add(MethodName);

            Method = SearchZMethod(MethodName);
            RetType = Method.RetZType;
            IsAnalyed = true;
            return this;
        }

        private ZLMethodInfo SearchZMethod(string name)
        {
            ZType mainType = SubjectAnalyedExp.RetType;
            ZMethodCall calldesc = new ZMethodCall();
            calldesc.Add(name);
            if (mainType is ZLClassInfo)
            {
                var methods = (mainType as ZLClassInfo).SearchZMethod(calldesc);
                return methods[0];
            }
            return null;
        }

        #region Emit
        public override void Emit()
        {
            EmitSubject();
            EmitHelper.CallDynamic(IL, Method.SharpMethod);
            EmitConv();
        }

        private void EmitSubject()
        {
            SubjectAnalyedExp.Emit();
        }
        #endregion

        public override void SetParent(Exp parentExp)
        {
            ParentExp = parentExp;
            SubjectAnalyedExp.SetParent(this);
        }

        public override Exp[] GetSubExps()
        {
            return new Exp[] { SubjectAnalyedExp };
        }
    }
}
