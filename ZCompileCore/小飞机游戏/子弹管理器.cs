using System;
using ZLangRT;
using ZLangRT.Attributes;
using Z语言系统;

namespace 小飞机游戏
{
	[ZStatic]
	public static class 子弹管理器
	{
		private static 列表<子弹> _子弹群;

		[ZCode("子弹群")]
		public static 列表<子弹> 子弹群
		{
			get
			{
				return 子弹管理器._子弹群;
			}
			set
			{
				子弹管理器._子弹群 = value;
			}
		}

		private static void _InitMemberValueMethodName()
		{
			列表<子弹> 子弹群 = new 列表<子弹>();
			子弹管理器._子弹群 = 子弹群;
		}

		[ZCode("加入(子弹:Z)")]
		public static void 加入子弹(子弹 子弹)
		{
			子弹管理器.子弹群.Add(子弹);
		}

		[ZCode("删除(子弹:Z)")]
		public static void 删除子弹(子弹 子弹)
		{
			子弹管理器.子弹群.Remove(子弹);
		}

		[ZCode("显示子弹")]
		public static void 显示子弹()
		{
			列表<子弹> 子弹群 = 子弹管理器.子弹群;
			int num = 1;
			int count = 子弹群.Count;
			if (Calculater.LEInt(num, count))
			{
				do
				{
					子弹群[(num)].显示();
					num++;
				}
				while (Calculater.LEInt(num, count));
			}
		}

		[ZCode("移动子弹")]
		public static void 移动子弹()
		{
			子弹管理器.清除出界子弹();
			列表<子弹> 子弹群 = 子弹管理器.子弹群;
			int num = 1;
			int count = 子弹群.Count;
			if (Calculater.LEInt(num, count))
			{
				do
				{
					子弹群[(num)].移动();
					num++;
				}
				while (Calculater.LEInt(num, count));
			}
		}

		[ZCode("清除出界子弹")]
		public static void 清除出界子弹()
		{
			列表<子弹> 列表 = new 列表<子弹>();
			列表<子弹> 列表2 = 列表;
			列表<子弹> 子弹群 = 子弹管理器.子弹群;
			int count = 子弹群.Count;
			int num = 1;
			if (Calculater.LEInt(num, count))
			{
				do
				{
					子弹 子弹 = 子弹群[(num)];
					if (Calculater.EQBool(子弹.出界(), false))
					{
						列表2.Add(子弹);
					}
					num++;
				}
				while (Calculater.LEInt(num, count));
			}
			子弹管理器.子弹群 = 列表2;
		}

		[ZCode("取得子弹数量")]
		public static int 取得子弹数量()
		{
			return 子弹管理器.子弹群.Count;
		}

		static 子弹管理器()
		{
			子弹管理器._InitMemberValueMethodName();
		}
	}
}
