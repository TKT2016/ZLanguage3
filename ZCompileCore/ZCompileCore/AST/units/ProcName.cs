using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Parsers;
using ZCompileDesc.Compilings;
using ZCompileDesc.Descriptions;
using ZCompileDesc.ZTypes;
using ZCompileKit;
using ZLangRT;
using ZNLP;

namespace ZCompileCore.AST
{
    public class ProcName : SectionPartProc
    {
        public List<object> NameTerms = new List<object>();
        ZMethodDesc _ProcDesc;
        List<ProcArg> ProcArgs = new List<ProcArg>();
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
                    ProcArgs.AddRange(pbrackets.Args);
                }
                else if (term is LexToken)
                {

                }
                else
                {
                    throw new CCException();
                }
            }
            bool isStatic = this.ClassContext.IsStatic();
            int start_i = isStatic ? 0 : 1;
            for (var i = 0; i < ProcArgs.Count; i++)
            {
                ProcArg procArg = ProcArgs[i];
                int argIndex = start_i + i;
                procArg.SetArgIndex(argIndex);
            }
        }

        public override void AnalyBody()
        {
            return;
        }

        public override void EmitName()
        {
            //bool isStatic = this.ClassContext.IsStatic();
            //var methodBuilder = this.ProcContext.EmitContext.CurrentMethodBuilder;
            //int start_i = isStatic ? 0 : 1;
            //int k = start_i;
            for (var i = 0; i < ProcArgs.Count; i++)
            {
                ProcArg procArg = ProcArgs[i];
                procArg.EmitName();
                //ZParam zparam = procArg.GetZParam();
                //if (!zparam.IsGeneric)
                //{
                //    ParameterBuilder pb = methodBuilder.DefineParameter(k, ParameterAttributes.None, zparam.ZParamName);
                //    //zparam.ParameterInfo = pb;
                //    //k++;
                //}
            }
        }

        public override void EmitBody()
        {
            return;
        }

        public string GetMethodName()
        {
            //ContextClass context = this.ProcContext.ClassContext;
            ZClassType baseZType = this.ClassContext.GetSuperZType();// context.GetSuperZType();
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

        private string CreateMethodName(ZMethodDesc zdesc)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < zdesc.Parts.Count; i++)
            {
                object item = zdesc.Parts[i];
                if (item is string)
                {
                    list.Add(item as string);
                }
                else if (item is ZBracketDefDesc)
                {
                    ZBracketDefDesc zbracket = item as ZBracketDefDesc;
                    for (var j = 0; j < zbracket.ParamsCount; j++)
                    {
                        var zparam = zbracket.GetParam(j);
                        if (zparam.IsGenericArg)
                        {
                            list.Add("类型");
                        }
                        else
                        {
                            list.Add(zparam.ZParamType.ZName);
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

        public ZMethodDesc GetZDesc()
        {
            if (_ProcDesc == null)
            {
                _ProcDesc = new ZMethodDesc();
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
                        ZBracketDefDesc bracketDesc = pbrackets.GetZDesc();
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

        public void SetContext(ContextProc procContext)
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

