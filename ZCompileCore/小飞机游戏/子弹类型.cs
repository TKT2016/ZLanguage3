using System;
using ZLangRT.Attributes;

namespace 小飞机游戏
{
    [ZEnum]
    public enum 子弹类型
    {
        [ZCode("敌人子弹")]
        敌人子弹 = 1,
        [ZCode("玩家子弹")]
        玩家子弹
    }
}
