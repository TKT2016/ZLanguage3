using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZCompileDesc.Utils;
using ZLangRT.Utils;

namespace ZCompileDesc.Descriptions
{
    public class ZLDimItemInfo 
    {
        public string DimVarName { get; private set; }
        public string DimTypeName { get; private set; }
       

        public ZLDimItemInfo(string dimName, string dimTypeName)
        {
            DimVarName = dimName;
            DimTypeName = dimTypeName;
        }

        public ZType[] DimTypes
        {
            get
            {
                ZLType[] idescZtypes = ZTypeManager.GetByMarkName(DimTypeName);
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
