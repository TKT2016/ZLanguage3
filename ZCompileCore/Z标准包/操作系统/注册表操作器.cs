using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using ZLangRT.Attributes;

namespace Z标准包.操作系统
{
    [ZStatic]
    public static class 注册表操作器
    {
        [ZCode("创建(注册表项:X)")]
        public static void Create(注册表项 X)
        {
            X.Create();
        }

        [ZCode("打开(注册表项:X)")]
        public static void Open(注册表项 X)
        {
            X.Open();
        }

        [ZCode("删除(注册表项:X)")]
        public static void Delete(注册表项 X)
        {
            X.Delete();
        }

        [ZCode("关闭(注册表项:X)")]
        public static void Close(注册表项 X)
        {
            X.Close();
        }

        [ZCode("创建(注册表键:J)")]
        public static void Create(注册表键 J)
        {
            J.Write("");
        }

        [ZCode("删除(注册表键:J)")]
        public static void Delete(注册表键 J)
        {
            J.Delete();
        }
    }
}
