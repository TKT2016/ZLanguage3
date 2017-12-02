using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileKit.Tools;

namespace ZCompileCore.AST
{
    public class DimVarAST : SectionPartFile
    {
        public LexToken NameToken;
        public LexToken TypeToken;
        public string DimName;
        public string DimTypeName;
        //public ContextFile FileContext { get; set; }

        public override void AnalyText()
        {
            Analy();
        }

        bool isAnalyed = false;
        private void Analy()
        {
            if (isAnalyed) return;
            DimName = NameToken.GetText();
            DimTypeName = TypeToken.GetText();
            if (this.FileContext.ImportUseContext.ContainsDim(DimName))
            {
                this.FileContext.Errorf(NameToken.Position, "'{0}'重复声明", DimName);
            }
            else
            {
                this.FileContext.ImportUseContext.AddDim(DimName, DimTypeName);
            }
            isAnalyed = true;
        }

        public override void AnalyType()
        {
            Analy();
        }

        public override void AnalyBody()
        {
            Analy();
        }

        public override void EmitName()
        {
            return;
        }

        public override void EmitBody()
        {
            return;
        }

        FieldBuilder fieldBuilder;
        public void Emit(TypeBuilder classBuilder, ILGenerator il)
        {
            FieldAttributes fieldAttr = FieldAttributes.Private | FieldAttributes.Static | FieldAttributes.InitOnly;
            fieldBuilder = classBuilder.DefineField(DimName, typeof(string), fieldAttr);
            EmitHelper.LoadString(il, DimTypeName);
            EmitHelper.StormField(il, fieldBuilder);
        }

        public void SetContext(ContextFile fileContext)
        {
            this.FileContext = fileContext;
        }

        public override string ToString()
        {
            return string.Format("{0}={1}", NameToken.GetText(), TypeToken.GetText());
        }
    }
}
