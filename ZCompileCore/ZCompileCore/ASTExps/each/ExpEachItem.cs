using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Symbols;
using ZCompileDesc.Descriptions;
using ZLangRT.Utils;
using ZCompileCore.Tools;
using System.Reflection;
using ZCompileKit.Tools;
using ZCompileDesc.Utils;
using ZCompileDesc.ZTypes;
using ZCompileDesc;

namespace ZCompileCore.AST
{
    public class ExpEachItem : Exp, ISetter
    {
        public SymbolLocalVar ListSymbol;
        public SymbolLocalVar IndexSymbol;
        PropertyInfo Property;

        public ExpEachItem(ContextExp expContext, SymbolLocalVar listSymbol, SymbolLocalVar indexSymbol)
        {
            ExpContext = expContext;
            ListSymbol = listSymbol;
            IndexSymbol = indexSymbol;
        }

        public override Exp Analy( )
        {
            var subjType = ListSymbol.SymbolZType;
            
            ZClassType zclass = subjType as ZClassType;
            Property = zclass.SharpType.GetProperty(CompileConst.ZListItemPropertyName);
            RetType = ZTypeManager.GetBySharpType( Property.PropertyType) as ZType;
            return this;
        }
        
        public override void Emit()
        {
            EmitGet();
            base.EmitConv();
        }

        public override Exp[] GetSubExps()
        {
            return new Exp[] { };
        }		

        public void EmitGet( )
        {
            MethodInfo getMethod = Property.GetGetMethod();
            EmitHelper.LoadVar(IL,ListSymbol.VarBuilder);
            EmitHelper.LoadVar(IL,IndexSymbol.VarBuilder);
            EmitHelper.CallDynamic(IL, getMethod);
        }
        
        public void EmitSet(Exp valueExp)
        {
            MethodInfo setMethod = Property.GetSetMethod();
            EmitHelper.LoadVar(IL,ListSymbol.VarBuilder);
            EmitHelper.LoadVar(IL, IndexSymbol.VarBuilder);
            valueExp.Emit();
            EmitHelper.CallDynamic(IL, setMethod);
        }

        public bool CanWrite
        {
            get
            {
                return Property.CanWrite;
            }
        }

        public override string ToString()
        {
            return string.Format("{0}第{1}",ListSymbol.SymbolName,IndexSymbol.SymbolName);
        }
    }
}

