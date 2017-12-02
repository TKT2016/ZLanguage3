using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using ZLangRT.Attributes;

namespace Z标准包.网络
{
    [ZInstance]
    public class IP地址
    {
        IPAddress ipaddr;

        public IP地址(string ip)
        {
            ipaddr = IPAddress.Parse(ip);
        }

    }
}
