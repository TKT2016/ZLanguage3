using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST;
using ZCompileCore.Lex;

using ZCompileCore.Tools;
using ZCompileDesc.Descriptions;
using ZCompileCore.Contexts;


namespace ZCompileCore.AST.Exps
{
    /// <summary>
    /// 父类中的字段
    /// </summary>
    public class ExpFieldSuper : ExpFieldBase
    {
        ZLFieldInfo ZField;

        public ExpFieldSuper(ContextExp expContext, LexToken token)
            : base(expContext)
        {
            VarToken = token;
        }

        public override Exp Analy()
        {
            if (this.IsAnalyed) return this;
            if (this.ExpContext == null) throw new CCException();
            VarName = VarToken.Text;
            ZLClassInfo zbase = this.ClassContext.GetSuperZType();
            ZField = zbase.SearchField(VarName);
            if (ZField == null) throw new CCException();
            RetType = ZField.ZFieldType;
            IsAnalyed = true;
            return this;
        }

        protected override System.Reflection.FieldInfo GetFieldInfo()
        {
            return ZField.SharpField;
        }

        protected override bool GetIsStatic()
        {
            return this.ZField.IsStatic;
        }

        #region Emit

        //public void EmitLoadFielda()
        //{
        //    bool isstatic = this.ZField.IsStatic;
        //    EmitHelper.EmitThis(IL, isstatic);
        //    EmitHelper.LoadFielda(IL, ZField.SharpField);
        //}

        //public void EmitGet()
        //{
        //    EmitHelper.EmitThis(IL, false);
        //    EmitSymbolHelper.EmitLoad(IL, ZField);
        //    //MethodInfo getMethod = (this.ZField as ZLFieldInfo).SharpProperty.GetGetMethod();
        //    //EmitHelper.CallDynamic(IL, getMethod);
        //    base.EmitConv();
        //}

        //public override void EmitSet(Exp valueExp)
        //{
        //    EmitSetProperty(valueExp);
        //}

        //private void EmitSetProperty(Exp valueExp)
        //{
        //    EmitHelper.EmitThis(IL, false);
        //    EmitValueExp(valueExp);
        //    EmitSymbolHelper.EmitStorm(IL, ZField);
        //    //MethodInfo setMethod = (this.ZField as ZLPropertyInfo).SharpProperty.GetSetMethod();
        //    //EmitHelper.CallDynamic(IL, setMethod);
        //    base.EmitConv();
        //}

        #endregion

        #region 覆盖

        public override bool CanWrite
        {
            get
            {
                return ZField.GetCanWrite();
            }
        }

        #endregion
    }
}
