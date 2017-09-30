using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Lex;
using ZCompileCore.Parser;
using ZCompileDesc;
using ZCompileDesc.Collections;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;
using ZCompileDesc.Words;
using ZCompileDesc.ZTypes;

namespace ZCompileCore.Parsers
{
    public class NameTypeParser
    {
        IWordDictionary collection;

        public NameTypeParser(IWordDictionary collection)
        {
            this.collection = collection;
        }

        public ParseResult ParseType(Token token)
        {
            ParseResult result = null;
            result = ParseNameByAll(token, collection);
            return result;
        }

        public ParseResult ParseVar(Token token)
        {
            ParseResult result = null;
            result =  ParseType(token);
            if (result == null)
            {
                result = ParseNameBySegmenter(token, collection);
            }
            return result;
        }

        private ParseResult ParseNameByAll(Token token, IWordDictionary collection)
        {
            string varName = token.GetText();
            ParseResult result = null;
            WordInfo word = collection.SearchWord(varName);
            if (word!=null)
            {
                var zType = word.WDataList[0].Data as ZType;
                result = new ParseResult() { VarName = varName, ZType = zType };
                return result;
            }
            return null;
        }

        private ParseResult ParseNameBySegmenter(Token token, IWordDictionary collection)
        {
            WordSegmenter segmenter = new WordSegmenter(collection);
            Token[] newTokens = segmenter.Split(token);
            if (newTokens.Length == 2)
            {
                string argTypeName = newTokens[0].GetText();
                var ArgType = ZTypeManager.GetByMarkName(argTypeName)[0] as ZType;
                var result = new ParseResult() { TypeName = argTypeName, ZType = ArgType, VarName = newTokens[1].GetText() };
                return result;
            }
            else
            {
                return null;
            }
        }

        public class ParseResult
        {
            public string TypeName;
            public ZType ZType;
            public string VarName;
        }
    }
}
