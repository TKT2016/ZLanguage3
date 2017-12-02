using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Lex;
using ZLangRT.Attributes;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;
using ZCompileDesc.ZTypes;
using ZCompileDesc;
using ZCompileCore.Contexts;

namespace ZCompileCore.AST
{
    public class SectionEnum : SectionBase
    {
        public LexToken KeyToken;
        public LexToken NameToken;
        public EnumBuilder Builder;

        public List<LexToken> Values = new List<LexToken>();
        string EnumName;
        string EnumFullName;

        public void AddValueToken(LexToken token)
        {
            Values.Add(token);
        }

        public ZEnumType Compile(ModuleBuilder moduleBuilder, string packageName, string fileName)
        {
            AnalyText();
            EmitName();
            AnalyBody();
            EmitBody();
            var EmitedType = Builder.CreateType();
            ZEnumType ztype = ZTypeManager.GetByMarkType(EmitedType) as ZEnumType;
            return ztype;
        }

        public override void AnalyText()
        {
            string fileName = this.FileContext.FileModel.GetFileNameNoEx();
            if (NameToken != null)
            {
                EnumName = NameToken.GetText();
            }
            else
            {
                EnumName = fileName;
            }
        }

        public override void AnalyType()
        {
           
        }

        Dictionary<string, LexToken> dict = new Dictionary<string, LexToken>();
        public override void AnalyBody()
        {
            for (int i = 0; i < Values.Count; i++)
            {
                LexToken item = Values[i];
                string name = item.GetText();
                if (dict.ContainsKey(name))
                {
                    ErrorF(item.Position, "'{0}'重复", name);
                }
                else
                {
                    dict.Add(name, item);
                }
            }
        }

        public override void EmitName()
        {
            string packageName = this.FileContext.ProjectContext.PackageName;
            ModuleBuilder moduleBuilder = this.FileContext.ProjectContext.EmitContext.ModuleBuilder;
            EnumFullName = packageName + "." + EnumName;
            Builder = moduleBuilder.DefineEnum(EnumFullName, TypeAttributes.Public, typeof(int));
            SetAttrTktClass(Builder);
        }

        public override void EmitBody()
        {
            for (int i = 0; i < Values.Count; i++)
            {
                LexToken item = Values[i];
                string name = item.GetText();
                FieldBuilder builder = Builder.DefineLiteral(name, i + 1);
                SetAttrZCode(builder, name);
            }
        }

        private void SetAttrZCode(FieldBuilder fieldBuilder,string zcode)
        {
            Type myType = typeof(ZCodeAttribute);
            ConstructorInfo infoConstructor = myType.GetConstructor(new Type[] { typeof(string) });
            CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(infoConstructor, new object[] { zcode });
            fieldBuilder.SetCustomAttribute(attributeBuilder);
        }

        private void SetAttrTktClass(EnumBuilder classBuilder)
        {
            Type myType = typeof(ZEnumAttribute);
            ConstructorInfo infoConstructor = myType.GetConstructor(new Type[] { });
            CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(infoConstructor, new object[] { });
            classBuilder.SetCustomAttribute(attributeBuilder);
        }

        public void SetContext(ContextFile fileContext)
        {
            this.FileContext = fileContext;
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.AppendLine();
            buf.Append(NameToken != null ? NameToken.GetText() : "");
            buf.Append(" : ");
            if (Values.Count > 0)
            {
                string itemCode = string.Join(",", Values.Select(p => p.GetText()));
                buf.AppendLine(itemCode);
            }
            buf.AppendLine();
            return buf.ToString();
        }
    }
}
