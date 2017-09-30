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

namespace ZCompileCore.AST
{
    public class SectionEnum : SectionBase
    {
        public Token KeyToken;
        public Token NameToken;
        public EnumBuilder Builder;

        public List<Token> Values = new List<Token>();
        string EnumName;
        string EnumFullName;

        public void AddValueToken(Token token)
        {
            Values.Add(token);
        }

        public ZType Compile(ModuleBuilder moduleBuilder, string packageName)
        {
            AnalyName();
            EmitName( moduleBuilder, packageName);
            AnalyBody();
            EmitBody();
            var EmitedType = Builder.CreateType();
            ZType ztype = ZTypeManager.GetByMarkType(EmitedType) as ZType;
            return ztype;
        }

        private void AnalyName()
        {
            EnumName = NameToken.GetText();
        }

        private void EmitName(ModuleBuilder moduleBuilder, string packageName)
        {
            EnumFullName = packageName + "." + EnumName;
            Builder = moduleBuilder.DefineEnum(EnumFullName, TypeAttributes.Public, typeof(int));
            SetAttrTktClass(Builder);
        }

        private void SetAttrTktClass(EnumBuilder classBuilder)
        {
            Type myType = typeof(ZEnumAttribute);
            ConstructorInfo infoConstructor = myType.GetConstructor(new Type[] { });
            CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(infoConstructor, new object[] { });
            classBuilder.SetCustomAttribute(attributeBuilder);
        }

        public void AnalyBody()
        {
            Dictionary<string, Token> dict = new Dictionary<string, Token>();
            for (int i = 0; i < Values.Count; i++)
            {
                Token item = Values[i];
                string name = item.GetText();
                if(dict.ContainsKey(name))
                {
                    errorf(item.Position, "'{0}'重复", name);
                }
                else
                {
                    dict.Add(name, item);
                }
            }
        }

        public void EmitBody()
        {
            for (int i = 0; i < Values.Count; i++)
            {
                Token item = Values[i];
                string name = item.GetText();
                var builder = Builder.DefineLiteral(name, i+1);
            }
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.AppendLine();
            buf.Append(NameToken!=null? NameToken.GetText():"");
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
