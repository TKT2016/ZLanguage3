using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ZCompileKit.Infoes;

namespace ZCompileCore.Engines
{
    public class ZFileModel
    {
        public ZCompileFileInfo ZFileInfo { get; private set; }

        public ZFileModel(ZCompileFileInfo zFileInfo)
        {
            ZFileInfo = zFileInfo;
        }
        /// <summary>
        /// 项目实体
        /// </summary>
        //public ZyyProjectModel Project { get; set; }

        ///// <summary>
        ///// Z语言源文件
        ///// </summary>
        //public FileInfo SourceFileInfo { get; set; }

        ///// <summary>
        ///// 编译前在Z程序前的补充代码
        ///// </summary>
        //public string PreSourceCode { get; set; }

        public string GetFileNameNoEx()
        {
            if(ZFileInfo.IsVirtual)
            {
                return ZFileInfo.VirtualFileName;
            }
            else
            {
                return Path.GetFileNameWithoutExtension(ZFileInfo.RealFilePath);
            }      
        }

        
    }
}
