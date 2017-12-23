using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileDesc.Descriptions;

namespace ZCompileDesc.Collections
{
    public interface IZTypeDictionary
    {
        bool ContainsZType(string zname);
        ZLType[] SearchZType(string zname);
    }
}
