using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCompileCore
{
    internal static class Debugr
    {
        public static bool IsInDebug =false;

        public static void WriteLine(object obj=null)
        {
            if(obj!=null)
                Console.WriteLine(obj.ToString());
        }
    }
}
