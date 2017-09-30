using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileDesc.Collections;
using ZCompileDesc.Words;

namespace ZCompileCore.Contexts
{
    public class ExpParseDictionary : IWordDictionary
    {
        ContextExp ExpContext;
        public ExpParseDictionary(ContextExp expContext)
        {
            ExpContext = expContext;
        }

        #region IWordDictionary实现
        public bool ContainsWord(string text)
        {
            return ExpContext.FileContext.ContainsWord(text)
                || ExpContext.ProcContext.ContainsVar(text)
            ;
        }

        public WordInfo SearchWord(string text)
        {
            WordInfo word1 = ExpContext.FileContext.SearchWord(text);
            WordInfo word2 = ExpContext.ProcContext.SearchVar(text);
            WordInfo newWord = WordInfo.Merge(word1, word2);
            return newWord;
        }
        #endregion
    }
}
