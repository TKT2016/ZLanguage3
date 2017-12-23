using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.Contexts;
using ZCompileDesc;
using ZCompileDesc.Descriptions;


namespace ZCompileCore.Parsers
{
    public class ZTypeParser
    {
        ContextImportTypes segManager;

        public ZTypeParser(ContextImportTypes segManager)
        {
            this.segManager = segManager;
        }

        public ZType[] Find(string text)
        {
            ZType[] result = null;
            result = SearchOne(text);
            return result;
        }

        private ZType[] SearchOne(string ztypeName)
        {
            ZType[] ztypes = this.segManager.SearchByClassNameOrDimItem(ztypeName);
            if (ztypes.Length > 0)
            {
                return ztypes;
                //ParseResult result2 = new ParseResult() { ResultCount = 1, ArgZTypes = ztypes, ArgName = ztypeName };
                //return result2;
            }
            return null;
        }

    }
}
