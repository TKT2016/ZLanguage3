using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;

using ZCompileDesc.Descriptions;
using ZLangRT.Utils;
using ZCompileCore.Tools;
using System.Reflection;
using ZCompileKit.Tools;
using ZCompileDesc.Utils;

using ZCompileDesc;
using Z语言系统;

namespace ZCompileCore.ASTExps
{
    public class ExpEachItem : Exp, ISetter
    {
        public ZCLocalVar ListSymbol;
        public ZCLocalVar IndexSymbol;
        PropertyInfo Property;

        public ExpEachItem(ContextExp expContext, ZCLocalVar listSymbol, ZCLocalVar indexSymbol)
        {
            ExpContext = expContext;
            ListSymbol = listSymbol;
            IndexSymbol = indexSymbol;
        }

        public override Exp Analy( )
        {
            if (this.IsAnalyed) return this;
            var subjType = ListSymbol.GetZType();
            
            ZLClassInfo zclass = subjType as ZLClassInfo;
            Property = zclass.SharpType.GetProperty(ZLangUtil.ZListItemPropertyName);
            RetType = ZTypeManager.GetBySharpType( Property.PropertyType) as ZType;
            IsAnalyed = true;
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
            return string.Format("{0}第{1}",ListSymbol.ZName,IndexSymbol.ZName);
        }
    }
}

