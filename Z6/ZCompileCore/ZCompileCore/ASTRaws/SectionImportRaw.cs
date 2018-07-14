using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.Lex;

namespace ZCompileCore.ASTRaws
{
    public class SectionImportRaw : SectionRaw
    {
        public LexTokenText KeyToken;
        public List<PackageRaw> Packages = new List<PackageRaw>();

        public CodePosition Position
        {
            get
            {
                if (KeyToken!=null)
                {
                    return KeyToken.Position;
                }
                else
                {
                    return new CodePosition(0, 0);
                }
            }
        }

        public class PackageRaw
        {
            public List<LexTokenText> Parts = new List<LexTokenText>();

            public CodePosition Position
            {
                get
                {
                    if(Parts.Count>0)
                    {
                        return Parts[0].Position;
                    }
                    else
                    {
                        return new CodePosition(0,0);
                    }
                }
            }
        }
    }
}
