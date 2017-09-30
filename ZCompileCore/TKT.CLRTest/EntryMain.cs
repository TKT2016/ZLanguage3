using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TKT.CLRTest.S3;

namespace TKT.CLRTest
{
    public class EntryMain
    {
        public static void Main()
        {
            //TestDefaultValue TD = new TestDefaultValue();
            //Console.WriteLine(TD.POINT);
            //Console.WriteLine(TD.GUID);

            TestProperty.启动();
            Console.ReadKey();
        }

        public static float ToF(int i)
        {
            return i;
        }

        public static float ToF2(int i)
        {
            float x = i;
            return x;
        }
    }
}
