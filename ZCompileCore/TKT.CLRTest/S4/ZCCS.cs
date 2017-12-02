using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLangRT.Attributes;
using Z标准包.绘图;

namespace TKT.CLRTest.S4
{
    [ZInstance]
	public class 战场参数
	{
		private int _长度;

		private int _高度;

		private 绘图器 _绘图器;

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

		[ZCode("绘图器")]
		public 绘图器 绘图器
		{
			get
			{
				return this._绘图器;
			}
			set
			{
				this._绘图器 = value;
			}
		}

        //private static void <_InitMemberValueMethodName>(战场参数 战场参数)
        //{
        //    战场参数._长度 = 534;
        //    战场参数._高度 = 562;
        //}

		public 战场参数()
		{
			//战场参数.<_InitMemberValueMethodName>(this);
		}
	}
}
