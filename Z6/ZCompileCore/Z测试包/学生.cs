using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLangRT.Attributes;

namespace Z测试包
{
    [ZInstance]
    public class 学生
    {
        [ZCode("姓名")]
        public string Name { get; set; }

        [ZCode("年龄")]
        public int Arg { get; set; }

        [ZCode("学号")]
        public string Code { get; set; }

        public 学生(string 姓名, int 年龄,string 学号)
        {
            Name = 姓名;
            Arg = 年龄;
            Code = 学号;
        }
    }
}
