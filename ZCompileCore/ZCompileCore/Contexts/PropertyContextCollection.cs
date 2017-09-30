using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Symbols;
using ZCompileDesc.Words;

namespace ZCompileCore.Contexts
{
    public class PropertyContextCollection
    {
        public ContextClass ClassContext { get; set; }

        public WordDictionary Dict { get; private set; }

        public PropertyContextCollection()
        {
            Dict = new WordDictionary("代码属性表");
        }

        #region IWordDictionary实现
        public bool ContainsWord(string text)
        {
            return Dict.ContainsWord(text)
            ;
        }

        public WordInfo SearchWord(string text)
        {
            return Dict.SearchWord(text);
        }
        #endregion

    }
}
