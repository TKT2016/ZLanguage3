using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.ASTRaws;
using ZCompileCore.Contexts;
using ZCompileDesc.Descriptions;
using ZLangRT.Attributes;

namespace ZCompileCore.AST
{
    public class SectionClassName:SectionNameBase
    {
        private ClassAST ClassTree;
        public SectionClassName(SectionNameRaw raw)
            : base(raw)
        {

        }

        public void Analy(ClassAST ast)
        {
            ClassTree = ast;
            base.AnalyRaw(ast);
            this.ClassTree.ClassContext.SetClassName(TypeName);
            ZCClassInfo cclass = this.ClassTree.ClassContext.GetZCompilingType();
            this.FileContext.ImportUseContext.ImportZCompiling(cclass);
        }
    }
}
