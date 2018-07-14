using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.ASTRaws;
using ZCompileCore.Contexts;
using ZCompileDesc.Descriptions;
using ZLangRT;

namespace ZCompileCore.AST
{
    public class MethodName
    {
        public ProcNameRaw NameRaw;
        public ProcMethod MethodAST;
        private List<object> NameParts;

        public MethodName(ProcMethod methodAST, ProcNameRaw nameRaw)
        {
            MethodAST = methodAST;
            NameRaw = nameRaw;
            NameParts = new List<object>();
        }

        public void Analy()
        {
            NameParts.Clear();
            for (int i = 0; i < NameRaw.NameTerms.Count; i++)
            {
                var term = NameRaw.NameTerms[i];
                if (term is ProcNameRaw.NameBracket)
                {
                    var pbrackets = (ProcNameRaw.NameBracket)term;
                    List<MethodParameter> mps = new List<MethodParameter>();
                    foreach (var item in pbrackets.Parameters)
                    {
                        MethodParameter mp = new MethodParameter(this, item);
                        mp.Analy();
                        mps.Add(mp);
                    }
                    NameParts.Add(mps);
                }
                else if (term is ProcNameRaw.NameText)
                {
                    ProcNameRaw.NameText t = (ProcNameRaw.NameText)term;
                    string text = t.TextToken.Text;
                    NameParts.Add(text);
                }
                else
                {
                    throw new CCException();
                }
            }
        }

        public string GetMethodName()
        {
            ZLClassInfo baseZType = this.MethodAST.MethodContext.ClassContext.GetSuperZType();
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

        public void EmitName()
        {
            for (int i = 0; i < NameParts.Count; i++)
            {
                var term = NameParts[i];
                if (term is List<MethodParameter>)
                {
                    var _ZBracketDefDesc = new ZCBracketDesc();
                    var args = (List<MethodParameter>)term;
                    foreach (MethodParameter item in args)
                    {
                        item.EmitName();
                    }
                }
            }
            //for (int i = 0; i < NameRaw.NameTerms.Count; i++)
            //{
            //    var term = NameRaw.NameTerms[i];
            //    if (term is ProcBracket)
            //    {
            //        var pbracket = term as ProcBracket;
            //        pbracket.EmitName();
            //    }
            //}
        }
        private ZCMethodDesc _ProcDesc;
        public ZCMethodDesc GetZDesc()
        {
            if (_ProcDesc == null)
            {
                _ProcDesc = ((ContextMethod)this.MethodAST.MethodContext).ZMethodInfo.ZMethodDesc;// new ZCMethodDesc();
                for (int i = 0; i < NameParts.Count; i++)
                {
                    var term = NameParts[i];
                    if (term is string)
                    {
                        //var textterm = term as LexToken;
                        string namePart = (string) term;
                        _ProcDesc.Add(namePart);
                    }
                    else if (term is List<MethodParameter>)
                    {
                        var _ZBracketDefDesc = new ZCBracketDesc();
                        var args = (List<MethodParameter>)term;
                        foreach (MethodParameter item in args)
                        {
                            var zarg = item.GetZParam();
                            _ZBracketDefDesc.Add(zarg);
                        }
                        //ProcBracket pbrackets = term as ProcBracket;
                        //var bracketDesc = pbrackets.GetZDesc();
                        _ProcDesc.Add(_ZBracketDefDesc);
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
        
    }
}
