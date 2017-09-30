using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using ZLangRT.Attributes;

namespace Z标准包.文件系统
{
    [ZStatic]
    public class 文件系统操作器
    {
        #region 文件夹操作
        [ZCode("创建(DirectoryInfo:wjj)")]
        //[ZCode("创建文件夹(DirectoryInfo:wjj)")]
        public static void 创建(DirectoryInfo wjj)
        {
            wjj.Create();
        }

        //[ZCode("创建文件夹(string:wjj)")]
        //public static void 创建文件夹(string wjj)
        //{
        //    Directory.CreateDirectory(wjj);
        //}

        [ZCode("删除(DirectoryInfo:wjj)")]
        //[ZCode("删除文件夹(DirectoryInfo:wjj)")]
        public static void 删除(DirectoryInfo wjj)
        {
            wjj.Delete();
        }

       //[ZCode("删除文件夹(string:wjj)")]
       // public static void 删除文件夹(string wjj)
       // {
       //     if (Directory.Exists(wjj))
       //         Directory.Delete(wjj);
       // }
        #endregion


       #region 文件操作
       [ZCode("创建(FileInfo:wj)")]
       //[ZCode("创建文件(FileInfo:wj)")]
       public static void 创建(FileInfo wj)
       {
           wj.Create();
       }

       //[ZCode("创建文件(string:wj)")]
       //public static void 创建文件(string wj)
       //{
       //    File.Create(wj);
       //}

       [ZCode("删除(FileInfo:wj)")]
       //[ZCode("删除文(DirectoryInfo:wj)")]
       public static void 删除(FileInfo wj)
       {
           wj.Delete();
       }

       //[ZCode("删除文件(string:wjj)")]
       //public static void 删除文件(string wj)
       //{
       //    if (File.Exists(wj))
       //        File.Delete(wj);
       //}
       #endregion
    }
}
