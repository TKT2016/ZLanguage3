using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Symbols;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;
using ZCompileDesc.ZMembers;
using ZCompileDesc.ZTypes;
using ZCompileDesc;

namespace ZCompileCore.Contexts
{
    public class ContextUse
    {
        //public ContextFile FileContext { get; private set; }
        public List<ZClassType> UseZClassList { get; private set; }
        public List<ZEnumType> UseZEnumList { get; private set; }
        public List<ZDimType> UseZDimList { get; private set; }
        //public UseSymbolTable SymbolTable { get; private set; }
        public ContextUse()
        {
            UseZClassList = new List<ZClassType>();
            UseZEnumList = new List<ZEnumType>();
            UseZDimList = new List<ZDimType>();
            //SymbolTable = new UseSymbolTable("Use", null, UseZClassList, UseZEnumList);
        }

        Dictionary<string, object> useTypes = new Dictionary<string, object>();

        public void Add(string ztypeName)
        {
            useTypes.Add(ztypeName, ztypeName);
        }

        public bool Contains(string ztypeName)
        {
            return useTypes.ContainsKey(ztypeName);
        }

        public void AddUseType(IZDescType iztype)
        {
            string key = iztype.ZName;
            useTypes.Remove(key);
            useTypes.Add(iztype.ZName, iztype);
        }

        public void Add(IZDescType iztype)
        {
            if(iztype is ZEnumType)
            {
                UseZEnumList.Add(iztype as ZEnumType);
            }
            else if (iztype is ZDimType)
            {
                UseZDimList.Add(iztype as ZDimType);
            }
            else if (iztype is ZClassType)
            {
                UseZClassList.Add(iztype as ZClassType);
            }
            else
            {
                throw new CCException();
            }
        }

        public ZMethodInfo[] SearchUseMethod(ZCallDesc calldesc)
        {
            List<ZMethodInfo> list = new List<ZMethodInfo>();
            foreach (ZClassType zclass in UseZClassList)
            {
                //if (zclass.IsStatic)
                {
                    var zitem = zclass.SearchZMethod(calldesc);
                    if (zitem != null && zitem.Length > 0)
                    {
                        list.AddRange(zitem);
                    }
                }
            }
            return list.ToArray();
        }

        public ZMemberInfo SearchUseZMember(string name)
        {
            foreach (ZClassType zclass in this.UseZClassList)
            {
                //if (zclass.IsStatic)
                {
                    var zitem = zclass.SearchZMember(name);
                    if (zitem != null)
                    {
                        return zitem;
                    }
                }
            }
            return null;
        }

        public bool IsUseProperty(string name)
        {
            foreach (ZClassType zclass in UseZClassList)
            {
                //if (zclass.IsStatic)
                {
                    if (zclass.SearchZMember(name) != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
