using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKT.CLRTest.S4
{
    class FieldTest2A
    {
        public string name = "xxx";
    }

    class FieldTest2
    {
        FieldTest2A F1=new FieldTest2A ();

        public object Get()
        {
            return this.F1.name;
        }
    }
}
