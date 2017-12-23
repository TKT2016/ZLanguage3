using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;
using ZCompileDesc;

namespace ZCompileCore.Contexts
{
    public class ContextUse
    {
        //public ContextFile FileContext { get; private set; }
        public List<ZLClassInfo> UseZClassList { get; private set; }
        public List<ZLEnumInfo> UseZEnumList { get; private set; }
        public List<ZLDimInfo> UseZDimList { get; private set; }
        //public UseSymbolTable SymbolTable { get; private set; }
        public ContextUse()
        {
            UseZClassList = new List<ZLClassInfo>();
            UseZEnumList = new List<ZLEnumInfo>();
            UseZDimList = new List<ZLDimInfo>();
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

        public void AddUseType(ZLType iztype)
        {
            string key = iztype.ZTypeName;
            useTypes.Remove(key);
            useTypes.Add(iztype.ZTypeName, iztype);
        }

        public void Add(ZLType iztype)
        {
            if(iztype is ZLEnumInfo)
            {
                UseZEnumList.Add(iztype as ZLEnumInfo);
            }
            else if (iztype is ZLDimInfo)
            {
                UseZDimList.Add(iztype as ZLDimInfo);
            }
            else if (iztype is ZLClassInfo)
            {
                UseZClassList.Add(iztype as ZLClassInfo);
            }
            else
            {
                throw new CCException();
            }
        }

        public ZLMethodInfo[] SearchUseMethod(ZMethodCall calldesc)
        {
            List<ZLMethodInfo> list = new List<ZLMethodInfo>();
            foreach (ZLClassInfo zclass in UseZClassList)
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

        public ZLPropertyInfo SearchUseZProperty(string name)
        {
            foreach (ZLClassInfo zclass in this.UseZClassList)
            {
                //if (zclass.IsStatic)
                {
                    var zitem = zclass.SearchProperty(name);
                    if (zitem != null)
                    {
                        return zitem;
                    }
                }
            }
            return null;
        }

        public ZLFieldInfo SearchUseZField(string name)
        {
            foreach (ZLClassInfo zclass in this.UseZClassList)
            {
                //if (zclass.IsStatic)
                {
                    var zitem = zclass.SearchField(name);
                    if (zitem != null)
                    {
                        return zitem;
                    }
                }
            }
            return null;
        }

        public bool IsUsedProperty(string name)
        {
            foreach (ZLClassInfo zclass in UseZClassList)
            {
                //if (zclass.IsStatic)
                {
                    if (zclass.SearchProperty(name) != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool IsUsedField(string name)
        {
            foreach (ZLClassInfo zclass in UseZClassList)
            {
                //if (zclass.IsStatic)
                {
                    if (zclass.SearchField(name) != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
