using System;
using ZLangRT.Attributes;

namespace 小飞机游戏
{
    [ZEnum]
    public enum 游戏状态
    {
        [ZCode("开始")]
        开始 = 1,
        [ZCode("暂停")]
        暂停,
        [ZCode("结束")]
        结束
    }
}
