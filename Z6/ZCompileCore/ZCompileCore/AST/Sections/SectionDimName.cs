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
    public class SectionDimName:SectionNameBase
    {
        private DimAST DimTree;
        public SectionDimName(SectionNameRaw raw)
            : base(raw)
        {

        }

        public void Analy(DimAST ast)
        {
            DimTree = ast;
            base.AnalyRaw(ast);
            //ZCClassInfo cclass = this.EnumTree.ClassContext.GetZCompilingType();
            //this.FileContext.ImportUseContext.ImportCompilingName(cclass);
        }
    }
}
