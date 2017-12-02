using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z语言系统;

namespace TKT.CLRTest.S4
{
    public static class FieldTest
    {
        //public int X坐标 { get; set; }
        //public int Y坐标 { get; set; }
        //public void Show()
        //{
        //    var D = new Point(X坐标, Y坐标);
        //}

        public static 列表<子弹> 子弹群 { get; set; }

        static FieldTest()
        {
            子弹群 = new 列表<子弹>();
        }

        public static int 取得子弹数量()
        {
            int a = 子弹群.Count;
            return a;
        }

        //public int A1 ;
        //public string A2 ;

        //public FieldTest()
        //{
        //    _Init(this);
        //}

        //private static void _Init(FieldTest ft)
        //{
        //    ft.A1=8;
        //    ft.A2 = "AA22";
        //}
    }

    public class 子弹
    {

    }
}
