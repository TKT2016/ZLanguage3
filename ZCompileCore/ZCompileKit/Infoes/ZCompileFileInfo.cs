using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ZCompileKit.Infoes
{
    public class ZCompileFileInfo
    {
        public bool IsVirtual { get; set; }
        public string RealFilePath { get; private set; }

        public FileInfo RealFileInfo { get; private set; }
        public string VirtualFileName { get; private set; }

        public string FilePreText { get; private set; }

        public string FileVirtualText { get; private set; }

        public ZCompileFileInfo(bool isVirtual,string filePath,string preText, string virtualText)
        {
            IsVirtual = isVirtual;
            if(!isVirtual)
            {
                RealFilePath = filePath;
                RealFileInfo = new FileInfo(RealFilePath);
            }
            else
            {
                VirtualFileName=filePath;
            }

            FilePreText = preText??"";
            FileVirtualText = virtualText ?? "";
        }

        public string ZFileName
        {
            get
            {
                if (!IsVirtual)
                {
                    return RealFileInfo.Name;
                }
                else
                {
                    return VirtualFileName;
                }
            }
        }
    }
}
