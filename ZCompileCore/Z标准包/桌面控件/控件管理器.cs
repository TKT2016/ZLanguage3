using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZLangRT;
using ZLangRT.Attributes;

namespace Z标准包.桌面控件
{
    [ZStatic]
    public class 控件管理器 
    {
        [ZCode("初始化")]
        public static void 初始化()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
        }

        [ZCode("启动(ZForm:form)")]
        public static void 启动(ZForm form)
        {
            Application.Run(form);
        }

    }
}
