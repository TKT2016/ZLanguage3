using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileDesc.Descriptions;
using ZCompileDesc.ZTypes;

namespace ZCompileDesc.Collections
{
    public interface IZTypeDictionary
    {
        bool ContainsZType(string zname);
        ZType[] SearchZType(string zname);
    }
}
