using System;
using ZLangRT.Attributes;
using Z标准包.绘图;

namespace 小飞机游戏
{
	[ZStatic]
	public static class 战场参数
	{
		private static int _长度;

		private static int _高度;

		private static 绘图器 _绘图器;

		[ZCode("长度")]
		public static int 长度
		{
			get
			{
				return 战场参数._长度;
			}
			set
			{
				战场参数._长度 = value;
			}
		}

		[ZCode("高度")]
		public static int 高度
		{
			get
			{
				return 战场参数._高度;
			}
			set
			{
				战场参数._高度 = value;
			}
		}

		[ZCode("绘图器")]
		public static 绘图器 绘图器
		{
			get
			{
				return 战场参数._绘图器;
			}
			set
			{
				战场参数._绘图器 = value;
			}
		}

		private static void _InitMemberValueMethodName()
		{
			战场参数._长度 = 534;
			战场参数._高度 = 562;
		}

		static 战场参数()
		{
			战场参数._InitMemberValueMethodName();
		}
	}
}
