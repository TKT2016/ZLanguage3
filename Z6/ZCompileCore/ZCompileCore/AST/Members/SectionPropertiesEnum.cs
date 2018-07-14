using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.ASTRaws;
using ZCompileCore.Lex;
using ZLangRT.Attributes;

namespace ZCompileCore.AST
{
    public class SectionPropertiesEnum
    {
        private SectionPropertiesRaw Raw;
        private List<string> EnumItems = new List<string>();
        private Dictionary<string, LexTokenText> dict = new Dictionary<string, LexTokenText>();

        private EnumAST ASTEnum;

        public SectionPropertiesEnum(EnumAST astEnum,SectionPropertiesRaw raw)
        {
            ASTEnum = astEnum;
            Raw = raw;
        }

        public void AnalyTypes()
        {

        }

        public void AnalyNames()
        {
            if (Raw == null || Raw.Properties == null) return;
            for (int i = 0; i < Raw.Properties.Count; i++)
            {
                PropertyASTRaw item = Raw.Properties[i];
                string name = item.NameToken.Text;
                if (dict.ContainsKey(name))
                {
                    this.ASTEnum.FileContext.Errorf(item.NameToken.Position, "'{0}'重复", name);
                }
                else
                {
                    dict.Add(name, item.NameToken);
                    EnumItems.Add(name);
                }
                if (item.ValueExp != null)
                {
                    this.ASTEnum.FileContext.Errorf(item.NameToken.Position, "约定类型的属性'{0}'不能有值", name);
                }
            }
        }

        public void Analy()
        {
            if (Raw == null || Raw.Properties == null) return;
            for (int i = 0; i < Raw.Properties.Count; i++)
            {
                PropertyASTRaw item = Raw.Properties[i];
                string name = item.NameToken.Text;
                if (dict.ContainsKey(name))
                {
                    this.ASTEnum.FileContext.Errorf(item.NameToken.Position, "'{0}'重复", name);
                }
                else
                {
                    dict.Add(name, item.NameToken);
                    EnumItems.Add(name);
                }
                if(item.ValueExp!=null)
                {
                    this.ASTEnum.FileContext.Errorf(item.NameToken.Position, "约定类型的属性'{0}'不能有值", name);
                }
            }
        }

        public void Emit(EnumBuilder enumBuilder)
        {
            for (int i = 0; i < EnumItems.Count; i++)
            {
                string name = EnumItems[i];
                FieldBuilder builder = enumBuilder.DefineLiteral(name, i + 1);
                ASTUtil.SetAttrZCode(builder, name);
            }
        }

        public void EmitNames( )
        {
            EnumBuilder enumBuilder = this.ASTEnum.EnumTypeBuilder;

            for (int i = 0; i < EnumItems.Count; i++)
            {
                string name = EnumItems[i];
                FieldBuilder builder = enumBuilder.DefineLiteral(name, i + 1);
                ASTUtil.SetAttrZCode(builder, name);
            }
        }
    }
}
