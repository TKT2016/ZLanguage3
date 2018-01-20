using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using ZCompileCore.ASTExps;
using ZCompileCore.Tools;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;
using ZCompileKit;
using ZCompileKit.Tools;

namespace ZCompileCore.ASTExps
{
    public class ExpNewLambda : Exp
    {
        ExpLambdaBody lambdaExp;
        LambdaOutModel lambdaInfo;

        public ExpNewLambda( Exp actionExp, ZType funcType)
        {
            lambdaInfo = new LambdaOutModel(actionExp,funcType);
        }

        public override Exp Analy( )
        {
            if (this.IsAnalyed) return this;
            ChectRetType();
            lambdaExp = new ExpLambdaBody(this.ExpContext,lambdaInfo);
            lambdaExp.Analy();
            IsAnalyed = true;
            return this;
        }

        private bool ChectRetType()
        {
            if (lambdaInfo.ActionExp.RetType == null)
            {
                throw new CCException();
            }
            else if (ZTypeUtil.IsConditionFn(lambdaInfo.FnRetType))
            {
                if (ZTypeUtil.IsBool(lambdaInfo.ActionExp.RetType))
                {
                    ErrorF(lambdaInfo.ActionExp.Position, "结果应该是" + lambdaInfo.FnRetType.ZTypeName);
                    return false;
                }
            }
            else if (ZTypeUtil.IsAction(lambdaInfo.FnRetType))// (fnRetType.SharpType == typeof(Action))
            {
                return true;
            }
            return true;
        }

        public override void Emit()
        {
            lambdaExp.Emit();
            LocalBuilder lanmbdaLocalBuilder = IL.DeclareLocal(lambdaExp.NestedClassContext.EmitContext.ClassBuilder);
            IL.Emit(OpCodes.Newobj, lambdaExp.NewBuilder);
            EmitHelper.StormVar(IL, lanmbdaLocalBuilder);

            EmitInitOutField(lanmbdaLocalBuilder);
            EmitInitArg(lanmbdaLocalBuilder);
            EmitInitLocal(lanmbdaLocalBuilder);
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
                //constructorInfos = ((ZCClassInfo)lambdaExp.FnRetType).SharpType.GetConstructors();
                throw new CCException();
            }
            IL.Emit(OpCodes.Newobj, constructorInfos[0]);
            base.EmitConv();
        }

        private void EmitInitLocal(LocalBuilder lanmbdaLocalBuilder)
        {
            for (int i = 0; i < this.lambdaInfo.BodyZVars.Count; i++)
            {
                ZCLocalVar varSymbol = this.lambdaInfo.BodyZVars[i];
                ZCFieldInfo fieldSymbol = lambdaExp.lambdaBody.Get(varSymbol.ZName);//.FieldSymbols[i];
                EmitHelper.LoadVar(IL, lanmbdaLocalBuilder);
                EmitSymbolHelper.EmitLoad(IL, varSymbol);
                EmitSymbolHelper.EmitStorm(IL, fieldSymbol);
            }
        }

        private void EmitInitArg(LocalBuilder lanmbdaLocalBuilder)
        {
            for (int i = 0; i < this.lambdaInfo.BodyZParams.Count; i++)
            {
                ZCParamInfo paramSymbol = this.lambdaInfo.BodyZParams[i];
                ZCFieldInfo fieldSymbol = lambdaExp.lambdaBody.Get(paramSymbol.ZName);
                EmitHelper.LoadVar(IL, lanmbdaLocalBuilder);
                EmitSymbolHelper.EmitLoad(IL, paramSymbol);
                EmitSymbolHelper.EmitStorm(IL, fieldSymbol);
            }
        }

        private void EmitInitOutField(LocalBuilder lanmbdaLocalBuilder)
        {
            if (this.ExpContext.ClassContext.IsStatic() == false)
            {
                ZCFieldInfo fieldSymbol = lambdaExp.lambdaBody.OutClassField;//.FieldSymbols[0];
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

        public override Exp[] GetSubExps()
        {
            return lambdaInfo.ActionExp.GetSubExps();
        }

    }
}
