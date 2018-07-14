using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Tools;
using ZCompileDesc;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;
using Z语言系统;

namespace ZCompileCore.AST.Exps
{
    public class ExpDi : Exp , IEmitSet
    {
        
        private Exp _SubjectExp;
        public Exp SubjectExp { get { return _SubjectExp; } set { _SubjectExp = value; _SubjectExp.ParentExp = this; } }
        public LexTokenText KeyToken { get; set; }
        private Exp _ArgExp;
        public Exp ArgExp { get { return _ArgExp; } set { _ArgExp = value; _ArgExp.ParentExp = this; } }
        public LexTokenText GeToken { get; set; }
        public Exp ElementTypeExp { get; set; }

        private PropertyInfo Property;

        public ExpDi(ContextExp expContext)
            : base(expContext)
        {

        }

        public override void SetParent(Exp parentExp)
        {
            ParentExp = parentExp;
            SubjectExp.SetParent(this);
            ArgExp.SetParent(this);
        }

        public override Exp[] GetSubExps()
        {
            return new Exp[] { SubjectExp, ArgExp };
        }		

        public override Exp Analy( )
        {
            if (this.IsAnalyed) return this;
            SubjectExp = AnalySubExp(SubjectExp);
            ArgExp = AnalySubExp(ArgExp);
            if (!this.AnalyCorrect) return this;

            var propertyName = ZLangUtil.ZListItemPropertyName;
            var subjType = SubjectExp.RetType;
            if(subjType is ZLClassInfo)
            {
                ZLClassInfo zclass = subjType as ZLClassInfo;
                Type argType = ZTypeUtil.GetTypeOrBuilder(ArgExp.RetType);
                Property = zclass.SharpType.GetProperty(propertyName, new Type[] { argType });
            }

            if (Property == null)
            {
                Errorf(SubjectExp.Position, "不存在索引");
            }
            else
            {
                RetType = ZTypeManager.GetBySharpType( Property.PropertyType) as ZType;
            }
            IsAnalyed = true;
            return this;
        }
        
        public override void Emit()
        {
            EmitGet();
            base.EmitConv();
        }

        public void EmitGet( )
        {
            MethodInfo getMethod = Property.GetGetMethod();
            SubjectExp.Emit();
            ArgExp.RequireType = ZTypeManager.GetBySharpType(getMethod.ReturnType) as ZType;
            ArgExp.Emit();
            EmitHelper.CallDynamic(IL, getMethod);
        }
        
        public void EmitSet(Exp valueExp)
        {
            MethodInfo setMethod = Property.GetSetMethod();
            SubjectExp.Emit();
            ArgExp.RequireType = ZTypeManager.GetBySharpType(setMethod.GetParameters()[0].ParameterType) as ZType;
            ArgExp.Emit();
            //EmitHelper.Box(il, ArgExp.RetType, setMethod.GetParameters()[0].ParameterType);
            valueExp.RequireType = ZTypeManager.GetBySharpType(setMethod.GetParameters()[1].ParameterType) as ZType;
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
            return string.Format("{0}第{1}",SubjectExp.ToString(),ArgExp.ToString());
        }
    }
}
