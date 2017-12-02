using System;
using ZLangRT;
using ZLangRT.Attributes;
using Z语言系统;

namespace 小飞机游戏
{
	[ZStatic]
	public static class 敌机管理器
	{
		private static 列表<敌人> _敌群;

		[ZCode("敌群")]
		public static 列表<敌人> 敌群
		{
			get
			{
				return 敌机管理器._敌群;
			}
			set
			{
				敌机管理器._敌群 = value;
			}
		}

		private static void _InitMemberValueMethodName()
		{
			列表<敌人> 敌群 = new 列表<敌人>();
			敌机管理器._敌群 = 敌群;
		}

		[ZCode("显示敌人")]
		public static void 显示敌人()
		{
			列表<敌人> 敌群 = 敌机管理器.敌群;
			int num = 1;
			int count = 敌群.Count;
			if (Calculater.LEInt(num, count))
			{
				do
				{
					敌群[num].显示();
					num++;
				}
				while (Calculater.LEInt(num, count));
			}
		}

		[ZCode("发射子弹")]
		public static void 发射子弹()
		{
			列表<敌人> 敌群 = 敌机管理器.敌群;
			int num = 1;
			int count = 敌群.Count;
			if (Calculater.LEInt(num, count))
			{
				do
				{
					敌群[num].显示();
					num++;
				}
				while (Calculater.LEInt(num, count));
			}
		}

		[ZCode("生成一个敌人")]
		public static void 生成一个敌人()
		{
			int x坐标 = 随机数器.生成随机数(0, 520);
			int y坐标 = 随机数器.生成随机数(0, 200);
			敌人 敌人 = new 敌人();
			敌人.X坐标 = x坐标;
			敌人.Y坐标 = y坐标;
			敌人.X速度 = 随机数器.生成随机数(0 - 4, 4);
			敌人.Y速度 = 随机数器.生成随机数(0 - 4, 4);
			敌机管理器.敌群.Add(敌人);
		}

		[ZCode("生成一群敌人")]
		public static void 生成一群敌人()
		{
			敌机管理器.生成一个敌人();
			敌机管理器.生成一个敌人();
			敌机管理器.生成一个敌人();
		}

		[ZCode("移动敌人")]
		public static void 移动敌人()
		{
			列表<敌人> 列表 = new 列表<敌人>();
			列表<敌人> 列表2 = 列表;
			列表<敌人> 敌群 = 敌机管理器.敌群;
			int count = 敌群.Count;
			int num = 1;
			if (Calculater.LEInt(num, count))
			{
				do
				{
					敌人 敌人 = 敌群[num];
					if (Calculater.EQBool(敌人.出界(), false))
					{
						列表2.Add(敌人);
					}
					num++;
				}
				while (Calculater.LEInt(num, count));
			}
			敌机管理器.敌群.Clear();
			敌机管理器.敌群 = 列表2;
			列表<敌人> 敌群2 = 敌机管理器.敌群;
			int num2 = 1;
			int count2 = 敌群2.Count;
			if (Calculater.LEInt(num2, count2))
			{
				do
				{
					敌群2[(num2)].移动();
					num2++;
				}
				while (Calculater.LEInt(num2, count2));
			}
		}

		[ZCode("取得敌人数量")]
		public static int 取得敌人数量()
		{
			return 敌机管理器.敌群.Count;
		}

		static 敌机管理器()
		{
			敌机管理器._InitMemberValueMethodName();
		}
	}
}
