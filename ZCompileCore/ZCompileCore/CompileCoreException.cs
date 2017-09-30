using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCompileCore
{
    public class CompileCoreException:Exception
    {
        public CompileCoreException( )
            : base("编译错误")
        {

        }

        public CompileCoreException(string message)
            : base(message)
        {

        }
    }
}
