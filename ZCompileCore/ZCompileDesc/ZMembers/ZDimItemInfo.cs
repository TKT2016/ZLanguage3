using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZCompileDesc.Utils;
using ZCompileDesc.ZTypes;
using ZLangRT.Utils;

namespace ZCompileDesc.ZMembers
{
    public class ZDimItemInfo 
    {
        public string DimVarName { get; private set; }
        public string DimTypeName { get; private set; }
       

        public ZDimItemInfo(string dimName, string dimTypeName)
        {
            DimVarName = dimName;
            DimTypeName = dimTypeName;
        }

        public ZType[] DimTypes
        {
            get
            {
                IZDescType[] idescZtypes = ZTypeManager.GetByMarkName(DimTypeName);
                List<ZType> list = new List<ZType> ();
                foreach(var  item in idescZtypes)
                {
                    if(item is ZType)
                    {
                        list.Add((ZType) item);
                    }
                }
                return list.ToArray();
            }
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", DimVarName,DimTypeName);
        }
    }
}
