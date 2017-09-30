using System.Collections.Generic;
using System.Linq;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Parsers;
using ZCompileCore.Symbols;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Words;
using ZCompileDesc.ZTypes;

namespace ZCompileCore.AST
{
    public class ProcBracket : UnitBase
    {
        public List<ProcArg> Args { get; private set; }

        public ProcBracket()
        {
            Args = new List<ProcArg>();
        }

        public ContextProc ProcContext;

        public void Add(ProcArg arg)
        {
            Args.Add(arg);
        }

        public void Analy(ContextProc ProcContext, NameTypeParser parser)
        {
            this.ProcContext = ProcContext;
            foreach(var arg in Args)
            {
                arg.Analy(ProcContext,parser);
            }
        }

        private ZBracketDefDesc _ZBracketDefDesc;
        public ZBracketDefDesc GetZBracketDefDesc()
        {
            if (_ZBracketDefDesc==null)
            {
                _ZBracketDefDesc = new ZBracketDefDesc();
                foreach(var item in Args)
                {
                    var zarg = item.ZParam;
                    _ZBracketDefDesc.Add(zarg);
                }
            }
            return _ZBracketDefDesc;
        }

        public override string ToString()
        {
            return string.Format("({0})", string.Join(" , ", Args.Select(p=>p.ToString())));
        }
    }
}
