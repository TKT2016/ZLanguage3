using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileDesc.Descriptions;

namespace ZCompileCore.AST.Exps
{
    public class LambdaBodyModel
    {
        public ZCFieldInfo OutClassField { get; set; }

        public List<ZCFieldInfo> FieldSymbols { get; set; }

        public ZCFieldInfo Get(string name)
        {
            ZCFieldInfo fi = FieldSymbols.Where(u => u.ZName == name).FirstOrDefault();
            return fi;
        }

        public LambdaBodyModel()
        {
            FieldSymbols = new List<ZCFieldInfo>();
        }
    }
}
