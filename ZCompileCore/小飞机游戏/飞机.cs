using System;
using ZLangRT;
using ZLangRT.Attributes;
using Z标准包.文件系统;

namespace 小飞机游戏
{
	[ZInstance]
	public class 飞机 : 物体
	{
		private int _等级;

		private int _发射计数器;

		[ZCode("等级")]
		public int 等级
		{
			get
			{
				return this._等级;
			}
			set
			{
				this._等级 = value;
			}
		}

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

		private static void _InitMemberValueMethodName(飞机 飞机)
		{
			飞机._等级 = 1;
			飞机._发射计数器 = 0;
		}

		[ZCode("升级")]
        public virtual void 升级()
		{
			this.等级 = Calculater.AddInt(this.等级, 1);
		}

		[ZCode("发射子弹")]
        public virtual void 发射子弹()
		{
			if (Calculater.GTInt(this.发射计数器, 4))
			{
				子弹管理器.加入子弹(new 子弹
				{
					X坐标 = Calculater.AddInt(this.X坐标, 24),
					Y坐标 = Calculater.SubInt(this.Y坐标, 24),
					图片 = new 图片( "xfjres/red_bullet.png"),
					Y速度 = 0 - 20,
					子弹类型 = 子弹类型.玩家子弹
				});
				this.发射计数器 = 0;
			}
			else
			{
				this.发射计数器 = Calculater.AddInt(this.发射计数器, 1);
			}
		}

		public 飞机()
		{
			飞机._InitMemberValueMethodName(this);
			this.X坐标 = 150;
			this.Y坐标 = 100;
			this.长度 = 48;
			this.高度 = 48;
            this.图片 = new 图片("xfjres/icon48x48.png");
		}
	}
}
