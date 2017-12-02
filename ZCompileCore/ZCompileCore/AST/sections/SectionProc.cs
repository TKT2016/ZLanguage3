﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Parsers;
using ZCompileCore.Symbols;
using ZLangRT.Attributes;
using ZLangRT.Utils;
using ZCompileDesc.Descriptions;
using Z语言系统;
using ZCompileDesc.Utils;
using ZCompileDesc.ZTypes;
using ZCompileDesc.ZMembers;
using ZCompileDesc;
using ZCompileDesc.Compilings;

namespace ZCompileCore.AST
{
    public class SectionProc : SectionClassBase
    {
        public ProcName NamePart;
        public LexToken RetToken;
        public StmtBlock Body;
        public ZType RetZType;
        ContextProc ProcContext;
        ZMethodCompiling methodCompiling;

        public override void AnalyText()
        {
            NamePart.AnalyText();
        }

        public override void AnalyType()
        {
            bool isStatic = this.ClassContext.IsStatic();
            AnalyRet();
            NamePart.AnalyType();
            ZMethodDesc zmdesc = NamePart.GetZDesc();
            methodCompiling = new ZMethodCompiling((ZMethodDesc)zmdesc, this.RetZType);
            methodCompiling.SetIsStatic(isStatic);
            this.ProcContext.SetMethodCompiling(methodCompiling);
        }

        public override void AnalyBody()
        {
            Body.Analy();
        }

        public override void EmitName()
        {
            //if (this.NamePart.ToString().IndexOf( "清除出界子弹")!=-1)
            //{
            //    Console.WriteLine("清除出界子弹");
            //}
            var classBuilder = this.ClassContext.GetTypeBuilder();// compilingType.ClassBuilder;
            bool isStatic = this.ClassContext.IsStatic();
            //var classBuilder = this.ProcContext.ClassContext.EmitContext.ClassBuilder;
            ZMethodDesc ProcDesc = NamePart.GetZDesc();
            List<Type> argTypes = new List<Type>();
            foreach (var zparam in ProcDesc.DefArgs)
            {
                argTypes.Add(zparam.ZParamType.SharpType);
            }
            var MethodName = NamePart.GetMethodName();
            MethodAttributes methodAttributes;
            //bool isStatic = (this.ProcContext.IsStatic);
            if (isStatic)
            {
                methodAttributes = MethodAttributes.Public | MethodAttributes.Static;
            }
            else
            {
                methodAttributes = MethodAttributes.Public | MethodAttributes.Virtual;
            }
            MethodBuilder methodBuilder = classBuilder.DefineMethod(MethodName, methodAttributes,
                RetZType.SharpType, argTypes.ToArray());
            if (MethodName == "启动")
            {
                Type myType = typeof(STAThreadAttribute);
                ConstructorInfo infoConstructor = myType.GetConstructor(new Type[] { });
                CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(infoConstructor, new object[] { });
                methodBuilder.SetCustomAttribute(attributeBuilder);
            }
            else
            {
                SetAttrZCode(methodBuilder);
            }
            this.ProcContext.SetBuilder(methodBuilder);
            this.NamePart.EmitName();
        }

        public override void EmitBody()
        {
            var IL = this.ProcContext.GetILGenerator();//.ILout;
            this.ProcContext.LoacalVarList.Reverse();
            for (int i = 0; i < this.ProcContext.LoacalVarList.Count; i++)
            {
                string ident = this.ProcContext.LoacalVarList[i];
                SymbolLocalVar varSymbol = this.ProcContext.GetDefLocal(ident);
                varSymbol.VarBuilder = IL.DeclareLocal(varSymbol.SymbolZType.SharpType);
                varSymbol.VarBuilder.SetLocalSymInfo(varSymbol.SymbolName);
            }

            Body.Emit();
            if (this.RetZType.SharpType != typeof(void))
            {
                IL.Emit(OpCodes.Ldloc_0);
            }
            IL.Emit(OpCodes.Ret);
        }

        private void SetAttrZCode(MethodBuilder methodBuilder)
        {
            Type myType = typeof(ZCodeAttribute);
            ConstructorInfo infoConstructor = myType.GetConstructor(new Type[] { typeof(string) });
            string code = this.NamePart.GetZDesc().ToZCode();
            CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(infoConstructor, new object[] { code });
            methodBuilder.SetCustomAttribute(attributeBuilder);
        }

        private bool AnalyRet()
        {
            if (RetToken == null)
            {
                RetZType = ZLangBasicTypes.ZVOID;
            }
            else
            {
                string retText = RetToken.GetText();
                var ztypes = this.FileContext.ImportUseContext.SearchZTypesByClassNameOrDimItem(retText);
                if (ztypes.Length == 1)
                {
                    RetZType = ztypes[0];
                    return true;
                }
                else
                {
                    RetZType = ZLangBasicTypes.ZVOID;
                    ErrorF(RetToken.Position, "过程的结果'{0}'不存在", RetToken.GetText());
                }
            }
            this.ProcContext.RetZType = RetZType;
            return false;
        }


        public void SetContext(ContextClass classContext)
        {
            this.ClassContext = classContext;
            this.FileContext = this.ClassContext.FileContext;
            this.ProcContext = new ContextProc(this.ClassContext,false);
            NamePart.SetContext(this.ProcContext);
            Body.ProcContext = this.ProcContext;
        }

        public override string ToString()
        {
            StringBuilder buff = new StringBuilder();
            buff.Append(this.NamePart.ToString());
            buff.Append(":");
            if (this.RetToken != null)
                buff.Append(this.RetToken.GetText());
            buff.AppendLine();
            buff.Append(this.Body.ToString());
            return buff.ToString();
        }
    }
}
