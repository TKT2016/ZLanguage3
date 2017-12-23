using System;
using System.Drawing;
using System.Windows.Forms;
using ZLangRT;
using ZLangRT.Attributes;
using Z标准包.绘图;
using Z标准包.文件系统;
using Z标准包.桌面控件;
using Z语言系统;

namespace 小飞机游戏
{
	[ZInstance]
	public class 战场 : 动画窗体
	{
		private 飞机 _玩家;

		private 游戏状态 _状态;

		[ZCode("玩家")]
		public 飞机 玩家
		{
			get
			{
				return this._玩家;
			}
			set
			{
				this._玩家 = value;
			}
		}

		[ZCode("状态")]
		public 游戏状态 状态
		{
			get
			{
				return this._状态;
			}
			set
			{
				this._状态 = value;
			}
		}

		private static void _InitMemberValueMethodName(战场 战场)
		{
			战场._玩家 = new 飞机();
			战场._状态 = 游戏状态.开始;
		}

		[ZCode("初始化")]
		public override void 初始化()
		{
			敌机管理器.生成一群敌人();
		}

		[ZCode("响应鼠标移动")]
		public override void 响应鼠标移动()
		{
			//Point point=this.鼠标位置;
			//this.玩家.X坐标 = point.X;
            //Point point2 = this.鼠标位置;
			//this.玩家.Y坐标 = point2.Y;
            this.玩家.X坐标 = 鼠标位置.X;
            this.玩家.Y坐标 = 鼠标位置.Y;
		}

		[ZCode("用(绘图器:HTQ)绘图")]
		public override void 绘图(绘图器 HTQ)
		{
			战场参数.绘图器 = HTQ;
			HTQ.在_绘制(new Point(0, 0), new 图片("xfjres/bg_02.jpg"));
			this.玩家.显示();
			子弹管理器.显示子弹();
			敌机管理器.显示敌人();
			if (Calculater.EQRef(this.状态, 游戏状态.结束))
			{
				控制台.Write("游戏结束");
			}
		}

		[ZCode("更新")]
		public override void 更新()
		{
			if (Calculater.EQRef(this.状态, 游戏状态.开始))
			{
				this.检查碰撞();
				子弹管理器.移动子弹();
				this.玩家.发射子弹();
				敌机管理器.发射子弹();
				敌机管理器.移动敌人();
			}
		}

		[ZCode("检查碰撞")]
        public virtual void 检查碰撞()
		{
			列表<子弹> 子弹群 = 子弹管理器.子弹群;
			int count = 子弹群.Count;
			int num = 1;
			if (Calculater.LEInt(num, count))
			{
				do
				{
					子弹 子弹 = 子弹群[(num)];
					if (Calculater.EQRef(子弹.子弹类型, 子弹类型.敌人子弹))
					{
						if (子弹.碰撞到物体(this.玩家))
						{
							this.状态 = 游戏状态.结束;
						}
					}
					num++;
				}
				while (Calculater.LEInt(num, count));
			}
		}

		[ZCode("在(点:D)显示提示文字(文本:W)")]
        public virtual void 在点显示提示文字文本(Point D, string W)
		{
			控制台.Write(W);
		}

		[ZCode("响应键盘(键盘参数:J)")]
        public virtual void 响应键盘键盘参数(KeyPressEventArgs J)
		{
			if (Calculater.EQFloat((float)J.KeyChar, (float)键盘按键.空格键))
			{
				this.状态 = 游戏状态.开始;
			}
		}

		public 战场()
		{
			战场._InitMemberValueMethodName(this);
			this.启动时窗口居中();
			this.Text = "飞机大战";
			this.设置尺寸(战场参数.长度, 战场参数.高度);
			this.玩家 = new 飞机();
		}
	}
}
