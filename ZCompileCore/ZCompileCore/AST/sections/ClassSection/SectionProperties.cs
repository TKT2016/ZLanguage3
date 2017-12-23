using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Parsers;

using ZCompileCore.Tools;
using ZCompileDesc.Descriptions;


namespace ZCompileCore.AST
{
    public class SectionProperties : SectionClassBase
    {
        public LexToken TypeToken;
        public LexToken KeyToken;
        public List<PropertyAST> Properties = new List<PropertyAST>();
        ContextMethod PropertiesInitProcContext;

        public void AddProperty(PropertyAST property)
        {
            Properties.Add(property);
        }

        public override void AnalyText()
        {
            foreach (PropertyAST item in this.Properties)
            {
                item.AnalyText();
            }
        }

        public override void AnalyType()
        {
            foreach (PropertyAST item in this.Properties)
            {
                item.AnalyType();
            }
        }

        public override void AnalyBody()
        {
            foreach (PropertyAST item in this.Properties)
            {
                item.AnalyBody();
            }
        }

        public override void EmitName()
        {
            var classBuilder = this.ClassContext.GetTypeBuilder();
            bool isStatic = this.ClassContext.IsStatic();
            Type[] argTypes = Type.EmptyTypes;
            if(!isStatic)
            {
                argTypes = new Type[] { classBuilder };
            }
            MethodAttributes methodAttribute=MethodAttributes.Private | MethodAttributes.Static;
            initMethodBuilder = classBuilder.DefineMethod(CompileConst.InitMemberValueMethodName, methodAttribute, typeof(void), argTypes);
            if (!isStatic)
            {
                initMethodBuilder.DefineParameter(1, ParameterAttributes.None, CompileConst.InitMemberValueMethodParameterName);
            } 
            this.ClassContext.InitPropertyMethod = initMethodBuilder;
            this.PropertiesInitProcContext.SetBuilder (initMethodBuilder);
            //this.PropertiesInitProcContext.EmitContext.ILout = initMethodBuilder.GetILGenerator();
            foreach (PropertyAST item in this.Properties)
            {
                item.EmitName();
            }
        }
        MethodBuilder initMethodBuilder;
        public override void EmitBody()
        {
            ILGenerator il = initMethodBuilder.GetILGenerator();
            foreach (PropertyAST item in this.Properties)
            {
                item.EmitBody();
            }
            il.Emit(OpCodes.Ret);
        }

        public void SetContext(ContextClass classContext)
        {
            this.ClassContext = classContext;
            this.FileContext = this.ClassContext.FileContext;
            PropertiesInitProcContext = new ContextMethod(this.ClassContext);
            foreach (PropertyAST item in this.Properties)
            {
                item.SetContext(this.PropertiesInitProcContext);
            }
        }

        public override string ToString()
        {
            StringBuilder buff = new StringBuilder();
            if (this.TypeToken != null)
            {
                buff.Append(this.TypeToken.GetText());
            }
            buff.Append("属性:");

            for (int i = 0; i < this.Properties.Count;i++ )
            {
                var p = this.Properties[i];
                buff.Append(p.ToString());
                if (i < this.Properties.Count - 1)
                    buff.Append(",");
            }
            return buff.ToString();
        }
    }
}
