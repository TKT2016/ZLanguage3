using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileDesc.Descriptions;
using ZCompileDesc.ZTypes;

namespace ZCompileDesc.Collections
{
    public interface IZDescTypeDictionary
    {
        bool ContainsZDescType(string zname);
        IZDescType[] SearchZDescType(string zname);
    }
}
