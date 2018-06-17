using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.ASTRaws;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileDesc;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;

namespace ZCompileCore.AST
{
    public class ProcMethod : ProcAST
    {
        private MethodName NamePart;
        public ContextMethod MethodContext { get; protected set; }

        public override ContextProc GetContextProc()
        {
            return MethodContext;
        }

        public override void AnalyBody()
        {
            this.Body.Analy();
        }

        public ProcMethod(ClassAST classAST, SectionProcRaw raw)
        {
            ASTClass = classAST;
            Raw = raw;
            MethodContext = new ContextMethod(ASTClass.ClassContext);
            NamePart = new MethodName(this, Raw.NamePart);
            Body = new StmtBlock(this,Raw.Body);
        }

        public void AnalyName()
        {
            NamePart.Analy();
        }

        public void AnalyType()
        {
            AnalyRet();
            if (this.ASTClass.ClassContext.IsStatic())
            {
                MethodContext.SetIsStatic(true);
            }
        }

        public void AnalyExpDim()
        {
            this.Body.AnalyExpDim();
        }

        private void AnalyRet()
        {
            RetZType = null;
            if (Raw.RetToken == null)
            {
                RetZType = ZLangBasicTypes.ZVOID;
            }
            else
            {
                string retText = Raw.RetToken.Text;
                var ztypes = this.ASTClass.FileContext.ImportUseContext.SearchZTypesByClassNameOrDimItem(retText);
                if (ztypes.Length == 1)
                {
                    RetZType = ztypes[0];
                }
                else
                {
                    RetZType = ZLangBasicTypes.ZVOID;
                    ASTClass.ClassContext.FileContext.Errorf(Raw.RetToken.Position, "过程的结果'{0}'不存在", Raw.RetToken.Text);
                }
            }
            MethodContext.RetZType = RetZType;       
        }

        public void EmitBody()
        {
            var IL = this.MethodContext.GetILGenerator(); 
            List<ZCLocalVar> localList = this.MethodContext.LocalManager.LocalVarList;
            EmitLocalVar(IL, localList);
            Body.Emit();
            if (!ZTypeUtil.IsVoid(this.RetZType))
            {
                IL.Emit(OpCodes.Ldloc_0);
            }
            IL.Emit(OpCodes.Ret);
            CreateNestedType();
        }

        public void EmitName()
        {
            var classBuilder = this.MethodContext.ClassContext.GetTypeBuilder();
            bool isStatic = this.MethodContext.ClassContext.IsStatic();
            ZCMethodDesc ProcDesc = NamePart.GetZDesc();
            List<Type> argTypes = new List<Type>();
            var parameters = MethodContext.ZMethodInfo.ZParams;
            foreach (var zparam in parameters)
            {
                argTypes.Add(ZTypeUtil.GetTypeOrBuilder(zparam.ZParamType));
            }
            var MethodName = NamePart.GetMethodName();
            MethodAttributes methodAttributes;
            if (isStatic)
            {
                methodAttributes = MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig;
            }
            else
            {
                methodAttributes = MethodAttributes.Public | MethodAttributes.Virtual;
            }
            MethodBuilder methodBuilder = classBuilder.DefineMethod(MethodName, methodAttributes,
              ZTypeUtil.GetTypeOrBuilder(RetZType), argTypes.ToArray());
            if (MethodName == "启动")
            {
                Type myType = typeof(STAThreadAttribute);
                ConstructorInfo infoConstructor = myType.GetConstructor(new Type[] { });
                CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(infoConstructor, new object[] { });
                methodBuilder.SetCustomAttribute(attributeBuilder);
            }
            else
            {
                string code = this.NamePart.GetZDesc().ToZCode();
                ASTUtil.SetAttrZCode(methodBuilder, code);
            }
            this.MethodContext.SetBuilder(methodBuilder);
            this.NamePart.EmitName();
             
        }    
    }
}
