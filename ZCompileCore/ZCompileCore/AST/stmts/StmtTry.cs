using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Lex;
using ZCompileCore.Tools;

namespace ZCompileCore.AST
{
    public class StmtTry: Stmt
    {
        Label tryLabel;
        public override Stmt Analy()
        {
            //base.LoadRefTypes(context);
            //var symbols = this.AnalyStmtContext.Symbols;
            return this;
        }

        public override void Emit( )
        {
            tryLabel = IL.BeginExceptionBlock();
        }

        public override void AnalyExpDim()
        {

        }

        //public override CodePosition Position
        //{
        //    get { return new CodePosition (0,0); }
        //}


        #region 覆盖
        public override string ToString()
        {
            return (getStmtPrefix() + "try");
        }

        //public override CodePostion Postion
        //{
        //    get
        //    {
        //        return new CodePostion(0,0);
        //    }
        //}
        #endregion

    }
}
