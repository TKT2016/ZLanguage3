using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using ZCompileCore.AST.Exps;
using ZCompileCore.Tools;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;
using ZCompileCore;
using ZCompileCore.Contexts;

namespace ZCompileCore.AST.Exps
{
    public class ExpNewLambda : Exp
    {
        private ExpLambdaBody lambdaExp;
        private LambdaOutModel lambdaInfo;

        public ExpNewLambda(ContextExp expContext, Exp actionExp, ZType funcType)
            : base(expContext)
        {
            lambdaInfo = new LambdaOutModel(actionExp,funcType);
        }

        protected override void AnalyBody()
        {
            CheckRetType();
            lambdaExp = new ExpLambdaBody(this.ExpContext,lambdaInfo);
            lambdaExp.Analy();
        }

        private bool CheckRetType()
        {
            if (lambdaInfo.ActionExp.RetType == null)
            {
                throw new CCException();
            }
            else if (ZTypeUtil.IsConditionFn(lambdaInfo.FnRetType))
            {
                if (!ZTypeUtil.IsBool(lambdaInfo.ActionExp.RetType))
                {
                    Errorf(lambdaInfo.ActionExp.Position, "结果应该是" + lambdaInfo.FnRetType.ZTypeName);
                    return false;
                }
            }
            else if (ZTypeUtil.IsAction(lambdaInfo.FnRetType))
            {
                return true;
            }
            return true;
        }

        public override void Emit()
        {
            lambdaExp.Emit();
            LocalBuilder lanmbdaLocalBuilder = this.ProcContext.NestedInstance.VarBuilder;
            EmitInitOutField(lanmbdaLocalBuilder);
            EmitInitArg(lanmbdaLocalBuilder);
            //EmitInitLocal(lanmbdaLocalBuilder);
            EmitHelper.LoadVar(IL, lanmbdaLocalBuilder);
            IL.Emit(OpCodes.Ldftn, lambdaExp.ProcBuilder);
            ConstructorInfo[] constructorInfos = null;
            object retObj = lambdaInfo.FnRetType;
            if (retObj is ZLType)
            {
                constructorInfos = ((ZLType)retObj).SharpType.GetConstructors();
            }
            else 
            {
                throw new CCException();
            }
            IL.Emit(OpCodes.Newobj, constructorInfos[0]);
            base.EmitConv();
        }

        private void EmitInitArg(LocalBuilder lanmbdaLocalBuilder)
        {
            if (this.ProcContext.ArgList.Count > 0)
            {
                foreach (var arg in this.ProcContext.ArgList)
                {
                    ZCParamInfo paramSymbol = this.ProcContext.GetParameter(arg);
                    ZCFieldInfo fieldSymbol = this.ProcContext.GetNestedClassContext().MasterArgDict[arg];
                    EmitHelper.LoadVar(IL, lanmbdaLocalBuilder);
                    EmitSymbolHelper.EmitLoad(IL, paramSymbol);
                    EmitSymbolHelper.EmitStorm(IL, fieldSymbol);
                }
            }

            //for (int i = 0; i < this.lambdaInfo.BodyZParams.Count; i++)
            //{
            //    ZCParamInfo paramSymbol = this.lambdaInfo.BodyZParams[i];
            //    ZCFieldInfo fieldSymbol = lambdaExp.lambdaBody.Get(paramSymbol.ZName);
            //    EmitHelper.LoadVar(IL, lanmbdaLocalBuilder);
            //    EmitSymbolHelper.EmitLoad(IL, paramSymbol);
            //    EmitSymbolHelper.EmitStorm(IL, fieldSymbol);
            //}
        }

        //private void EmitInitLocal(LocalBuilder lanmbdaLocalBuilder)
        //{
        //    for (int i = 0; i < this.lambdaInfo.BodyZVars.Count; i++)
        //    {
        //        ZCLocalVar varSymbol = this.lambdaInfo.BodyZVars[i];
        //        ZCFieldInfo fieldSymbol = lambdaExp.lambdaBody.Get(varSymbol.ZName);
        //        if (fieldSymbol == null) throw new CCException();
        //        EmitHelper.LoadVar(IL, lanmbdaLocalBuilder);
        //        EmitSymbolHelper.EmitLoad(IL, varSymbol);
        //        EmitSymbolHelper.EmitStorm(IL, fieldSymbol);
        //    }
        //}

        private void EmitInitOutField(LocalBuilder lanmbdaLocalBuilder)
        {
            if (this.ExpContext.ClassContext.IsStatic() == false)
            {
                ZCFieldInfo fieldSymbol = lambdaExp.lambdaBody.OutClassField;
                EmitHelper.LoadVar(IL, lanmbdaLocalBuilder);
                EmitHelper.EmitThis(IL, false);
                EmitSymbolHelper.EmitStorm(IL, fieldSymbol);
            }
        }

        //private void EmitIdent(IIdent varSymbol)
        //{
        //    if (varSymbol is ZCLocalVar)
        //    {
        //        EmitSymbolHelper.EmitLoad(IL, (ZCLocalVar)varSymbol);
        //    }
        //    else if (varSymbol is ZCParamInfo)
        //    {
        //        EmitSymbolHelper.EmitLoad(IL, (ZCParamInfo)varSymbol);
        //    }
        //    else
        //    {
        //        throw new CCException();
        //    }
        //}

        public override void SetParent(Exp parentExp)
        {
            ParentExp = parentExp;
        }

        public override Exp[] GetSubExps()
        {
           // return lambdaInfo.ActionExp.GetSubExps();
            return new Exp[] { };
        }

    }
}
