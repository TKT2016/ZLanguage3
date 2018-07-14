using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ZLangRT.Attributes;

namespace Z标准包.设备
{
    //[ZInstance(typeof(DriveInfo))]
    [ZInstance]
    public class 驱动器
    {
        private DriveInfo driveInfo;

        public 驱动器(string driveName)
        {
            driveInfo = new DriveInfo(driveName);
        }

        [ZCode("可用空闲空间量")]
        public int AvailableFreeSpace { get { return (int)(driveInfo.AvailableFreeSpace / 1000 / 1000); } }

        [ZCode("文件系统名称")]
        public string DriveFormat { get { return driveInfo.DriveFormat; } }

        [ZCode("已准备好")]
        public bool IsReady { get { return driveInfo.IsReady; } }

        [ZCode("名称")]
        public string Name { get { return driveInfo.Name; } }

        //[ZCode("可用空闲空间量")]
        //public int TotalFreeSpace { get { return (int)(driveInfo.TotalFreeSpace / 1024 / 1024); } }

        [ZCode("总大小")]
        public int TotalSize { get { return (int)(driveInfo.TotalSize / 1000 / 1000); } }

        [ZCode("卷标")]
        public string VolumeLabel { get { return driveInfo.VolumeLabel; } }
    }
}
