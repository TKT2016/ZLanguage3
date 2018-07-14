using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.Lex;

namespace ZCompileCore.ASTRaws
{
    public class ProcNameRaw
    {
        public List<NamePart> NameTerms = new List<NamePart>();

        public bool IsConstructor( )
        {
            if (NameTerms.Count != 1) return false;
            if (!(NameTerms[0] is NameBracket)) return false;
            return true;
        }

        public NameBracket GetNameBracket()
        {
            if (!IsConstructor()) return null;
            return (NameTerms[0] as NameBracket);
        }

        public abstract class NamePart
        {

        }

        public class NameText : NamePart
        {
            public LexTokenText TextToken;
        }

        public class NameBracket : NamePart
        {
            public LexTokenSymbol LeftBracketToken;
            public List<ProcParameter> Parameters = new List<ProcParameter>();
            public LexTokenSymbol RightBracketToken;
        }

        public class ProcParameter
        {
            public LexTokenText ParameterToken;
        }
    }
}
