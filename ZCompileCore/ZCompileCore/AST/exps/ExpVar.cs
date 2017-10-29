using ZCompileCore.Lex;
using ZCompileCore.Symbols;
using ZCompileCore.Tools;
using System.Reflection.Emit;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Words;
using System.Reflection;
using ZCompileKit;
using ZCompileKit.Tools;
using ZCompileDesc.ZTypes;
using ZCompileDesc;

namespace ZCompileCore.AST
{
    public class ExpVar : Exp, ISetter
    {
        public Token VarToken { get; set; }

        public string VarName { get { return VarToken.GetText(); } }
        public SymbolBase VarSymbol { get; set; }

        public void SetAssigned(ZType retType)
        {
            RetType = retType;
        }

        public Exp AnalyDim()
        {
            SymbolLocalVar localVarSymbol = new SymbolLocalVar(VarName, RetType);
            localVarSymbol.LoacalVarIndex = this.ExpContext.ProcContext.CreateLocalVarIndex(VarName);
            this.ProcContext.Symbols.Add(localVarSymbol);
            VarSymbol = localVarSymbol;

            WordInfo word = new WordInfo(VarName, WordKind.VarName,this);
            this.ExpContext.ProcContext.LoacalVarWordDictionary.Add(word);
            return this;
        }

        public override Exp Analy( )
        {
            var symbols = this.ProcContext.Symbols; 
            VarSymbol = symbols.Get(VarName);
            if (VarSymbol == null)
            {
                ErrorE(this.Position, "变量'{0}'不存在", VarName);
                //RetType = ZLangBasicTypes.ZOBJECT;
            }
            else
            {
                RetType = VarSymbol.SymbolZType;
            }
            return this;
        }

        SymbolDefField NestedFieldSymbol;
        public void SetAsLambdaFiled(SymbolDefField fieldSymbol)
        {
            NestedFieldSymbol = fieldSymbol;
        }

        #region Emit
        public override void Emit()
        {
            EmitGet();
        }
        /* 变量表达式会可以会在分析后转移到内部类中，所以只能一次确定 */
        bool? isArg;
        bool? isLocal;
        public bool IsArg()
        {
            if (isArg == null)
            {
                isArg = this.ExpContext.ProcContext.ProcVarWordDictionary.ContainsKey(VarName);
            }
            return isArg.Value;
        }

        public bool IsLocalVar()
        {
            if (isLocal == null)
            {
                isLocal = this.ExpContext.ProcContext.LoacalVarWordDictionary.ContainsKey(VarName);
            }
            return isLocal.Value;
        }

        public bool IsVar()
        {
            return IsArg() || IsLocalVar();
        }

        public void EmitGet()
        {
            if (IsNested && IsVar())
            {
                if(this.NestedFieldSymbol!=null)
                {
                    EmitHelper.Emit_LoadThis(IL);
                    EmitSymbolHelper.EmitLoad(IL,this.NestedFieldSymbol);
                    base.EmitConv();
                }
                else if (VarSymbol is SymbolDefProperty)
                {
                    if (this.ClassContext.IsStaticClass == false)
                    {
                        EmitHelper.Emit_LoadThis(IL);
                        EmitHelper.LoadField(IL, this.ClassContext.NestedOutFieldSymbol.Field);
                    }
                    EmitSymbolHelper.EmitLoad(IL, VarSymbol);
                    base.EmitConv();
                }
                else
                {
                    throw new CompileCoreException();
                }
            }
            else
            {
                if (EmitSymbolHelper.NeedCallThis(VarSymbol))
                {
                    EmitHelper.Emit_LoadThis(IL);
                }
                EmitSymbolHelper.EmitLoad(IL, VarSymbol);
                base.EmitConv();
            }
        }

        public void EmitSet( Exp valueExp)
        {
            if (IsNested)
            {
                if (this.NestedFieldSymbol != null)
                {
                    EmitHelper.Emit_LoadThis(IL);
                    EmitValueExp(valueExp);
                    EmitSymbolHelper.EmitStorm(IL, this.NestedFieldSymbol);
                }
                else if (VarSymbol is SymbolDefProperty)
                {
                    if (this.ClassContext.IsStaticClass == false)
                    {
                        EmitHelper.Emit_LoadThis(IL);
                        EmitHelper.LoadField(IL, this.ClassContext.NestedOutFieldSymbol.Field);
                    }
                    EmitValueExp(valueExp);
                    EmitSymbolHelper.EmitStorm(IL, VarSymbol);
                }
                else
                {
                    throw new CompileCoreException();
                }
            }
            else
            {
                if (EmitSymbolHelper.NeedCallThis(VarSymbol))
                {
                    EmitHelper.Emit_LoadThis(IL);
                }
                EmitValueExp(valueExp);
                EmitSymbolHelper.EmitStorm(IL, VarSymbol);
                base.EmitConv();
            }
        }

        private void EmitValueExp(Exp valueExp)
        {
            valueExp.RequireType = this.RetType;
            valueExp.Emit();
        }

        #endregion


        #region 覆盖

        public bool CanWrite
        {
            get
            {
                return VarSymbol.CanWrite;
            }
        }

        public override Exp[] GetSubExps()
        {
            return new Exp[] { };
        }

        public override string ToString()
        {
            return VarToken.GetText();
        }

        public override CodePosition Position
        {
            get
            {
                return VarToken.Position;
            }
        }
        #endregion
    }
}
