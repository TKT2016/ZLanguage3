using System;
using ZLangRT;
using ZLangRT.Attributes;
using ZLangRT.Utils;
using Z语言系统;

namespace TKT.CLRTest.S3
{
    class TestStrongTrans
    {
        public float ToFloat(object obj)
        {
            var a = (float)obj;
            return a;
        }

        public int ToInt(object obj)
        {
            var a = (int)obj;
            return a;
        }

        public S3A1 ToSub(S3A obj)
        {
            var a = (S3A1)obj;
            return a;
        }

        public string ToStr(object obj)
        {
            var a = (string)obj;
            return a;
        }

        private void Test()
        {
            var a = Calculater.Cast<int>((object)100);
            var b = Calculater.Cast<string>((object)"aaa");
            var c = Calculater.Cast<float>((object)2.0);
            Console.WriteLine(""+a+b+c);
        }
    }

    class S3A
    {

    }

    class S3A1:S3A
    {

    }
}