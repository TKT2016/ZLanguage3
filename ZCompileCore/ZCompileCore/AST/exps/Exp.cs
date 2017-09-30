﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Tools;
using ZCompileDesc;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;
using ZCompileDesc.ZTypes;
using ZCompileDesc.ZMembers;
using ZCompileKit.Tools;

namespace ZCompileCore.AST
{
    public abstract class Exp:Tree
    {
        #region Context
        public ContextExp ExpContext { get;protected set; }
        public ContextProc ProcContext { get { return this.ExpContext.ProcContext; } }
        public ContextClass ClassContext { get { return this.ExpContext.ProcContext.ClassContext; } }
        public override ContextFile FileContext { get { return this.ExpContext.ProcContext.ClassContext.FileContext; } }
        public ContextProject ProjectContext { get { return this.ExpContext.ProcContext.ClassContext.FileContext.ProjectContext; } }
        #endregion

        public bool IsNested { get; protected set; }
        public virtual void SetIsNested(bool b)
        {
            this.IsNested = b;
            Exp[] subs = GetSubExps();
            if (subs != null && subs.Length > 0)
            {
                foreach (var exp in subs)
                {
                    if (exp != null)
                    {
                        exp.SetIsNested(b);
                    }
                }
            }
        }
        public abstract Exp[] GetSubExps();

        public virtual void SetContext(ContextExp context)
        {
            this.ExpContext = context;
            Exp[] subs =  GetSubExps();
            if (subs!=null && subs.Length > 0)
            {
                foreach (var exp in subs)
                {
                    if (exp != null)
                    {
                        exp.SetContext(context);
                    }
                }
            }
        }

        public ZType RetType { get; set; }
        public ZType RequireType { get;  set; }

        public virtual Exp Parse()
        {
            return this;
        }

        public virtual Exp Analy( )
        {
            return this;
        }

        public virtual void Emit()
        {
            throw new NotImplementedException();
        }

        protected CodePosition ZeroCodePostion = new CodePosition(0, 0);
        public virtual CodePosition Postion { get { return ZeroCodePostion; } }

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
                return this.ExpContext.ProcContext.EmitContext.ILout;
            }
        }

        protected void EmitConv( )
        {
            if (RequireType != null && RetType!=null)
            EmitHelper.EmitConv(IL, RequireType.SharpType , RetType.SharpType);
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

        protected void EmitArgsExp(IEnumerable<Exp> args, ZMethodDesc zdesc)
        {
            var ztypes = zdesc.DefArgs.Select(p => p.ZParamType).ToArray();
            EmitArgsExp( args,ztypes);
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

        protected void EmitArgsExp(IEnumerable<Exp> args, ZMethodInfo zmethod)
        {
            var method = zmethod.SharpMethod;
            EmitArgsExp(args, method);
        }
    }
}
