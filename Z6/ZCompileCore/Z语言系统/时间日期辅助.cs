﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZLangRT;
using ZLangRT.Attributes;

namespace Z语言系统
{
    [ZStatic]
    public class 时间日期辅助
    {
        [ZCode("取得现在时间")]
        public static DateTime 取得现在时间()
        {
            return DateTime.Now;
        }

        [ZCode("计算(时间日期:dt)星期几")]
        public static int 计算星期几(DateTime dt)
        {
            DayOfWeek weekday = dt.DayOfWeek;
            if(weekday== DayOfWeek.Sunday)
            {
                return 7;
            }
            else
            {
                return (int)weekday;
            }
        }
    }
}
