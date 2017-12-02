using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Z标准包.桌面控件;

namespace 小飞机游戏
{
    class 主程序
    {
        //static void Main(string[] args)
        //{
        //}

        [STAThread]
        public static void Main()
        {
            控件管理器.初始化();
            战场 战场 = new 战场();
            控件管理器.启动(战场);
        }

        static 主程序()
        {
        }
    }
}
