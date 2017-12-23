using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Parsers;
using ZCompileDesc.Descriptions;
using ZCompileKit;
using ZLangRT;
using ZNLP;

namespace ZCompileCore.AST
{
    public class ProcName : SectionPartProc
    {
        public List<object> NameTerms = new List<object>();
        ZCMethodDesc _ProcDesc;

        public ContextMethod MethodContext { get { return (ContextMethod)ProcContext; } }

        public void AddNamePart(LexToken token)
        {
            NameTerms.Add(token);
        }

        public void AddBracket(ProcBracket bracket)
        {
            NameTerms.Add(bracket);
        }

        public override void AnalyText()
        {
            for (int i = 0; i < NameTerms.Count; i++)
            {
                var term = NameTerms[i];
                if (term is ProcBracket)
                {
                    var pbrackets = term as ProcBracket;
                    pbrackets.AnalyText();
                }
                else if (term is LexToken)
                {
                    
                }
                else
                {
                    throw new CCException();
                }
            }
        }

        public override void AnalyType()
        {
            for (int i = 0; i < NameTerms.Count; i++)
            {
                var term = NameTerms[i];
                if (term is ProcBracket)
                {
                    var pbrackets = term as ProcBracket;
                    pbrackets.AnalyType();
                    //ProcArgs.AddRange(pbrackets.Args);
                }
                else if (term is LexToken)
                {

                }
                else
                {
                    throw new CCException();
                }
            }
            //bool isStatic = this.ClassContext.IsStatic();
            //int start_i = 0;// isStatic ? 0 : 1;
            //if(isStatic)
            //{
            //    Console.WriteLine(" isStatic ");
            //}
            var size = this.ProcContext.GetParametersCount();//
            for (var i = 0; i < size; i++)
            {
                var procArg = this.ProcContext.GetParameter(i);// ProcArgs[i];
                int argIndex = i; // start_i + i;
                procArg.ParamIndex = argIndex;//.SetParameterIndex(argIndex);
               
            }
        }

        public override void AnalyBody()
        {
            return;
        }

        public override void EmitName()
        {
            for (int i = 0; i < NameTerms.Count; i++)
            {
                var term = NameTerms[i];
                if (term is ProcBracket)
                {
                    var pbracket = term as ProcBracket;
                    pbracket.EmitName();
                }
            }
            //var size = this.ProcContext.GetParametersCount();// ProcArgs.Count;
            //for (var i = 0; i < size; i++)
            //{
            //    var procArg = this.ProcContext.GetParameter(i);
            //    this.ProcContext.DefineParameter(procArg);
            //    //ProcParameter procArg = this.ProcContext.GetParameter(i);// ProcArgs[i];//MethodContext.ZMethodInfo.ZParams[i];
            //    //procArg.EmitName();
            //}
        }

        public override void EmitBody()
        {
            return;
        }

        public string GetMethodName()
        {
            ZLClassInfo baseZType = this.ClassContext.GetSuperZType();
            var procDesc = GetZDesc();
            if (baseZType != null)
            {
                var zmethods = baseZType.SearchZMethod(procDesc);
                if (zmethods.Length > 0)
                {
                    return zmethods[0].SharpMethod.Name;
                }
            }
            string mname = this.CreateMethodName(procDesc);
            return mname;
        }

        private string CreateMethodName(ZCMethodDesc zdesc)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < zdesc.Parts.Length; i++)
            {
                object item = zdesc.Parts[i];
                if (item is string)
                {
                    list.Add(item as string);
                }
                else if (item is ZCBracketDesc)
                {
                    ZCBracketDesc zbracket = item as ZCBracketDesc;
                    for (var j = 0; j < zbracket.ParamsCount; j++)
                    {
                        var zparam = zbracket.ZParams[j];
                        if (zparam.GetIsGenericParam())
                        {
                            list.Add("类型");
                        }
                        else
                        {
                            list.Add(zparam.ZParamType.ZTypeName);
                        }
                    }
                }
                else
                {
                    throw new ZyyRTException();
                }
            }
            return string.Join("", list);
        }

        public ZCMethodDesc GetZDesc()
        {
            if (_ProcDesc == null)
            {
                _ProcDesc = ((ContextMethod)this.ProcContext).ZMethodInfo.ZMethodDesc;// new ZCMethodDesc();
                for (int i = 0; i < NameTerms.Count; i++)
                {
                    var term = NameTerms[i];
                    if (term is LexToken)
                    {
                        var textterm = term as LexToken;
                        string namePart = textterm.GetText();
                        _ProcDesc.Add(namePart);
                    }
                    else if (term is ProcBracket)
                    {
                        ProcBracket pbrackets = term as ProcBracket;
                        var bracketDesc = pbrackets.GetZDesc();
                        _ProcDesc.Add(bracketDesc);
                    }
                    else
                    {
                        throw new CCException();
                    }
                }
                return _ProcDesc;
            }
            return _ProcDesc;
        }

        public void SetContext(ContextMethod procContext)
        {
            this.ProcContext = procContext;
            this.ClassContext = this.ProcContext.ClassContext;
            this.FileContext = this.ClassContext.FileContext;
            foreach (var item in NameTerms)
            {
                if (item is ProcBracket)
                {
                    (item as ProcBracket).SetContext(procContext);
                }
            }
        }

        public override string ToString()
        {
            List<string> buflist = new List<string>();
            foreach (var term in NameTerms)
            {
                buflist.Add(term.ToString());
            }
            string fnname = string.Join("", buflist);
            return fnname;
        }
    }
}

