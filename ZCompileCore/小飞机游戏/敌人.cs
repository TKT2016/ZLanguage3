using System;
using ZLangRT;
using ZLangRT.Attributes;
using Z标准包.文件系统;

namespace 小飞机游戏
{
	[ZInstance]
	public class 敌人 : 物体
	{
		private int _发射计数器;

		[ZCode("发射计数器")]
		public int 发射计数器
		{
			get
			{
				return this._发射计数器;
			}
			set
			{
				this._发射计数器 = value;
			}
		}

		private static void _InitMemberValueMethodName(敌人 敌人)
		{
			敌人._发射计数器 = 0;
		}

		[ZCode("发射子弹")]
		public virtual void 发射子弹()
		{
			if (Calculater.GTInt(this.发射计数器, 25))
			{
				子弹管理器.加入子弹(new 子弹
				{
					X坐标 = Calculater.AddInt(this.X坐标, 20),
					Y坐标 = Calculater.AddInt(this.Y坐标, 5),
                    图片 = new 图片("xfjres/grey_bullet.png"),
					Y速度 = 4,
					子弹类型 = 子弹类型.敌人子弹
				});
				this.发射计数器 = 0;
			}
			else
			{
				this.发射计数器 = Calculater.AddInt(this.发射计数器, 1);
			}
		}

		[ZCode("移动")]
		public override void 移动()
		{
			this.X坐标 = Calculater.AddInt(this.X坐标, this.X速度);
			this.Y坐标 = Calculater.AddInt(this.Y坐标, this.Y速度);
			if (this.出界())
			{
				this.X速度 = 0 - this.X速度;
				this.Y速度 = 0 - this.Y速度;
				this.X坐标 = Calculater.AddInt(this.X坐标, this.X速度);
				this.Y坐标 = Calculater.AddInt(this.Y坐标, this.Y速度);
			}
		}

		public 敌人()
		{
			敌人._InitMemberValueMethodName(this);
			this.长度 = 40;
			this.高度 = 28;
            this.图片 = new 图片("xfjres/DiJi.png");
		}
	}
}
