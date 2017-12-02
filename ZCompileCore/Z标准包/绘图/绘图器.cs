using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZLangRT;
using ZLangRT.Attributes;
using Z标准包.绘图.形状;
using Z标准包.文件系统;

namespace Z标准包.绘图
{
    [ZInstance]
    public class 绘图器 
    {
        Graphics graphics;

        public 绘图器(Graphics graphics)
        {
            this.graphics = graphics;
        }

        public Brush 笔刷 { get; set; }
        public Pen 画笔 { get; set; }

        [ZCode("绘制(矩形:rect)")]
        public void 绘制(矩形 rect)
        {
            graphics.DrawRectangle(画笔, rect.左上角位置.X, rect.左上角位置.Y,rect.长度,rect.宽度);
        }

        public void 绘制(线段 line)
        {
            graphics.DrawLine(画笔, line.第一点, line.第二点);
        }

        [ZCode("绘制(圆:circle)")]
        public void 绘制(圆 circle)
        {
            graphics.DrawEllipse(画笔, circle.圆心.X, circle.圆心.Y, circle.半径*2,circle.半径*2);
        }

        public void 绘制(椭圆 ellipse)
        {
            graphics.DrawEllipse(画笔, ellipse.圆心.X, ellipse.圆心.Y, ellipse.X轴半径 * 2, ellipse.Y轴半径 * 2);
        }

        public void 绘制(文本块 W)
        {
            //DrawString(string s, Font font, Brush brush, float x, float y);
            graphics.DrawString(W.内容,new Font(W.字体类型,W.字体大小),笔刷,W.位置);
        }

        [ZCode("在(Point:point)绘制(图片:image)")]
        public void 在_绘制(Point point, 图片 image)
        {
            graphics.DrawImage(image.ImageInfo,point);
        }

        public void 销毁()
        {
            graphics.Dispose();
        }

        [ZCode("以(Color:color)清除背景")]
        public void Clear(Color color)
        {
            graphics.Clear(color);
        }
    }
}
