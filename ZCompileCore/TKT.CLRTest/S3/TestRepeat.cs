using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKT.CLRTest.S3
{
    class TestRepeat
    {
        public void Test()
        {
            int count = 10;
            int index = 0;
            while(index<count)
            {
                Console.Write(" * ");
                index++;
            }
        }
    }
}
