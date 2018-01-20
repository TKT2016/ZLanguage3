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
using ZCompileKit.Tools;
using ZCompileCore.Parsers;
using ZCompileCore.ASTExps;

namespace ZCompileCore.AST
{
    public class ExpCall_Parsed : ExpCallAnalyedBase
    {
        ZMethodCall CallDesc;
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
            if (this.IsAnalyed) return this;
             AnalyProcDesc();
            Exp exp = SearchProc();
            exp = exp.Analy();
            IsAnalyed = true;
            return exp;
        }

        private Exp SearchProc( )
        {
            Exp temp = null;

            temp = SearchThis();
            if (temp != null) 
                return temp;

            temp = SearchBase();
            if (temp != null)
                return temp;

            temp = SearchUse();
            if (temp != null) 
                return temp;

            temp = SearchSubject();
            if (temp != null) 
                return temp;
            
            ErrorF(this.Position,"无法找到调用相应的过程");
            ExpCallNone expCallNone = new ExpCallNone(this.ExpContext, CallDesc , this);
            return expCallNone;     
        }
         
        private Exp SearchBase()
        {
            if (this.ExpContext.ClassContext.GetSuperZType() == null)
                return null;
            ZLMethodInfo[] descArray = this.ExpContext.ClassContext.GetSuperZType().SearchDeclaredZMethod(CallDesc);
            if (descArray.Length == 0)
            {
                return null;
            }
            else if (descArray.Length > 1)
            {
                ErrorF(this.Position, "找到多个过程，不能确定是属于哪一个简略使用的类型的过程");
                return null;
            }
            else
            {
                ZLMethodInfo methodDesc = descArray[0];
                ExpCallSuper expCallThis = new ExpCallSuper(this.ExpContext, CallDesc, methodDesc, this, ArgExps);
                return expCallThis;
            }
        }

        private Exp SearchSubject( )
        {
            Exp SubjectExp = Elements[0];
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
                while(SubjectExp is ExpBracket)
                {
                    ExpBracket expBracket = SubjectExp as ExpBracket;
                    if(expBracket.InneExps.Count==1)
                    {
                        SubjectExp = expBracket.InneExps[0];
                    }
                    else
                    {
                        throw new CCException();
                    }
                }
                ZMethodCall tailDesc = CallDesc.CreateTail();
                List<Exp> argExps = ListHelper.GetSubs<Exp>(ArgExps,1);
                ExpCallSubject expCallSubject = new ExpCallSubject(this.ExpContext, SubjectExp, tailDesc, this.SrcExp, argExps);
                return expCallSubject;
            }
        }

        private Exp SearchUse( )
        {
            //try
            //{
                ZLMethodInfo[] zmethods = this.ExpContext.ClassContext.FileContext.ImportUseContext.SearchUseMethod(CallDesc);
                if (zmethods.Length == 0)
                {
                    return null;
                }
                //else if (zmethods.Length > 1)
                //{
                //    ErrorF(this.Position, "找到多个过程，不能确定是属于哪一个简略使用的类型的过程");
                //    return null;
                //}
                else
                {
                    ExpCallUse expCallForeign = new ExpCallUse(this.ExpContext, CallDesc, zmethods[0], this.SrcExp, ArgExps);
                    return expCallForeign;
                }
            //}
            //catch(Exception ex)
            //{
            //    throw new CompileCoreException(ex.Message);
            //}
        }

        private Exp SearchThis( )
        {
            //ZMethodDesc[] descArray = this.ExpContext.ClassContext.SearchThisProc(CallDesc);
            ZCMethodInfo[] descArray = this.ExpContext.ClassContext.SearchThisProc(CallDesc);
            if (descArray.Length == 0)
            {
                return null;
            }
            else if (descArray.Length >1)
            {
                ErrorF(this.Position, "找到多个过程，不能确定是属于哪一个简略使用的类型的过程");
                return null;
            }
            else
            {
                ZCMethodInfo method = descArray[0];
                ExpCallThis expCallThis = new ExpCallThis(this.ExpContext, CallDesc, method, this, ArgExps);
                return expCallThis;
            }
        }

        private void AnalyProcDesc()
        {
           CallDesc= new ZMethodCall();
           ArgExps = new List<Exp>();
            foreach(var item in this.Elements)
            {
                //if (this.Elements[0].ToString().StartsWith("战场参数的绘图器"))
                //{
                //    Console.WriteLine(this.ToString());
                //}
                if(item is ExpProcNamePart)
                {
                    ExpProcNamePart namePartExp = item as ExpProcNamePart;
                    CallDesc.Add(namePartExp.PartName);
                }
                else if (item is ExpBracket)
                {
                    ExpBracket bracketExp = item as ExpBracket;
                    ZBracketCall zbracketDesc = bracketExp.GetCallDesc();
                    CallDesc.Add(zbracketDesc);
                    ArgExps.AddRange(bracketExp.GetSubExps());
                }
                else
                {
                    throw new CCException();
                }
            }
            //return zdesc;
        }

        public override void Emit()
        {
            throw new CCException();
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

        public override CodePosition Position
        {
            get
            {
                return Elements[0].Position; ;
            }
        }
        #endregion
    }
}
