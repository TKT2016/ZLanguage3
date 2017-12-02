using System;
using System.Drawing;
using ZLangRT;
using ZLangRT.Attributes;
using Z标准包.绘图.形状;
using Z标准包.文件系统;

namespace 小飞机游戏
{
	[ZInstance]
	public class 物体
	{
		private string _名称;

		private int _X坐标;

		private int _Y坐标;

		private int _长度;

		private int _高度;

		private int _X速度;

		private int _Y速度;

		private 图片 _图片;

		[ZCode("名称")]
		public string 名称
		{
			get
			{
				return this._名称;
			}
			set
			{
				this._名称 = value;
			}
		}

		[ZCode("X坐标")]
		public int X坐标
		{
			get
			{
				return this._X坐标;
			}
			set
			{
				this._X坐标 = value;
			}
		}

		[ZCode("Y坐标")]
		public int Y坐标
		{
			get
			{
				return this._Y坐标;
			}
			set
			{
				this._Y坐标 = value;
			}
		}

		[ZCode("长度")]
		public int 长度
		{
			get
			{
				return this._长度;
			}
			set
			{
				this._长度 = value;
			}
		}

		[ZCode("高度")]
		public int 高度
		{
			get
			{
				return this._高度;
			}
			set
			{
				this._高度 = value;
			}
		}

		[ZCode("X速度")]
		public int X速度
		{
			get
			{
				return this._X速度;
			}
			set
			{
				this._X速度 = value;
			}
		}

		[ZCode("Y速度")]
		public int Y速度
		{
			get
			{
				return this._Y速度;
			}
			set
			{
				this._Y速度 = value;
			}
		}

		[ZCode("图片")]
		public 图片 图片
		{
			get
			{
				return this._图片;
			}
			set
			{
				this._图片 = value;
			}
		}

		private static void _InitMemberValueMethodName(物体 物体)
		{
			物体._名称 = "";
			物体._X坐标 = 0;
			物体._Y坐标 = 0;
			物体._长度 = 0;
			物体._高度 = 0;
			物体._X速度 = 0;
			物体._Y速度 = 0;
		}

		[ZCode("取得中心位置")]
        public virtual Point 取得中心位置()
		{
			float num = Calculater.AddFloat((float)this.X坐标, Calculater.DivInt(this.长度, 2));
			float num2 = Calculater.AddFloat((float)this.Y坐标, Calculater.DivInt(this.高度, 2));
			int x = Calculater.Cast<int>(num);
			int y = Calculater.Cast<int>(num2);
			return new Point(x, y);
		}

		[ZCode("显示")]
        public virtual void 显示()
		{
			Point point = new Point(this.X坐标, this.Y坐标);
			战场参数.绘图器.在_绘制(point, this.图片);
		}

		[ZCode("移动")]
        public virtual void 移动()
		{
			this.X坐标 = Calculater.AddInt(this.X坐标, this.X速度);
			this.Y坐标 = Calculater.AddInt(this.Y坐标, this.Y速度);
		}

		[ZCode("出界")]
        public virtual bool 出界()
		{
			return Calculater.LTInt(Calculater.AddInt(this.X坐标, this.长度), 0) || Calculater.GTInt(this.X坐标, 战场参数.长度) || Calculater.LTInt(Calculater.AddInt(this.Y坐标, this.高度), 0) || Calculater.GTInt(this.Y坐标, 战场参数.高度);
		}

		[ZCode("碰撞到(物体:W)")]
        public virtual bool 碰撞到物体(物体 W)
		{
			矩形 矩形 = new 矩形(this.长度, this.高度);
			矩形.左上角位置=(new Point(this.X坐标, this.Y坐标));
			矩形 矩形2 = new 矩形(W.长度, W.高度);
			矩形2.左上角位置=(new Point(W.X坐标, W.Y坐标));
			return 矩形.相交于(矩形2);
		}

		public 物体()
		{
			物体._InitMemberValueMethodName(this);
		}
	}
}
