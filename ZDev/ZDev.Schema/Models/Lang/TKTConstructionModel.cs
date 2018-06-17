using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZDev.Schema.Models.Lang
{
    public class TKTConstructionModel : TKTModelBase
    {
        public List<TKTArgModel> Args { get; set; }

        public TKTConstructionModel()
        {
            Args = new List<TKTArgModel>();
        }

        public override string ToString()
        {
            List<string> buff = new List<string>();
            buff.Add("(");
            for (int i = 0; i < Args.Count;i++ )
            {
                TKTArgModel arg = Args[i];
                buff.Add(arg.ToString());
                if(i<Args.Count-1)
                {
                    buff.Add(",");
                }
            }
            buff.Add(")");
            string code = string.Join("", buff);
            return code;
        }
    }
}
