using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLangRT.Attributes;
using ZLogoEngine;
using Z语言系统;

namespace ZLTest
{
    public class TestCallTimes1 : TurtleForm
    {
        //private sealed class Nested1
        //{
        //    public void Nested1$$CALL()
        //    {
        //        this.旋转绘制三角();
        //    }
        //}
        int k = 12;

        [ZCode("开始绘图2")]
        public  void RunZLogoInt(int x)
        {
            int a = 2+x*x;
            //重复绘图.Nested1 @object = new 重复绘图.Nested1();
            补语控制.执行_次(() => { 旋转绘制三角(); }, k+x+a);
        }

		[ZCode("开始绘图")]
		public override void RunZLogo( )
		{
			//重复绘图.Nested1 @object = new 重复绘图.Nested1();
            补语控制.执行_次(() => { 旋转绘制三角(); },k);
		}

		[ZCode("旋转绘制三角")]
		public void 旋转绘制三角()
		{
			this.Turtle.RotateLeft((float)90);
            this.Turtle.Forward((float)200);
            this.Turtle.RotateLeft((float)120);
            this.Turtle.Forward((float)200);
            this.Turtle.RotateLeft((float)120);
            this.Turtle.Forward((float)200);
		}
    }
}
