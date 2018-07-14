using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST.Exps;
using ZCompileCore.ASTRaws;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Parsers.Raws;
using ZCompileCore.Tools;
using ZCompileDesc;
using ZCompileDesc.Descriptions;

namespace ZCompileCore.AST
{
    public class SectionPropertiesClass
    {
        public SectionPropertiesRaw Raw;
        //public Dictionary<string, LexTokenText> dict = new Dictionary<string, LexTokenText>();
        internal ClassAST ASTClass;
        private List<ClassPropertyAST> Propertys = new List<ClassPropertyAST>();
        public ContextMethod PropertiesContext { get; protected set; }

        public SectionPropertiesClass(ClassAST classAST, SectionPropertiesRaw raw)
        {
            ASTClass = classAST;
            Raw = raw;
            PropertiesContext = new ContextMethod(ASTClass.ClassContext);
            for (int i = 0; i < Raw.Properties.Count; i++)
            {
                PropertyASTRaw item = Raw.Properties[i];
                ClassPropertyAST p = new ClassPropertyAST(this, item);
                Propertys.Add(p);
            }
        }

        public void AnalyNames()
        {
            //Propertys.Clear();
            for (int i = 0; i < Propertys.Count; i++)
            {
                ClassPropertyAST p = Propertys[i];
                p.AnalyName();     
            }
        }

        public void AnalyTypes()
        {
            for (int i = 0; i < Propertys.Count; i++)
            {
                ClassPropertyAST p = Propertys[i];
                p.AnalyType();
            }
        }

        public void AnalyExpDim()
        {
            for (int i = 0; i < Propertys.Count; i++)
            {
                ClassPropertyAST p = Propertys[i];
                p.AnalyExpDim();
            }
        }

        public void AnalyBodys()
        {
            return;
        }

        public void EmitNames()
        {
            foreach(var item in Propertys)
            {
                item.EmitName();
            }
        }

        MethodAttributes methodAttribute = MethodAttributes.Private | MethodAttributes.Static;
        public MethodBuilder initMethodBuilder { get; private set; }

        public void EmitBodys()
        {
            //if(this.ClassContext.ClassName=="子弹管理器")
            //{
            //    Debugr.WriteLine("子弹管理器");
            //}
            bool isStatic = this.ClassContext.IsStatic();
            var classBuilder = this.ClassContext.GetTypeBuilder();
            Type[] argTypes = Type.EmptyTypes;
            if (!isStatic)
            {
                argTypes = new Type[] { classBuilder };
            }
            initMethodBuilder = classBuilder.DefineMethod(CompileConst.InitMemberValueMethodName, methodAttribute, typeof(void), argTypes);
            if (!isStatic)
            {
                initMethodBuilder.DefineParameter(1, ParameterAttributes.None, CompileConst.InitMemberValueMethodParameterName);
            }
            this.ClassContext.InitPropertyMethod = initMethodBuilder;
            this.PropertiesContext.SetBuilder(initMethodBuilder);
            foreach (var item in Propertys)
            {
                item.EmitBody();
            }
            var method = this.ClassContext.InitPropertyMethod;
            var IL = method.GetILGenerator();
            IL.Emit(OpCodes.Ret);
            
        }

        private ContextClass ClassContext
        {
            get
            {
                return this.ASTClass.ClassContext;
            }
        }
    }
}
