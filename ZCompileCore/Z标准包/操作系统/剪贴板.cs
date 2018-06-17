using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using ZLangRT.Attributes;
using System.Windows.Forms;

namespace Z标准包.操作系统
{
    [ZStatic]
    public static class 剪贴板
    {
        [ZCode("数据")]
        public static object Data
        {
            set
            {
                Clipboard.SetDataObject(value, false);
            }
            get
            {
                IDataObject iData = Clipboard.GetDataObject();
                return iData;
            }
        }

        [ZCode("有数据")]
        public static bool IsNull()
        {
            var data = Clipboard.GetDataObject();
            if(data==null) return false;
            return (data.GetFormats().Length != 0);
        }

        [ZCode("设置文本(string:str)")]
        public static void 设置文本(string str)
        {
            Clipboard.SetText(str);
        }

        [ZCode("获取文本")]
        public static string 获取文本()
        {
            string str= Clipboard.GetText();
            if(str==null)
            {
                str = "";
            }
            return str;
        }
    }
}
