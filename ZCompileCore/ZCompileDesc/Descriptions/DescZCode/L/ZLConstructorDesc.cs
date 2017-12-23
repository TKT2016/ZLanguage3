using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCompileDesc.Descriptions
{
    public class ZLConstructorDesc : ZAConstructorDesc
    {
        public ZLConstructorInfo ZConstructor { get; private set; }
        public ZLBracketDesc ZBracketDesc { get;private set; }

        public ZLConstructorDesc(ZLConstructorInfo zconstructor, ZLBracketDesc bracketDesc)
        {
            ZConstructor = zconstructor;
            ZBracketDesc = bracketDesc;
        }

        public override string ToZCode()
        {
            return ZBracketDesc.ToZCode();
            //List<string> list = new List<string>();
            //if (this.Constructor != null)
            //{
            //    list.Add(this.Constructor.DeclaringType.Name);
            //}
            //string argsText = string.Join(",", Args.ToString());
            //list.Add(argsText);
            //return string.Join("", list);
        }
    }
}
