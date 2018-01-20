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

using ZCompileNLP;

namespace ZCompileCore.Parsers
{
    public class TypeArgParser
    {
        ContextClass ClassContext;
        ContextImportUse ImportUseContext;

        //ContextImportTypes segManager;
        //public ZTypeFinder Finder { get;private set; }
        public TypeArgParser(ContextClass classContext)//(ContextImportTypes segManager)
        {
            //this.segManager = segManager;
           // Finder = new ZTypeFinder(segManager);
            this. ClassContext = classContext;
            ImportUseContext = classContext.FileContext.ImportUseContext;
        }

        public ParseResult Parse(LexToken token)
        {
            ParseResult result = null;
            result = ParseZType(token);
            if(result==null)
                result = new ParseResult() { ResultCount = 0 };
            return result;
        }

        private ParseResult ParseZType(LexToken token)
        {
            string text = token.GetText();
            string[] words = ImportUseContext.GetArgSegementer().Cut(text);// segManager.ArgSegementer.Cut(text).ToArray();
            if(words.Length==2)
            {
                return SearchTwo(words[0], words[1]);
            }
            else if (words.Length == 1)
            {
                return SearchOne(words[0]);
            }
            throw new CCException();
        }

        private ParseResult SearchOne(string ztypeName )
        {
            //ZTypeFinder.FindResult result = Finder.Find(ztypeName);
            ZType[] ztypes = ImportUseContext.SearchImportType(ztypeName);// this.segManager.SearchZTypesByClassNameOrDimItem(ztypeName);
            if (ztypes.Length > 0)
            {
                ParseResult result2 = new ParseResult() { ResultCount = 1, ArgZTypes = ztypes , ArgName = ztypeName };
                return result2;
            }
            return null;
        }

        private ParseResult SearchTwo(string ztypeName,string argname)
        {
            ZType[] ztypes = ImportUseContext.SearchZTypesByClassNameOrDimItem(ztypeName);// this.segManager.SearchZTypesByClassNameOrDimItem(ztypeName);
            if (ztypes.Length > 0)
            {
                ParseResult result2 = new ParseResult() { ResultCount = 1, ArgZTypes = ztypes, ArgName = argname };
                return result2;
            }
            return null;
           
        }

        public class ParseResult
        {
            public ZType[] ArgZTypes { get; set; }
            public string ArgName { get; set; }
            public int ResultCount { get; set; }
        }

    }
}
