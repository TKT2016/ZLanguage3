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
using ZCompileCore.Symbols;
using ZCompileCore.Tools;
using ZCompileDesc.Descriptions;
using ZCompileDesc.ZMembers;
using ZCompileDesc.ZTypes;
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
        ZCallDesc CallDesc;
        string MethodName;

        public ExpCallSubjectMethod(Exp subjectAnalyedExp,LexToken token)
        {
            SubjectAnalyedExp = subjectAnalyedExp;
            MethodToken = token;
        }

        public override Exp Analy()
        {
            MethodName = MethodToken.GetText();
            CallDesc = new ZCallDesc();
            CallDesc.Add(MethodName);

            Method = SearchZMethod(MethodName);
            RetType = Method.RetZType;
            return this;
        }

        private ZMethodInfo SearchZMethod(string name)
        {
            ZType mainType = SubjectAnalyedExp.RetType;
            ZCallDesc calldesc = new ZCallDesc();
            calldesc.Add(name);
            if (mainType is ZClassType)
            {
                var methods = (mainType as ZClassType).SearchZMethod(calldesc);
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
