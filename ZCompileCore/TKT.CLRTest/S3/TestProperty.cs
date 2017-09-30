using System;
using ZLangRT.Attributes;
using Z语言系统;

namespace TKT.CLRTest.S3
{
    [ZStatic]
    class TestProperty
    {
        private static int _计数器;

        private static string _名称;

        public static int 计数器
        {
            get
            {
                return TestProperty._计数器;
            }
            set
            {
                TestProperty._计数器 = value;
            }
        }

        public static string 名称
        {
            get
            {
                return TestProperty._名称;
            }
            set
            {
                TestProperty._名称 = value;
            }
        }

        private static void __InitPropertyMethod()
        {
            TestProperty._名称 = "TEST";
        }

        [STAThread]
        public static void 启动()
        {
            控制台.Write(TestProperty.计数器);
            控制台.Write(TestProperty.名称);
            控制台.等待按键();
        }
    }
}