using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCompileCore.Reports
{
    public static class CompileConsole
    {
        public static void Error(string msg)
        {
            var temp = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ForegroundColor = temp;
        }
    }
}
