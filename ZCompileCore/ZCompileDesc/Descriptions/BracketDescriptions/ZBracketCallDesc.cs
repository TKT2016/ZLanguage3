using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileDesc.Collections;

namespace ZCompileDesc.Descriptions
{
    public class ZBracketCallDesc //: ZBracketDescBase,IDefDesc
    {
        public List<ZArg> Args { get; protected set; }

        public ZBracketCallDesc()
        {
            //DefArgs = new NamingList<ZArgDefDescBase>();
            Args = new List<ZArg>();
        }

        public void Add(ZArg zarg)
        {
            Args.Add(zarg);
        }

        public int ArgsCount
        {
            get { return Args.Count; }
        }

        public int HasNameCount
        {
            get
            {
                return Args.Where(P => P.HasName).Count();
            }
        }

        public bool AllHasName()
        {
            return HasNameCount == this.Args.Count;
        }

        public ZArg GetArg(string name)
        {
            return Args.Where(P => P.HasName && P.ZArgName==name).FirstOrDefault();
        }

        public ZArg GetArg(int i)
        {
            return Args[i];
        }

        public string ToZCode()
        {
            List<string> zcodes = new List<string>();
            int size = this.ArgsCount;
            for (int i = 0; i < size; i++)
            {
                var zparam = this.Args[i];
                string zcode = zparam.ToZCode();
                zcodes.Add(zcode);
            }
            return "(" + string.Join(",", zcodes) + ")";
        }

        public override string ToString()
        {
            return ToZCode();
        }

    }
}
