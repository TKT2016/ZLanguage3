using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileDesc.Utils;

namespace ZCompileDesc.Descriptions
{
    public class ZCConstructorDesc : ZAConstructorDesc
    {
        public ZCConstructorInfo ZConstructor { get; set; }
        public ZCBracketDesc ZDesc { get; set; }

        public ZCConstructorDesc(ZCConstructorInfo zcc)
        {
            ZConstructor = zcc;
        }

        public override string ToZCode()
        {
            return ZDesc.ToZCode();
        }

        //public Type[] GetParamTypes()
        //{
        //    //throw new NotImplementedException();
        //    return ZConstructor.ZParams.Select(u=> ZTypeUtil.GetTypeOrBuilder(u.ZParamType)).ToArray();
        //}

        //public ZCParamInfo[] GetParamNormals()
        //{
        //    return ZConstructor.ZParams;
        //}
    }
}
