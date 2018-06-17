using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCompileDesc.Descriptions
{
    public class ZCBracketDesc : ZABracketDesc,IBracket
    {
        public IParameter[] GetParameters() { return _ParamList.ToArray(); }
        //public override ZAParamInfo[] GetZParams() { return _ParamList.ToArray(); }
        public override int GetParametersCount() { return _ParamList.Count; }
        public override string ToZCode() {
            StringBuilder buff = new StringBuilder();
            buff.Append("(");
            string paramtext = string.Join(",", _ParamList.Select(p => p.ToZode()));
            buff.Append(paramtext);
            buff.Append(")");
            return buff.ToString();
        }

        private List<ZCParamInfo> _ParamList = new List<ZCParamInfo>();

        public void Add(ZCParamInfo zlparam)
        {
            _ParamList.Add(zlparam);
        }

        public ZCParamInfo[] ZParams
        {
            get
            {
                return _ParamList.ToArray();
            }
        }

        public int ParamsCount
        {
            get
            {
                return _ParamList.Count;
            }
        }
    }
}
