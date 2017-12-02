using System;
using ZLangRT.Attributes;

namespace 小飞机游戏
{
	[ZInstance]
	public class 子弹 : 物体
	{
		private 子弹类型 _子弹类型;

		[ZCode("子弹类型")]
		public 子弹类型 子弹类型
		{
			get
			{
				return this._子弹类型;
			}
			set
			{
				this._子弹类型 = value;
			}
		}

		private static void _InitMemberValueMethodName(子弹 子弹)
		{
		}

		public 子弹()
		{
			子弹._InitMemberValueMethodName(this);
			this.长度 = 11;
			this.高度 = 21;
		}
	}
}
