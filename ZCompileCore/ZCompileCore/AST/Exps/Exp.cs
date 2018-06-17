using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Tools;
using ZCompileDesc;
using ZCompileDesc.Descriptions;

namespace ZCompileCore.AST.Exps
{
    public abstract class Exp
    {
        public bool IsAnalyed { get; protected set; }       
        public Exp ParentExp { get; internal set; }

        public virtual ZType RetType { get; set; }
        public virtual ZType RequireType { get; set; }

        public abstract void SetParent(Exp parentExp);

        public Exp(ContextExp expContext)
        {
            AnalyCorrect = true;
            ParentExp = null;
            _ExpContext = expContext;
        }

        #region Context
        protected ContextExp _ExpContext;
        public ContextExp ExpContext 
        {
            get
            {
                if (this.ParentExp != null)
                {
                    return this.ParentExp.ExpContext;
                }
                else
                {
                    return _ExpContext;
                }
            }
        }

        public ContextProc ProcContext { get { return this.ExpContext.ProcContext; } }
        public ContextClass ClassContext { get { return this.ProcContext.ClassContext; } }
        public ContextFile FileContext { get { return this.ClassContext.FileContext; } }
        public ContextProject ProjectContext { get { return this.FileContext.ProjectContext; } }

        public virtual void SetContextExpForce(ContextExp expContext)
        {
            _ExpContext = expContext;
            //ParentExp = null;
            this.SetParent(null);
            //var subs = GetSubExps();
            //if (subs.Length > 0)
            //{
            //    foreach (var exp in subs)
            //    {
            //        exp.SetContextExpForce(_ExpContext);
            //    }
            //}
        }

        #endregion

        public virtual Exp Parse()
        {
            return this;
        }

        public virtual Exp Analy()
        {
            if (this.IsAnalyed) return this;
            AnalyBody();
            IsAnalyed = true;
            return this;
        }

        public virtual void AnalyDim()
        {
            Exp[] subs = GetSubExps();
            if (subs.Length > 0)
            {
                foreach (Exp item in subs)
                {
                    item.AnalyDim();
                }
            }
        }

        protected virtual void AnalyBody()
        {
            return;
        }

        public virtual void Emit()
        {
            throw new CCException();
        }

        public virtual Exp[] GetSubExps()
        {
            return new Exp[]{};
        }

        protected void Errorf(CodePosition postion, string msgFormat, params string[] msgParams)
        {
            ASTUtil.Errorf(this.FileContext , postion, msgFormat, msgParams);
        }

        public virtual CodePosition Position { get { return new CodePosition (0,0); } } 

        public void CopyFieldsToExp(Exp newExp)
        {
            newExp.IsAnalyed = this.IsAnalyed;
            //newExp.IsTopExp = this.IsTopExp;
            //newExp.IsDim = this.IsDim;
            //newExp.ExpContext = this.ExpContext;
            newExp.RequireType = this.RequireType;
            //newExp.RetType = this.RetType;
        }

        protected virtual void EmitLoadMain()
        {
            if (this.ClassContext is ContextNestedClass)
            {
                ContextNestedClass nestedContext = (this.ClassContext as ContextNestedClass);
                if (!nestedContext.MasterClassIsStatic)
                {
                    EmitHelper.LoadField(IL, nestedContext.MasterClassField.FieldBuilder);
                }
            }
            else
            {
                bool isstatic = this.ClassContext.IsStatic();// GetIsStatic();
                EmitHelper.EmitThis(IL, isstatic);
            }
        }

        public bool AnalyCorrect { get; set; }
        //public bool IsNested { get; protected set; }
        //public virtual void SetIsNested(bool b)
        //{
        //    this.IsNested = b;
        //    Exp[] subs = GetSubExps();
        //    if (subs != null && subs.Length > 0)
        //    {
        //        foreach (var exp in subs)
        //        {
        //            if (exp != null)
        //            {
        //                exp.SetIsNested(b);
        //            }
        //        }
        //    }
        //}

        //public virtual void SetParent(Exp context)
        //{
        //    //this.ExpContext = context;
        //    Exp[] subs = GetSubExps();
        //    if (subs != null && subs.Length > 0)
        //    {
        //        foreach (var exp in subs)
        //        {
        //            if (exp != null)
        //            {
        //                exp.SetContext(context);
        //            }
        //        }
        //    }
        //}

        //public virtual void SetContext(ContextExp context)
        //{
        //    //this.ExpContext = context;
        //    Exp[] subs = GetSubExps();
        //    if (subs != null && subs.Length > 0)
        //    {
        //        foreach (var exp in subs)
        //        {
        //            if (exp != null)
        //            {
        //                exp.SetContext(context);
        //            }
        //        }
        //    }
        //}

        protected CodePosition ZeroCodePostion = new CodePosition(0, 0);


        protected Exp AnalySubExp(Exp exp)
        {
            if (exp == null)
            {
                AnalyCorrect = false;
                return null;
            }
            else
            {
                exp = exp.Analy();
                AnalyCorrect = AnalyCorrect && exp.AnalyCorrect;
                return exp;
            }
        }

        public ILGenerator IL
        {
            get
            {
                return this.ExpContext.ProcContext.GetILGenerator();
            }
        }

        protected void EmitConv()
        {
            if (RequireType != null && RetType != null)
                EmitHelper.EmitConv(IL, RequireType, RetType);
        }

        protected void EmitArgsExp(IEnumerable<Exp> args, IEnumerable<ZType> ztypes)
        {
            var args2 = new List<Exp>(args);
            var ztypes2 = new List<ZType>(ztypes);
            var size = args2.Count;

            for (int i = 0; i < size; i++)
            {
                Exp argExp = args2[i];
                argExp.RequireType = ztypes2[i];
                argExp.Emit();
            }
        }

        protected void EmitArgsExp(IEnumerable<Exp> args, ZLMethodDesc zdesc)
        {
            var ztypes = zdesc.ZLParams.Select(p => p.ZParamType).ToArray();
            EmitArgsExp(args, ztypes);
        }

        protected void EmitArgsExp(IEnumerable<Exp> args, IEnumerable<ParameterInfo> paras)
        {
            var ztypes = paras.Select(p => (ZTypeManager.GetBySharpType(p.ParameterType) as ZType)).ToArray();
            EmitArgsExp(args, ztypes);
        }

        protected void EmitArgsExp(IEnumerable<Exp> args, MethodInfo method)
        {
            var paramInfos = method.GetParameters();
            EmitArgsExp(args, paramInfos);
        }

        protected void EmitArgsExp(IEnumerable<Exp> args, ZLMethodInfo zmethod)
        {
            var method = zmethod.SharpMethod;
            EmitArgsExp(args, method);
        }
    }
}
