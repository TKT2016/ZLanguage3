using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.Contexts;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;

namespace ZCompileCore.Tools
{
    public static class BuilderUtil
    {
        public static MethodAttributes GetMethodAttr(bool isStatic)
        {
            if (isStatic)
                return MethodAttributes.Public | MethodAttributes.Static;
            else
                return MethodAttributes.Public;
        }

        public static FieldAttributes GetFieldAttr(bool isStatic)
        {
            if (isStatic)
                return FieldAttributes.Private | FieldAttributes.Static;
            else
                return FieldAttributes.Private;
        }

        public static void EmitLocalVar(ContextProc procContext,bool isStatic   ,ILGenerator IL, List<ZCLocalVar> localList)
        {
            localList.Reverse();
            for (int i = 0; i < localList.Count; i++)
            {
                ZCLocalVar varSymbol = localList[i];
                varSymbol.VarBuilder = IL.DeclareLocal(ZTypeUtil.GetTypeOrBuilder(varSymbol.GetZType()));
                varSymbol.VarBuilder.SetLocalSymInfo(varSymbol.ZName);
            }
            for (int i = 0; i < localList.Count; i++)
            {
                ZCLocalVar varSymbol = localList[i];
                if (varSymbol.IsNestedClassInstance)
                {
                    LocalBuilder lanmbdaLocalBuilder = procContext.NestedInstance.VarBuilder;
                    ConstructorBuilder newBuilder = procContext.GetNestedClassContext().DefaultConstructorBuilder;
                    IL.Emit(OpCodes.Newobj, newBuilder);
                    EmitHelper.StormVar(IL, lanmbdaLocalBuilder);
                    if (!isStatic)
                    {
                        ZCFieldInfo masterClassField = procContext.GetNestedClassContext().MasterClassField;
                        if(masterClassField!=null)
                        ILGeneratorUtil.LoadLocal(IL, lanmbdaLocalBuilder);
                        IL.Emit(OpCodes.Ldarg_0);
                        EmitSymbolHelper.EmitStorm(IL, masterClassField);
                    }
                }
            }
        }
    }
}
