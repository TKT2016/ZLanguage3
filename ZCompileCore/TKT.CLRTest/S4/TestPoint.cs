using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZLangRT.Attributes;

namespace TKT.CLRTest.S4
{
    [ZInstance]
    public class TestPoint
    {
        [ZCode("鼠标位置")]
        public Point 鼠标位置 { get; set; }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            鼠标位置 = new Point(e.X, e.Y);
            响应鼠标移动();
            
        }

        [ZCode("响应鼠标移动")]
        public virtual void 响应鼠标移动()
        {
            this.玩家.X坐标 = this.鼠标位置.X;
            this.玩家.Y坐标 = this.鼠标位置.Y;
        }

        private FJ 玩家 { get; set; }

        public class FJ
        {
            public int X坐标 { get; set; }
            public int Y坐标 { get; set; }
        }
    }

    
}
