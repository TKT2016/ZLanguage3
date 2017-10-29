using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileDesc;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;
using ZCompileDesc.ZTypes;

namespace ZCompileCore.AST
{
    public class ExpBracket:Exp
    {
        public Token LeftBracketToken { get; set; }
        public Token RightBracketToken { get;  set; }
        public List<Exp> InneExps { get; protected set; }

        public override Exp[] GetSubExps()
        {
            return InneExps.ToArray();
        }

        public ExpBracket()
        {
            InneExps = new List<Exp>();
        }

        public ExpBracket(Exp exp)
        {
            InneExps = new List<Exp>();
            InneExps.Add(exp);
        }

        public ExpBracket(List<Exp> exps)
        {
            InneExps = exps;
        }

        public override void SetContext(ContextExp context)
        {
            this.ExpContext = context;
            foreach (var expr in this.InneExps)
            {
                expr.SetContext(context);
            }
        }

        public void AddInnerExp(Exp exp)
        {
            InneExps.Add(exp);
        }

        public override Exp Analy( )
        {
            this.AnalyCorrect = true;
            for (int i=0;i<InneExps.Count;i++)
            {
                Exp exp = InneExps[i];
                exp.SetContext(this.ExpContext);
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
            if (this.InneExps.Count != 1) throw new CompileCoreException();
            Exp exp = this.InneExps[0];
            exp.Emit();
        }

        public List<ZType> GetInnerTypes()
        {
            List<ZType> list = new List<ZType>();
            foreach (var expr in this.InneExps)
            {
                list.Add(expr.RetType);
            }
            return list;
        }

        public ZBracketCallDesc GetCallDesc()
        {
            ZBracketCallDesc zbc = new ZBracketCallDesc();
            foreach (var exp in this.InneExps)
            {
                if(exp is ExpNameValue)
                {
                    ExpNameValue nvexp = (exp as ExpNameValue);
                    if(!(nvexp.ValueExp is ExpType))
                    {
                        ZArg zargdesc = new ZArg() { IsGeneric = false, ZArgType = nvexp.ValueExp.RetType, ZArgName = nvexp.ArgName };
                        zbc.Add(zargdesc);
                    }
                }
                else// if (!(exp is ExpType))
                {
                    var type = exp.RetType;
                    ZArg zargdesc = new ZArg() { ZArgType = exp.RetType };// (exp.RetType);
                    //zargdesc.Data = exp;
                    //listArgs.Add(zargdesc);
                    zbc.Add(zargdesc);
                }
            }
            
            return zbc;
        }

        public Exp UnBracket()
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

        public int Count
        {
            get
            {
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
