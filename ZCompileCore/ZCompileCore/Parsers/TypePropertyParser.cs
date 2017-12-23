using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Parser;
using ZCompileDesc;
using ZCompileDesc.Collections;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;

using ZNLP;

namespace ZCompileCore.Parsers
{
    public class TypePropertyParser
    {
        ContextImportTypes segManager;
        //ZTypeFinder finder;

        public TypePropertyParser(ContextImportTypes segManager)
        {
            this.segManager = segManager;
            //finder = new ZTypeFinder(segManager);
        }

        public ParseResult Parse(LexToken token)
        {
            ParseResult result = null;
            result = SearchOne(token);
            //if(result==null)
                //result = new ParseResult() {  ResultCount = 0 };
            return result;
        }

        private ParseResult SearchOne(LexToken token)
        {
            string ztypeName = token.GetText();
            ZType[] ztypes = this.segManager.SearchByClassNameOrDimItem(ztypeName);
            if (ztypes.Length > 0)
            {
                ParseResult result2 = new ParseResult() { ResultCount = ztypes.Length };
                result2.PTypes = ztypes;
                result2.Pname = ztypeName;
                return result2;
            }
            else
            {
                return null;
            }
        }

        public class ParseResult
        {
            public string Pname { get; set; }
            public ZType[] PTypes { get; set; }
            public int ResultCount { get; set; }
        }
    }
}
