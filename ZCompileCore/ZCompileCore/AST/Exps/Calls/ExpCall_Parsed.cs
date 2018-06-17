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
using ZCompileCore;
using ZCompileCore.Parsers;
using ZCompileCore.AST.Exps;

namespace ZCompileCore.AST
{
    public class ExpCall_Parsed : ExpCallAnalyedBase
    {
        private ZMethodCall CallDesc ;
        private List<Exp> Elements;

        public ExpCall_Parsed(ContextExp expContext, List<Exp> elements,  Exp srcExp)
            : base(expContext)
        {
            SetSubs(elements, srcExp);
        }

        private void SetSubs(List<Exp> elements, Exp srcExp)
        {
            Elements = elements;
            this.SrcExp = srcExp;
            foreach (Exp sub in Elements)
            {
                sub.SetParent(this);
            }
        }

        public override void SetParent(Exp parentExp)
        {
            ParentExp = parentExp;
            foreach (var item in Elements)
            {
                item.SetParent(this);
            }
        }

        public override Exp[] GetSubExps()
        {
            return Elements.ToArray();
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
            
            Errorf(this.Position,"无法找到调用相应的过程");
            ExpCallNone expCallNone = new ExpCallNone(this.ExpContext, CallDesc , this);
            return expCallNone;     
        }
         
        private Exp SearchBase()
        {
            if (this.ExpContext.ClassContext.GetSuperZType() == null)
                return null;
            ZLClassInfo superZType = this.ExpContext.ClassContext.GetSuperZType();
            ZLMethodInfo[] descArray = superZType.SearchDeclaredZMethod(CallDesc);
            if (descArray.Length == 0)
            {
                return null;
            }
            else if (descArray.Length > 1)
            {
                Errorf(this.Position, "找到多个过程，不能确定是属于哪一个简略使用的类型的过程");
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
                    if(expBracket.Count==1)
                    {
                        SubjectExp = expBracket.GetSubExps()[0];
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

        private Exp SearchUse()
        {
            ZLMethodInfo[] zmethods = this.ExpContext.ClassContext.FileContext.ImportUseContext.SearchUseMethod(CallDesc);
            if (zmethods.Length == 0)
            {
                return null;
            }
            else if (zmethods.Length > 1)
            {
                Errorf(this.Position, "找到多个过程，不能确定是属于哪一个简略使用的类型的过程");
                return null;
            }
            else
            {
                ExpCallUse expCallForeign = new ExpCallUse(this.ExpContext, CallDesc, zmethods[0], this.SrcExp, ArgExps);
                return expCallForeign;
            }
        }

        private Exp SearchThis( )
        {
            ZCMethodInfo[] descArray = this.ExpContext.ClassContext.SearchThisProc(CallDesc);
            if (descArray.Length == 0)
            {
                return null;
            }
            else if (descArray.Length >1)
            {
                Errorf(this.Position, "找到多个过程，不能确定是属于哪一个简略使用的类型的过程");
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
