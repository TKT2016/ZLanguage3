﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZLangRT;
using ZLangRT.Attributes;

namespace Z标准包.绘图.形状
{
    [ZStatic]
    public static class 点辅助 
    {
        [ZCode("点(float:x,float:y)")]
        public static Point 点(float x,float y)
        {
            return new Point((int)x, (int)y);
        }
    }
}
