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

        [ZCode("创建(文件夹:wjj)")]
        public static void 创建(文件夹 wjj)
        {
            wjj.DirInfo.Create();
        }

        [ZCode("删除(文件夹:wjj)")]
        public static void 删除(文件夹 wjj)
        {
            Directory.Delete(wjj.DirInfo.FullName, true);
            //wjj.DirInfo.Delete();
        }

        [ZCode("复制(文件夹:fromDir)到(文件夹:toDir)")]
        public static void CopyDir(文件夹 fromDir, 文件夹 toDir)
        {
            CopyDir(fromDir.DirInfo.FullName, toDir.DirInfo.FullName);
        }

        [ZCode("移动(文件夹:fromDir)到(文件夹:toDir)")]
        public static void MoveDir(文件夹 fromDir, 文件夹 toDir)
        {
            MoveDir(fromDir.DirInfo.FullName, toDir.DirInfo.FullName);
        }

        public static void CopyDir(string fromDir, string toDir)
        {
            if (!Directory.Exists(fromDir))
                return;

            if (!Directory.Exists(toDir))
            {
                Directory.CreateDirectory(toDir);
            }

            string[] files = Directory.GetFiles(fromDir);
            foreach (string formFileName in files)
            {
                string fileName = Path.GetFileName(formFileName);
                string toFileName = Path.Combine(toDir, fileName);
                File.Copy(formFileName, toFileName);
            }
            string[] fromDirs = Directory.GetDirectories(fromDir);
            foreach (string fromDirName in fromDirs)
            {
                string dirName = Path.GetFileName(fromDirName);
                string toDirName = Path.Combine(toDir, dirName);
                CopyDir(fromDirName, toDirName);
            }
        }

        public static void MoveDir(string fromDir, string toDir)
        {
            if (!Directory.Exists(fromDir))
                return;

            CopyDir(fromDir, toDir);
            Directory.Delete(fromDir, true);
        }

        #endregion


        #region 文件操作

        [ZCode("创建(文件:wj)")]
        public static void 创建(文件 wj)
        {
            wj.FeInfo.Create();
        }

        [ZCode("复制(文件:wa)到(文件:wb)")]
        public static void 复制(文件 wa, 文件 wb)
        {
            File.Copy(wa.FullName, wb.FullName);
        }

        [ZCode("移动(文件:wa)到(文件:wb)")]
        public static void 移动(文件 wa, 文件 wb)
        {
            File.Move(wa.FullName, wb.FullName);
        }

        [ZCode("删除(文件:wj)")]
        public static void 删除(文件 wj)
        {
            File.Delete(wj.FullName);
            //wj.FeInfo.Delete();
        }
        #endregion
    }
}
