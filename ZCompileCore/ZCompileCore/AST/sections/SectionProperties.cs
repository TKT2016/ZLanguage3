using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Parsers;
using ZCompileCore.Symbols;
using ZCompileCore.Tools;
using ZCompileDesc.Descriptions;

namespace ZCompileCore.AST
{
    public class SectionProperties : SectionBase
    {
        public Token TypeToken;
        public Token KeyToken;
        public List<PropertyAST> Properties = new List<PropertyAST>();
        public ContextClass ClassContext;
        ContextProc ProcContext;

        public void AddProperty(PropertyAST property)
        {
            Properties.Add(property);
        }

        public void AnalyName(NameTypeParser parser)
        {
            this.ProcContext = new ContextProc(this.ClassContext);
            foreach(var item in Properties)
            {
                item.Analy(parser, this.ClassContext.PropertyContext, this.ProcContext);
            }
        }

        public void EmitName(bool isStatic, TypeBuilder classBuilder)
        {
            MethodAttributes methodAttr = MethodAttributes.Private;
            if (isStatic)
                methodAttr = MethodAttributes.Private | MethodAttributes.Static;
            InitPropertyMethod = classBuilder.DefineMethod("__InitPropertyMethod", methodAttr, typeof(void), null);
            this.ClassContext.InitPropertyMethod = this.InitPropertyMethod;
            foreach (var item in Properties)
            {
                item.EmitName( isStatic,  classBuilder);
            }
        }

        public MethodBuilder InitPropertyMethod { get; private set; }

        public void EmitValue(bool isStatic, TypeBuilder classBuilder)
        {
            ILGenerator il = InitPropertyMethod.GetILGenerator();
            this.ProcContext.EmitContext.ILout = il;
            foreach (var item in Properties)
            {
                item.EmitValue(isStatic,il);
            }
            il.Emit(OpCodes.Ret);
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
