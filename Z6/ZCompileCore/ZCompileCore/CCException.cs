using System;

namespace ZCompileCore
{
    /// <summary>
    /// 编译异常
    /// </summary>
    public class CCException:Exception
    {
        public CCException( )
            : base("编译错误")
        {

        }

        public CCException(string message)
            : base(message)
        {

        }
    }
}
