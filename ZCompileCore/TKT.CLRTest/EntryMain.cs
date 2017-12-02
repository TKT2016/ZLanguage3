using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TKT.CLRTest.S3;
using TKT.CLRTest.S4;
using Z标准包.操作系统;
using Z语言系统;

namespace TKT.CLRTest
{
    public class EntryMain
    {
        public static void Main()
        {
            //FieldBuilder_Sample.Main2();
            DemoAssemblyBuilder.Main2();
            /*
            注册表项 注册表项 = new 注册表项("HKEY_CLASSES_ROOT\\*\\software\\");
            注册表操作器.Create(注册表项);
            注册表键 注册表键 = new 注册表键("HKEY_CLASSES_ROOT\\*\\software\\", "B");
            注册表操作器.Create(注册表键);
            注册表键.Write("test");
            注册表键 注册表键2 = new 注册表键("HKEY_CLASSES_ROOT\\*\\software\\", "C");
            注册表操作器.Create(注册表键2);
            注册表键2.Write("test C2");
            string text = 注册表键.Read();
            控制台.Write(text);
            控制台.换行();
            注册表键.Write("BBBBB");
            text = 注册表键.Read();
            控制台.Write(text);
            控制台.换行();
            控制台.等待按键();

            注册表操作Test.CreateSubKey("software\\A11B11C11");
            注册表操作Test.WriteSubKeyValue("software\\A11B11C11", "A2", "A1111");
            注册表操作Test.DeleteSubKey("software\\A11B11C11");*/
            //TestDefaultValue TD = new TestDefaultValue();
            //Console.WriteLine(TD.POINT);
            //Console.WriteLine(TD.GUID);

            //TestProperty.启动();
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
