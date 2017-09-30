using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Tools;
using ZLangRT;
using ZCompileDesc.Descriptions;
using ZCompileKit;
using ZCompileDesc.ZMembers;
using ZCompileKit.Tools;
using ZCompileCore.Parsers;

namespace ZCompileCore.AST
{
    public class ExpCall_Parsed : ExpCallAnalyedBase
    {
        ZCallDesc CallDesc;
        List<Exp> Elements;

        public ExpCall_Parsed( List<Exp> elements, ContextExp context,Exp srcExp)
        {
            Elements = elements;
            setContext(context);
            this.SrcExp = srcExp;
        }

        public override Exp[] GetSubExps()
        {
            return ArgExps.ToArray();
        }

        private void setContext(ContextExp context)
        {
            this.ExpContext = context;
            foreach (var expr in this.Elements)
            {
                expr.SetContext(context);
            }
        }

        public override Exp Analy( )
        {
             AnalyProcDesc();
            Exp exp = SearchProc();
            exp = exp.Analy();
            return exp;
        }

        private Exp SearchProc( )
        {
            Exp temp = null;

            temp = SearchThis();
            if (temp != null) 
                return temp;

            temp = SearchUse();
            if (temp != null) 
                return temp;

            temp = SearchSubject();
            if (temp != null) 
                return temp;
            
            ErrorE(this.Postion,"无法找到调用相应的过程");
            ExpCallNone expCallNone = new ExpCallNone(this.ExpContext, CallDesc , this);
            return expCallNone;     
        }

        private Exp SearchSubject( )
        {
            var SubjectExp = Elements[0];
            if (SubjectExp is ExpProcNamePart)
            {
                return null;
            }
            else if (SubjectExp is ExpNameValue)
            {
                return null;
            }
            else
            {
                ZCallDesc tailDesc = CallDesc.CreateTail();
                List<Exp> argExps = ListHelper.GetSubs<Exp>(ArgExps,1);
                ExpCallSubject expCallSubject = new ExpCallSubject(this.ExpContext, SubjectExp, tailDesc, this.SrcExp, argExps);
                return expCallSubject;
            }
        }

        private Exp SearchUse( )
        {
            try
            {
                ZMethodInfo[] zmethods = this.ExpContext.ClassContext.FileContext.SearchUseProc(CallDesc);
                if (zmethods.Length == 0)
                {
                    return null;
                }
                else if (zmethods.Length > 1)
                {
                    ErrorE(this.Postion, "找到多个过程，不能确定是属于哪一个简略使用的类型的过程");
                    return null;
                }
                else
                {
                    ExpCallUse expCallForeign = new ExpCallUse(this.ExpContext, CallDesc, zmethods[0], this.SrcExp, ArgExps);
                    return expCallForeign;
                }
            }
            catch(Exception ex)
            {
                throw new CompileCoreException(ex.Message);
            }
        }

        private Exp SearchThis( )
        {
            ZMethodDesc[] descArray = this.ExpContext.ClassContext.SearchThisProc(CallDesc);
            if (descArray.Length == 0)
            {
                return null;
            }
            else if (descArray.Length >1)
            {
                ErrorE(this.Postion, "找到多个过程，不能确定是属于哪一个简略使用的类型的过程");
                return null;
            }
            else
            {
                ZMethodDesc methodDesc = descArray[0];
                ExpCallThis expCallThis = new ExpCallThis(this.ExpContext, CallDesc, methodDesc, this, ArgExps);
                return expCallThis;
            }
        }

        private void AnalyProcDesc()
        {
           CallDesc= new ZCallDesc();
           ArgExps = new List<Exp>();
            foreach(var item in this.Elements)
            {
                if(item is ExpProcNamePart)
                {
                    ExpProcNamePart namePartExp = item as ExpProcNamePart;
                    CallDesc.Add(namePartExp.PartName);
                }
                else if (item is ExpBracket)
                {
                    ExpBracket bracketExp = item as ExpBracket;
                    ZBracketCallDesc zbracketDesc = bracketExp.GetCallDesc();
                    CallDesc.Add(zbracketDesc);
                    ArgExps.AddRange(bracketExp.GetSubExps());
                }
                else
                {
                    throw new CompileCoreException();
                }
            }
            //return zdesc;
        }

        public override void Emit()
        {
            throw new CompileCoreException();
        }

        #region 辅助
        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            List<string> tempcodes = new List<string>();
            foreach (var expr in Elements)
            {
                if (expr != null)
                {
                    tempcodes.Add(expr.ToString());
                }
                else
                {
                    tempcodes.Add(" ");
                }
            }
            buf.Append(string.Join("", tempcodes));
            return buf.ToString();
        }

        public override CodePosition Postion
        {
            get
            {
                return Elements[0].Postion; ;
            }
        }
        #endregion
    }
}
