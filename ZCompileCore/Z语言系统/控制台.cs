using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLangRT;
using ZLangRT.Attributes;

namespace Z语言系统
{
    [ZStatic]
    public static class 控制台
    {
        [ZCode("响铃")]
        public static void 响铃()
        {
            Console.Beep();
        }

        [ZCode("换行")]
        public static void 换行()
        {
            Console.WriteLine();
        }

        [ZCode("换(int:k)行")]
        public static void 换行(int k)
        {
            for (int i = 0; i < k; i++)
            {
                Console.WriteLine();
            }
        }

        [ZCode("读取文本")]
        public static string 读取文本()
        {
            return Console.ReadLine();
        }

        [ZCode("读取文本(string:提示语)")]
        public static string 读取文本(string 提示语)
        {
            Console.Write(提示语);
            return Console.ReadLine();
        }

        [ZCode("等待按键")]
        public static void 等待按键()
        {
            Console.ReadKey();
        }

        [ZCode("读取整数")]
        public static int 读取整数()
        {
            string str = Console.ReadLine();
            int value = int.Parse(str);
            return value;
        }

        [ZCode("读取整数(string:提示语)")]
        public static int 读取整数(string 提示语)
        {
            Console.Write(提示语);
            return 读取整数();
        }

        [ZCode("读取浮点数()")]
        public static float 读取浮点数()
        {
            string str = Console.ReadLine();
            float value = float.Parse(str);
            return value;
        }

        [ZCode("读取浮点数(string:提示语)")]
        public static float 读取浮点数(string 提示语)
        {
            Console.Write(提示语);
            return 读取浮点数();
        }

        [ZCode("打印(object:obj)")]
        public static void Write(object obj)
        {
            if (obj is bool)
            {
                bool b = (bool)obj;
                Console.Write(判断符.ToText(b));
            }
            else
            {
                Console.Write(obj);
            }
        }

        [ZCode("换行打印(事物:obj)")]
        public static void 换行打印(object obj)
        {
            Console.WriteLine();
            Write(obj);
        }
    }
}
