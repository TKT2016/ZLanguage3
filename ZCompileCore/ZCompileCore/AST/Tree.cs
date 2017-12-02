using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Reports;

namespace ZCompileCore.AST
{
    public abstract class Tree
    {
        public abstract ContextFile FileContext { get; }
        public bool AnalyCorrect { get; protected set; }

        public Tree()
        {
            AnalyCorrect = true;
        }

        /// <summary>
        /// 报告错误并把分析结果设为false
        /// </summary>
        protected virtual void ErrorF(CodePosition postion, string msgFormat, params string[] msgParams)
        {
            this.FileContext.Errorf(postion, msgFormat, msgParams);
            AnalyCorrect = false;
           
        }
    }
}
