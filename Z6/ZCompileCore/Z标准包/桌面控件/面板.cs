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
    [ZInstance(typeof(ZPanel))]
    public abstract class 面板
    {
        [ZCode("背景色")]
        public Color BackColor { get; set; }
    }
}
