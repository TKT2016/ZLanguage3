using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKT.CLRTest.S3
{
    class TestSC
    {
        public static void Start()
        {
            var C = 获取事物();
            var D =(string)C;
            Console.WriteLine(D);
            Console.ReadKey();
        }

        public static object 获取事物()
        {
            return "APP";
        }
    }
}
