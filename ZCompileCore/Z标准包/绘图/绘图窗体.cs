using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZLangRT.Attributes;

namespace Z标准包.绘图
{
    [ZInstance]
    public class 绘图窗体:Form
    {
        [ZCode("绘图器")]
        public 绘图器 绘图器 { get; set; }

        public 绘图窗体()
        {
            this.BackColor = Color.White;
            this.Width = 800;
            this.Height = 600;
            this.Text = "绘图窗体";
            //this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormPaint);
            Graphics g = this.CreateGraphics();
            this.绘图器 = new 绘图器(g);
        }

        //private void FormPaint(object sender, PaintEventArgs e)
        //{
        //    Graphics g = e.Graphics;
        //    this.绘图器 = new 绘图器(g);
        //    this.绘制图形();
        //}

        public virtual void 绘制图形()
        {

        }
    }
}
