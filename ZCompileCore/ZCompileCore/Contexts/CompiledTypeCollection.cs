using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZCompileDesc.Collections;
using ZCompileDesc.Utils;
using ZCompileDesc.ZTypes;

namespace ZCompileDesc.ZTypes
{
    public class CompiledTypeCollection :IZTypeDictionary// IWordDictionary, 
    {
        List<IZDescType> CompiledTypes ;

        public CompiledTypeCollection()
        {
            CompiledTypes = new List<IZDescType> ();

        }

        public void Add(IZDescType ztype)
        {
            CompiledTypes.Add(ztype);
        }

        public void AddRange(IEnumerable< IZDescType> ztype)
        {
            CompiledTypes.AddRange(ztype);
        }

        public List<IZDescType> ToList()
        {
             List<IZDescType> CompiledTypes2 = new List<IZDescType> () ;
            CompiledTypes2.AddRange(CompiledTypes);
            return CompiledTypes2;
        }

        public IZDescType Get(string name)
        {
            foreach (var type in CompiledTypes)
            {
                if (type.ZName == name)
                    return type;
            }
            return null;
        }

        public bool ContainsZType(string zname)
        {
            IZDescType ztype = Get(zname);
            if (ztype == null) return false;
            if (!(ztype is ZType)) return false;
            return true;
        }

        public ZType[] SearchZType(string zname)
        {
            IZDescType ztype = Get(zname);
            if (ztype == null) return new ZType[]{};
            if (!(ztype is ZType)) return new ZType[] { };
            return new ZType[] { ztype as ZType };
        }

        public bool ContainsName(string text)
        {
            foreach (var item in CompiledTypes)
            {
                IZDescType zd = (IZDescType)item;
                if(zd.ZName==text)
                    return true;
            }
            return false;
        }

        public override string ToString()
        {
            return string.Format("{0}({1}))",
                "CompiledTypeCollection", string.Join(",", CompiledTypes.Select(P=>P.ZName)));
        }

       
    }
}
