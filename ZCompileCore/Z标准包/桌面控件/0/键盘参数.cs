using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZLangRT;
using ZLangRT.Attributes;

namespace Z标准包.桌面控件
{
    [ZInstance(typeof(KeyPressEventArgs))]
    public class 键盘参数
    {
        [ZCode("按键")]
        public char KeyChar { get; set; }
    }
}
