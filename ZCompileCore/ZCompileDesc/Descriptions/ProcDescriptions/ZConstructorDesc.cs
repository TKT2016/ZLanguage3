using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ZCompileDesc.Descriptions
{
    public class ZConstructorDesc:IDefDesc
    {
        public ConstructorInfo Constructor { get; set; }
        public ZBracketDefDesc ParamsBracket { get; set; }

        public ZConstructorDesc( )
        {
            ParamsBracket = new ZBracketDefDesc();
        }

        public ZConstructorDesc(ZBracketDefDesc zbracket )
        {
            ParamsBracket = zbracket;
        }

        public bool ZEquals(ZConstructorDesc zconstructor)
        {
            return this.ParamsBracket.ZEquals(zconstructor.ParamsBracket);
        }

        public bool ZEquals(ZNewDesc znew)
        {
            return this.ParamsBracket.ZEquals(znew.ArgsBracket);
        }

        public string ToZCode()
        {
            return ParamsBracket.ToZCode();
            //List<string> list = new List<string>();
            //if (this.Constructor != null)
            //{
            //    list.Add(this.Constructor.DeclaringType.Name);
            //}
            //string argsText = string.Join(",", Args.ToString());
            //list.Add(argsText);
            //return string.Join("", list);
        }

        public override string ToString()
        {
            return this.ToZCode();
        }
    }
}
