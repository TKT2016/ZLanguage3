using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using ZLangRT.Attributes;
using Z标准包.文件系统;
using System.IO;

namespace ZExcel开发包
{
    [ZInstance]
    public class Excel工作文件
    {
        private ExcelPackage WorkPackage;

        public Excel工作文件()
        {
            WorkPackage = new ExcelPackage();
        }

        public Excel工作文件(文件 W)
        {
            WorkPackage = new ExcelPackage(W.FileInfo);
        }

        public Excel工作文件(string F)
        {
            FileInfo FeInfo = new FileInfo(F);
            WorkPackage = new ExcelPackage(FeInfo);
        }

        [ZCode("释放资源")]
        public void Dispose()
        {
            WorkPackage.Dispose();
        }

        private Excel工作簿 _工作簿;

        [ZCode("工作簿")]
        public Excel工作簿 工作簿
        {
            get { 
                if (_工作簿 == null) {
                    _工作簿 = new Excel工作簿(WorkPackage.Workbook);
                }
                return _工作簿;
            }
        }

        [ZCode("保存")]
        public void Save()
        {
            WorkPackage.Save();
        }

        [ZCode("另存为(文件:W)")]
        public void SaveAs(文件 W)
        {
            WorkPackage.SaveAs(W.FileInfo);
        }

        [ZCode("另存为(文本:F)")]
        public void SaveAs(string F)
        {
            FileInfo FeInfo = new FileInfo(F);
            WorkPackage.SaveAs(FeInfo);
        }
    }
}
