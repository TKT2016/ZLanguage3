using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;
using ZCompileDesc.ZTypes;

namespace ZCompileDesc.Utils
{
    internal static class ProcDescHelper
    {
        public static ZConstructorDesc CreateZConstructorDesc(ConstructorInfo ci)
        {
            ZBracketDefDesc zbracket = new ZBracketDefDesc();
            //List<ZArgDefNormalDesc> args = new List<ZArgDefNormalDesc>();
            foreach (ParameterInfo param in ci.GetParameters())
            {
                ZType zparamType = ZTypeManager.GetBySharpType(param.ParameterType) as ZType;
                ZParam arg = new ZParam(param.Name, zparamType, param);
                //args.Add(arg);
                zbracket.Add(arg);
            }
            ZConstructorDesc desc = new ZConstructorDesc(zbracket);
            desc.Constructor=ci;
            return desc;
        }
    }
}
