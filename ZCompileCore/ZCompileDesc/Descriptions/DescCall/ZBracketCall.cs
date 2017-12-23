using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCompileDesc.Descriptions
{
    public class ZBracketCall : ACall,IBracket
    {
        List<ZArgCall> _Args = new List<ZArgCall>();

        public ZBracketCall()
        {
            //DefArgs = new NamingList<ZArgDefDescBase>();
            //Args = new List<ZArgCall>();
        }
        public IParameter[] GetParameters() { return Args; }
        public int GetParametersCount() { return ArgsCount; }

        public void Add(ZArgCall zarg)
        {
            _Args.Add(zarg);
        }

        public int ArgsCount
        {
            get { return _Args.Count; }
        }

        public ZArgCall[] Args
        {
            get { return _Args.ToArray(); }
        }

        public int HasNameCount
        {
            get
            {
                return _Args.Where(P => P.HasCallParameterName()).Count();
            }
        }

        public bool AllHasName()
        {
            return HasNameCount == this._Args.Count;
        }

        public ZArgCall GetArg(string name)
        {
            return _Args.Where(P => P.HasCallParameterName() && P.ZArgName == name).FirstOrDefault();
        }

        public ZArgCall GetArg(int i)
        {
            return _Args[i];
        }

        public override string ToZCode()
        {
            List<string> zcodes = new List<string>();
            int size = this.ArgsCount;
            for (int i = 0; i < size; i++)
            {
                var zparam = this._Args[i];
                string zcode = zparam.ToZCode();
                zcodes.Add(zcode);
            }
            return "(" + string.Join(",", zcodes) + ")";
        }

     
    }
}
