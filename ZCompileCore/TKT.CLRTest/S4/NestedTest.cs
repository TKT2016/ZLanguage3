using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKT.CLRTest.S4
{
    public class NestedTest
    {
        public string Name { get; set; }

        public class NestedClassTest
        {
            public void print()
            {
                //Console.WriteLine(Name);
            }
        }
    }
}
