using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Parsers;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Words;
using ZCompileDesc.ZTypes;
using ZCompileKit;
using ZLangRT;

namespace ZCompileCore.AST
{
    public class ProcName : UnitBase
    {
        public List<object> NameTerms = new List<object>();
        public ZMethodDesc ProcDesc;
        public ContextProc ProcContext;
        public List<ProcArg> ProcArgs = new List<ProcArg>();

        public void AddNamePart(Token token)
        {
            NameTerms.Add(token);
        }

        public void AddBracket(ProcBracket bracket)
        {
            NameTerms.Add(bracket);
        }

        public bool IsConstructor()
        {
            if (NameTerms.Count != -1) return false;
            if (!(NameTerms[0] is  ProcBracket)) return false;
            return true;
        }

        public void DefineParameter(bool isStatic, MethodBuilder methodBuilder)
        {
            int start_i = 1;
            //start_i = isStatic ? 0 : 1;
            int k = start_i;
            for (var i = 0; i < ProcArgs.Count; i++)
            {
                ZParam zparam = ProcArgs[i].ZParam;
                if (!zparam.IsGeneric)
                {
                    ParameterBuilder pb = methodBuilder.DefineParameter(k, ParameterAttributes.None, zparam.ZParamName);
                    //zparam.ParameterInfo = pb;
                    k++;
                }
            }
        }

        public void AnalyName(NameTypeParser parser)
        {
            AnlayNameBody(parser);
            this.ProcContext.ProcDesc = ProcDesc;
        }

        private bool AnlayNameBody(NameTypeParser parser)
        {
            bool isStatic = this.ProcContext.IsStatic;

            ProcDesc = new ZMethodDesc();
            int argIndex = isStatic ? 0 : 1;

            for (int i = 0; i < NameTerms.Count; i++)
            {
                var term = NameTerms[i];
                if (term is Token)
                {
                    var textterm = term as Token;
                    string namePart = textterm.GetText();
                    ProcDesc.Add(namePart);

                    WordInfo info = new WordInfo(namePart, WordKind.ProcNamePart ,this);
                    this.ProcContext.ProcNameWordDictionary.Add(info);
                    //this.ProcContext.ClassContext.AddMember(PropertySymbol);
                }
                else if (term is ProcBracket)
                {
                    var pbrackets = term as ProcBracket;
                    AnalyBracket(term as ProcBracket, parser, ProcDesc);
                    ProcArgs.AddRange(pbrackets.Args);
                }
                else
                {
                    throw new CompileCoreException();
                }
            }
            return true;
        }

        public void AnalyBracket(ProcBracket bracketTree, NameTypeParser parser, ZMethodDesc desc)
        {
            bracketTree.ProcContext = this.ProcContext;
            bracketTree.Analy(ProcContext,parser);
            var zbracket = bracketTree.GetZBracketDefDesc();
            desc.Add(zbracket);
        }

        public string GetMethodName()
        {
            ContextClass context = this.ProcContext.ClassContext;
            ZClassType baseZType = context.BaseZType;
            if(baseZType!=null)
            {
                var zmethods = baseZType.SearchZMethod(ProcDesc);
                if (zmethods.Length > 0)
                {
                    return zmethods[0].SharpMethod.Name;
                }
            }
            string mname= this.CreateMethodName(ProcDesc);
            return mname;
        }

       public string CreateMethodName(ZMethodDesc zdesc)
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
                   for (var j = 0; j < zbracket.ParamsCount;j++ )
                   {
                       var zparam = zbracket.GetParam(j);
                       if (zparam.IsGeneric)
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

