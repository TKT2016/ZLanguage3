using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Reports;
using ZCompileDesc.Descriptions;


namespace ZCompileCore.AST
{
    public class SectionImport : SectionBase
    {
        public LexToken KeyToken;
        public List<PackageNameAST> Packages = new List<PackageNameAST>();

        public override void AnalyText()
        {
            foreach (PackageNameAST itemPackage in this.Packages)
            {
                itemPackage.AnalyText();
            }
        }

        public override void AnalyType()
        {
            foreach (PackageNameAST itemPackage in this.Packages)
            {
                itemPackage.AnalyType();
            }
        }

        public override void AnalyBody()
        {
            return;
        }

        public override void EmitName()
        {
            return;
        }

        public override void EmitBody()
        {
            return;
        }

        public void SetContext(ContextFile fileContext)
        {
            this.FileContext = fileContext;
            foreach (PackageNameAST itemPackage in this.Packages)
            {
                itemPackage.SetContext(fileContext);
            }
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append(KeyToken.GetText());
            buf.Append(":");
            List<string> tempList = new List<string>();
            foreach (var item in this.Packages)
            {
                buf.Append(item.ToString());
            }
            buf.Append(string.Join(",", tempList));
            buf.AppendLine(";");
            return buf.ToString();
        }

    }
}
