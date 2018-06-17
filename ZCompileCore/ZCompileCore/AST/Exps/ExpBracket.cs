using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.AST.Exps;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileDesc;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;


namespace ZCompileCore.AST.Exps
{
    public class ExpBracket:Exp
    {
        public LexToken LeftBracketToken { get; set; }
        public LexToken RightBracketToken { get;  set; }
        protected List<Exp> InneExps { get; set; }

        public ExpBracket(ContextExp expContext)
            : base(expContext)
        {
            InneExps = new List<Exp>();
        }

        public override void SetParent(Exp parentExp)
        {
            ParentExp = parentExp;
            foreach(var item in InneExps)
            {
                item.SetParent(this);
            }
        }

        public override Exp[] GetSubExps()
        {
            return InneExps.ToArray();
        }

        public virtual void AddInnerExp(Exp exp)
        {
            exp.ParentExp = this;
            InneExps.Add(exp);     
        }

        public bool IsExpBracketTagNew()
        {
            if (this.InneExps.Count == 1)
            {
                if (InneExps[0] is ExpTagNew)
                {
                    return true;
                }
            }
            return false;
        }

        public ExpBracketTagNew AnalyToTagNew()
        {
            //if (IsAnalyed) return null;
            if (IsExpBracketTagNew())
            {
                ExpBracketTagNew exp = new ExpBracketTagNew(this.ExpContext,this.LeftBracketToken,
                    this.RightBracketToken, (InneExps[0] as ExpTagNew));
                //exp.ParentExp = this.ParentExp;
                //exp.SetContext(this.ExpContext);
                ExpBracketTagNew expnew = (ExpBracketTagNew)(exp.Analy());
                return expnew;
            }
            return null;
        }

        public override Exp Analy( )
        {
            if (IsAnalyed) return this;
            ExpBracketTagNew newTagExp = AnalyToTagNew();
            if (newTagExp!=null)
            {
                IsAnalyed = true;
                this.AnalyCorrect = true;
                return newTagExp;
            }
            //if(this.InneExps.Count==1)
            //{
            //    if (InneExps[0] is ExpTagNew)
            //    {
            //        ExpBracketTagNew exp = new ExpBracketTagNew(this.LeftBracketToken, 
            //            this.RightBracketToken, (InneExps[0] as ExpTagNew));
            //        exp.SetContext(this.ExpContext);
            //        return exp.Analy();
            //    }
            //}
            this.AnalyCorrect = true;
            for (int i=0;i<InneExps.Count;i++)
            {
                Exp exp = InneExps[i];
                //exp.SetContext(this.ExpContext);
                exp = exp.Analy();
                if(exp==null)
                {
                    AnalyCorrect = false;
                }
                else
                {
                    InneExps[i] = exp;
                    AnalyCorrect = AnalyCorrect && exp.AnalyCorrect;
                }
            }
            AnalyRet();
            IsAnalyed = true;
            return this;
        }

        public void AnalyRet()
        {
            if (InneExps.Count == 1)
            {
                RetType = InneExps[0].RetType;
            }
            else
            {
                RetType = ZLangBasicTypes.ZVOID;
            }
        }

        public override void Emit()
        {
            if (this.InneExps.Count != 1) throw new CCException();
            Exp exp = this.InneExps[0];
            exp.Emit();
        }

        public virtual List<ZType> GetInnerTypes()
        {
            List<ZType> list = new List<ZType>();
            foreach (var expr in this.InneExps)
            {
                //if (!(expr is ExpArgNewDefault))
                {
                    list.Add(expr.RetType);
                }
            }
            return list;
        }

        public virtual ZBracketCall GetCallDesc()
        {
            ZBracketCall zbc = new ZBracketCall();
            foreach (var exp in this.InneExps)
            {
                if(exp is ExpNameValue)
                {
                    ExpNameValue nvexp = (exp as ExpNameValue);
                    if(!(nvexp.ValueExp is ExpTypeBase))
                    {
                        ZArgCall zargdesc = new ZArgCall() { IsGeneric = false, ZArgType = nvexp.ValueExp.RetType, ZArgName = nvexp.ArgName };
                        zbc.Add(zargdesc);
                    }
                }
                //else if(exp is ExpArgNewDefault)
                //{

                //}
                else// if (!(exp is ExpType))
                {
                    var type = exp.RetType;
                    ZArgCall zargdesc = new ZArgCall() { ZArgType = exp.RetType };// (exp.RetType);
                    //zargdesc.Data = exp;
                    //listArgs.Add(zargdesc);
                    zbc.Add(zargdesc);
                }
            }
            
            return zbc;
        }

        public virtual Exp UnBracket()
        {
            if (this.Count != 1) return this;
            Exp exp = this.InneExps[0];
            if(exp is ExpBracket)
            {
                return (exp as ExpBracket).UnBracket();
            }
            else
            {
                return exp;
            }
        }

        public virtual int Count
        {
            get
            {
                //if (InneExps.Count == 0 && (InneExps[0] is ExpArgNewDefault))
                //{
                //   return 0;
                //}
                return InneExps.Count;
            }
        }

        #region 覆盖
        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("(");
            if (InneExps != null && InneExps.Count > 0)
            {
                List<string> tempcodes = new List<string>();
                foreach (var expr in InneExps)
                {
                    if (expr != null)
                    {
                        tempcodes.Add(expr.ToString());
                    }
                    else
                    {
                        tempcodes.Add("  ");
                    }
                }
                buf.Append(string.Join(",", tempcodes));
            }
            buf.Append(")");
            return buf.ToString();
        }

        public override CodePosition Position
        {
            get
            {
                return LeftBracketToken!=null ?LeftBracketToken.Position:InneExps[0].Position;
            }
        }
        #endregion
    }
}
