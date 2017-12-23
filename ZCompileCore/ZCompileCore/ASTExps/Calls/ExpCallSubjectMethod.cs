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

using ZCompileKit.Tools;

namespace ZCompileCore.ASTExps
{
    /// <summary>
    /// 表达式调用方法
    /// </summary>
    public class ExpCallSubjectMethod : ExpCallBase
    {
        Exp SubjectAnalyedExp;
        LexToken MethodToken;
        ZMethodCall CallDesc;
        string MethodName;
        ZLMethodInfo Method;

        public ExpCallSubjectMethod(Exp subjectAnalyedExp,LexToken token)
        {
            SubjectAnalyedExp = subjectAnalyedExp;
            MethodToken = token;
        }

        public override Exp Analy()
        {
            MethodName = MethodToken.GetText();
            CallDesc = new ZMethodCall();
            CallDesc.Add(MethodName);

            Method = SearchZMethod(MethodName);
            RetType = Method.RetZType;
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

        public override Exp[] GetSubExps()
        {
            return new Exp[] { SubjectAnalyedExp };
        }
    }
}
