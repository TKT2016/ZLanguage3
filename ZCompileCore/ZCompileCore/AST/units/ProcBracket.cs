using System.Collections.Generic;
using System.Linq;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Parsers;
using ZCompileCore.Symbols;
using ZCompileDesc.Descriptions;
using ZCompileDesc.ZTypes;

namespace ZCompileCore.AST
{
    public class ProcBracket : SectionPartProc
    {
        public List<ProcArg> Args { get; private set; }

        public ProcBracket()
        {
            Args = new List<ProcArg>();
        }

        public override void AnalyText()
        {
            foreach (var item in Args)
            {
                item.AnalyText();
            }
        }

        public override void AnalyType()
        {
            foreach (var item in Args)
            {
                item.AnalyType();
            }
        }

        public override void AnalyBody()
        {
            foreach (var item in Args)
            {
                item.AnalyBody();
            }
        }

        public override void EmitName()
        {
            foreach (var item in Args)
            {
                item.EmitName();
            }
        }

        public override void EmitBody()
        {
            foreach (var item in Args)
            {
                item.EmitBody();
            }
        }

        public void SetContext(ContextProc procContext)
        {
            this.ProcContext = procContext;
            this.ClassContext = this.ProcContext.ClassContext;
            this.FileContext = this.ClassContext.FileContext;
            foreach (var item in Args)
            {
                item.SetContext(procContext);
            }
        }

        //public ContextProc ProcContext;

        public void Add(ProcArg arg)
        {
            Args.Add(arg);
        }

        //public void Analy(ContextProc ProcContext, TypeArgParser parser)
        //{
        //    this.ProcContext = ProcContext;
        //    foreach(var arg in Args)
        //    {
        //        arg.Analy(ProcContext,parser);
        //    }
        //}

        private ZBracketDefDesc _ZBracketDefDesc;
        public ZBracketDefDesc GetZDesc()
        {
            if (_ZBracketDefDesc==null)
            {
                _ZBracketDefDesc = new ZBracketDefDesc();
                foreach(ProcArg item in Args)
                {
                    var zarg = item.GetZParam();
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
