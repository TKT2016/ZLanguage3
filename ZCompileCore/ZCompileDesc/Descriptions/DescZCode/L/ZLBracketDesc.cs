using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCompileDesc.Descriptions
{
    public class ZLBracketDesc : ZABracketDesc,IBracket
    {
        public IParameter[] GetParameters() { return _ParamList.ToArray(); }
        //public override ZAParamInfo[] GetZParams() { return _ParamList.ToArray(); }
        public override int GetParametersCount() { return ParamsCount; }
        public override string ToZCode()
        {
            StringBuilder buff = new StringBuilder();
            buff.Append(" ( ");
            string paramtext = string.Join(" , ", _ParamList.Select(p => p.ToString()));
            buff.Append(paramtext);
            buff.Append(" ) ");
            return buff.ToString();
        }

        private List<ZLParamInfo> _ParamList = new List<ZLParamInfo>();
       

        public void Add(ZLParamInfo zlparam)
        {
            _ParamList.Add(zlparam);
        }

        public ZLParamInfo[] ZParams
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
