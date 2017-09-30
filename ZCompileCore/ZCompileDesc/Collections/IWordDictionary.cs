using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileDesc.Words;

namespace ZCompileDesc.Collections
{
    public interface IWordDictionary
    {
        bool ContainsWord(string text);
        WordInfo SearchWord(string text);
    }
}
