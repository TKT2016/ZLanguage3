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
    public class ProcConstructorDefault : ProcConstructorBase
    {
        public override void AnalyBody()
        {
           
        }

        public ProcConstructorDefault(ClassAST classAST )
        {
            RetZType = ZLangBasicTypes.ZVOID;
            ASTClass = classAST;
            Raw = null;
            ConstructorContext = new ContextConstructor(ASTClass.ClassContext);
        }

        public override void AnalyExpDim()
        {
             
        }

        public override void EmitBody()
        {
            //if (this.ClassContext.ClassName == "子弹管理器")
            //{
            //    Debugr.WriteLine("子弹管理器");
            //}
            ILGenerator IL = this.ConstructorContext.GetILGenerator();
            EmitCallSuper(IL);
            EmitCallInitPropertyMethod(IL);
            IL.Emit(OpCodes.Ret);
        }

        public override void EmitName()
        {
            var classBuilder = this.ConstructorContext.ClassContext.GetTypeBuilder();
            if(classBuilder==null)
            {
                throw new CCException();
            }
            bool isStatic = this.ConstructorContext.IsStatic();
            MethodAttributes methodAttributes;
            CallingConventions callingConventions;

            if (isStatic)
            {
                methodAttributes = MethodAttributes.Private | MethodAttributes.Static;
                callingConventions = CallingConventions.Standard;
            }
            else
            {
                methodAttributes = MethodAttributes.Public;
                callingConventions = CallingConventions.HasThis;
            }
            var argTypes = new Type[] { };
            ConstructorBuilder constructorBuilder = classBuilder.DefineConstructor(methodAttributes, callingConventions, argTypes);
            ConstructorContext.SetBuilder(constructorBuilder);
        }
        
    }
}
