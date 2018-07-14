using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using ZLangRT.Attributes;

namespace Z标准包.操作系统
{
    [ZInstance]
    public class 注册表键
    {
        public 注册表项 Parent { get; private set; }
        public string Name { get;private set; }

        public 注册表键(string parent, string name)
        {
            Parent = new 注册表项( parent);
            Name = name;
        }

        public 注册表键(注册表项 parent,string name)
        {
            Parent = parent;
            Name = name;
        }

        [ZCode("写入数据(string:value)")]
        public void Write(string value)
        {
            Parent.WriteSub(this.Name, value);
            //Parent.Open();
            //Parent.keySub.SetValue(Name, value);
            //Parent.Close();
        }

        [ZCode("读取数据")]
        public string Read()
        {
            Parent.Open();
            string value = Parent.keySub.GetValue(Name).ToString();
            Parent.Close();
            return value;
        }

        public void Delete()
        {
            Parent.DeleteSub(this.Name);
            //Parent.Open();
            //Parent.keySub.DeleteValue(Name);
            //Parent.Close();
        }
    }
}
