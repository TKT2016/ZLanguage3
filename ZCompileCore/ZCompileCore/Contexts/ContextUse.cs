using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Symbols;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;
using ZCompileDesc.Words;
using ZCompileDesc.ZMembers;
using ZCompileDesc.ZTypes;
using ZCompileDesc;

namespace ZCompileCore.Contexts
{
    public class ContextUse
    {
        public ContextFile FileContext { get; private set; }
        public List<ZClassType> UseZClassList { get; private set; }
        public List<ZEnumType> UseZEnumList { get; private set; }
        public List<ZDimType> UseZDimList { get; private set; }
        public UseSymbolTable SymbolTable { get; private set; }
        public ContextUse()
        {
            UseZClassList = new List<ZClassType>();
            UseZEnumList = new List<ZEnumType>();
            UseZDimList = new List<ZDimType>();
            //_UsedEnumWordDict = new WordDictionary();
            SymbolTable = new UseSymbolTable("Use", null, UseZClassList, UseZEnumList);
        }

        WordDictionary _UsedEnumWordDict;
        public WordDictionary GetUseEnumWords()
        {
            if (_UsedEnumWordDict == null)
            {
                _UsedEnumWordDict = new WordDictionary("使用枚举表");
                foreach (ZEnumType zdim in UseZEnumList)
                {
                    AddEnumWord(zdim, _UsedEnumWordDict);
                }
            }
            return _UsedEnumWordDict;
        }

        private void AddEnumWord(ZEnumType zenum, WordDictionary wordDictionary)
        {
            //ZEnumType zenum = descType as ZEnumType;
            //UseManageContext.UseZEnumList.Add(zenum);
            ZEnumItemInfo[] values = zenum.EnumElements;
            foreach (var field in values)
            {
                foreach (var zname in field.ZNames)
                {
                    WordInfo info = new WordInfo(zname , WordKind.EnumElement);
                    wordDictionary.Add(info);
                }
            }
        }

        WordDictionary _dimWords;
        public WordDictionary GetUseDimWords()
        {
            if(_dimWords==null)
            {
                _dimWords = new WordDictionary ("使用成员表");
                foreach (ZDimType zdim in UseZDimList)
                {
                    AddDimWord(zdim, _dimWords);
                }
            }
            return _dimWords;
        }

        private void AddDimWord(ZDimType zdim, WordDictionary wordDictionary)
        {
            Dictionary<string, string> dims = zdim.Dims;
            foreach (string dimName in dims.Keys)
            {
                string dimTypeName = dims[dimName];
                IZDescType[] ztypes = ZTypeManager.GetByMarkName(dimTypeName) ;
                if (ztypes.Length > 0)
                {
                    ZType ztype = ztypes[0] as ZType;
                    WordInfo word = new WordInfo(dimName, WordKind.DimName,ztype);
                    wordDictionary.Add(word);
                }
            }
        }

        public void SetFileContext(ContextFile fileContext)
        {
            this.FileContext = fileContext;
        }

        public ZMethodInfo[] SearchProc(ZCallDesc procDesc)
        {
            List<ZMethodInfo> data = new List<ZMethodInfo>();
            foreach (var zclass in UseZClassList)
            {
                var zmethods = zclass.SearchZMethod(procDesc);
                if (zmethods !=null &&zmethods.Length > 0)
                {
                    data.AddRange(zmethods);
                }
            }
            return data.ToArray();
        }

        public bool ContainsType(string typeName)
        {
            foreach (var item in UseZClassList)
            {
                if (item.ZName == typeName)
                {
                    return true;
                }
            }
            foreach (var item in UseZEnumList)
            {
                if (item.ZName == typeName)
                {
                    return true;
                }
            }
            foreach (var item in UseZDimList)
            {
                if (item.ZName == typeName)
                {
                    return true;
                }
            }
            return false;
        }

    }

}
