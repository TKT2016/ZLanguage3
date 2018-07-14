using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using ZLangRT.Attributes;
using Z语言系统;

namespace Z标准包.设备
{
    [ZStatic]
    public static class 驱动器控制器
    {
        [ZCode("获取所有硬盘")]
        public static 列表<硬盘> GetAllHardDisk()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
            列表<硬盘> list = new 列表<硬盘>();
            foreach (ManagementObject mo in searcher.Get())
            {
                硬盘 model = new 硬盘();
                model.编号 = mo["SerialNumber"].ToString().Trim();
                list.Add(model);
            }
            return list;
        }

        [ZCode("获取所有驱动器")]
        public static 列表<驱动器> GetAllDriveInfo()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_logicaldisk");
            列表<驱动器> list = new 列表<驱动器>();
            foreach (ManagementObject disk in searcher.Get())
            {
                string name = disk["Name"].ToString().Trim();
                驱动器 model = new 驱动器(name);
                list.Add(model);
            }
            return list;
        }
    }
}
