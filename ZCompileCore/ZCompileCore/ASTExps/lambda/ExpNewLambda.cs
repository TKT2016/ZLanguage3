using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using ZCompileCore.ASTExps;
using ZCompileCore.Symbols;
using ZCompileCore.Tools;
using ZCompileDesc.Compilings;
using ZCompileDesc.Descriptions;
using ZCompileDesc.ZTypes;
using ZCompileKit;
using ZCompileKit.Tools;

namespace ZCompileCore.AST
{
    public class ExpNewLambda : Exp
    {
        ExpLambdaBody lambdaExp;
        List<SymbolBase> BodySymbolVars;
        Exp actionExp;
        ZType fnRetType;

        public override Exp[] GetSubExps()
        {
            return actionExp.GetSubExps();
        }

        public ExpNewLambda( Exp actionExp, ZType funcType)
        {
            this.actionExp = actionExp;
            this.fnRetType = funcType;
        }

        List<ExpLocal> BodyExpVars;
        public override Exp Analy( )
        {
            if (actionExp.RetType==null)
            {
                throw new CCException();
            }
            else if (fnRetType.SharpType == typeof(Func<bool>))
            {
                if (actionExp.RetType.SharpType != typeof(bool))
                {
                    ErrorF(actionExp.Position, "结果应该是" + fnRetType.ZName);
                }
            }
            else if (fnRetType.SharpType == typeof(Action))
            {
                 
            }
            
            BodyExpVars = GetAllSubLocalExpVars(actionExp);
            BodySymbolVars = GetAllSubVars(BodyExpVars);

            lambdaExp = new ExpLambdaBody(actionExp, fnRetType, BodySymbolVars, BodyExpVars,this.ExpContext);
            lambdaExp.Analy();
            return this;
        }

        public override void Emit()
        {
            lambdaExp.Emit();
            LocalBuilder lanmbdaLocalBuilder = IL.DeclareLocal(lambdaExp.NestedClassContext.EmitContext.ClassBuilder);
            IL.Emit(OpCodes.Newobj, lambdaExp.NewBuilder);
            EmitHelper.StormVar(IL, lanmbdaLocalBuilder);
            int i = 0;
            if(this.ExpContext.ClassContext.IsStatic()==false)
            {
                ZMemberCompiling fieldSymbol = lambdaExp.FieldSymbols[0];
                EmitHelper.LoadVar(IL, lanmbdaLocalBuilder);
                EmitHelper.EmitThis(IL, false);
                EmitSymbolHelper.EmitStorm(IL,fieldSymbol);// IL.Emit(OpCodes.Stfld, fieldSymbol.Field);
                i++;
            }
            for (;i<this.BodySymbolVars.Count;i++)
            {
                SymbolBase thisSymbol = this.BodySymbolVars[i];
                ZMemberCompiling fieldSymbol = lambdaExp.FieldSymbols[i];

                EmitHelper.LoadVar(IL, lanmbdaLocalBuilder);
                if (EmitSymbolHelper.NeedCallThis(thisSymbol))
                {
                    EmitHelper.Emit_LoadThis(IL,false);
                }
                EmitSymbolHelper.EmitLoad(IL, thisSymbol);
                EmitSymbolHelper.EmitStorm(IL, fieldSymbol);// IL.Emit(OpCodes.Stfld, fieldSymbol.Field);
            }

            EmitHelper.LoadVar(IL, lanmbdaLocalBuilder);
            IL.Emit(OpCodes.Ldftn, lambdaExp.ProcBuilder);
            ConstructorInfo[] constructorInfos = lambdaExp.FnRetType.SharpType.GetConstructors();
            IL.Emit(OpCodes.Newobj, constructorInfos[0]);
            base.EmitConv();
        }

        #region 取得表达式内所有变量
        private List<ExpLocal> GetAllSubLocalExpVars(Exp exp)
        {
            List<Exp> exps = GetAllSubExps(exp);
            List<ExpLocal> results = new List<ExpLocal>();
            foreach (var item in exps)
            {
                if (item is ExpLocal)
                {
                    ExpLocal varExp = item as ExpLocal;
                    if ( results.IndexOf(varExp) == -1)// (varExp.IsVar() && results.IndexOf(varExp) == -1)
                    {
                        results.Add(varExp);
                    }
                }
            }
            return results;
        }

        private List<SymbolBase> GetAllSubVars(List<ExpLocal> exps)
        {
            throw new CCException();

            //List<SymbolBase> results = new List<SymbolBase>();
            //foreach(var item in exps)
            //{
            //    if (item is ExpLocal)
            //    {
            //        ExpLocalVar varExp = item as ExpLocalVar;
            //        results.Add(varExp.GetSymbol());
            //        //if (varExp.VarSymbol is SymbolArg || varExp.VarSymbol is SymbolLocalVar)
            //        //{
            //        //    if(results.IndexOf(varExp.VarSymbol)==-1)
            //        //    {
            //        //        results.Add(varExp.VarSymbol);
            //        //    }
            //        //}
            //    }
            //}
            //return results;
        }

        private List<Exp> GetAllSubExps(Exp exp)
        {
            List<Exp> list = new List<Exp>();
            AddSubExp(exp, list);
            return list;
        }

        private void AddSubExp(Exp exp,List<Exp> list )
        {
            list.Add(exp);
            Exp[] subexps = exp.GetSubExps();
            foreach (var expsub in subexps)
            {
                AddSubExp(expsub, list);
            }
        }
        #endregion

    }
}
