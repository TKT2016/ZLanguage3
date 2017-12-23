using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// 程序中定义的使用类中的单个词的方法
    /// </summary>
    public class ExpCallSingleUse : ExpCallSingle
    {
        protected ZLMethodInfo Method;
        public ExpCallSingleUse(LexToken token)
        {
            VarToken = token;
        }

        public override Exp Analy()
        {
            VarName = VarToken.GetText();
            Method = SearchZMethod(VarName);
            RetType = Method.RetZType;
            return this;
        }

        private ZLMethodInfo SearchZMethod(string name)
        {
            ZMethodCall calldesc = new ZMethodCall();
            calldesc.Add(name);
            ContextImportUse contextiu = this.FileContext.ImportUseContext;
            return contextiu.SearchUseMethod(calldesc)[0];
            //ContextUse cu = this.ClassContext.FileContext.UseContext;
            //foreach (ZClassType zclass in cu.UseZClassList)
            //{
            //    if (zclass.IsStatic)
            //    {
            //        var zitem = zclass.SearchZMethod(calldesc);
            //        if (zitem != null && zitem.Length>0)
            //        {
            //            return zitem[0];
            //        }
            //    }
            //}
            //throw new CCException();
        }

        #region Emit
        public override void Emit()
        {
            EmitHelper.CallDynamic(IL, Method.SharpMethod);
            EmitConv(); 
        }

        #endregion

        
     
    }
}
