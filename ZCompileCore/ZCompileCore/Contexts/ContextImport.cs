using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZLangRT;
using ZLangRT.Utils;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Words;
using ZCompileDesc.ZTypes;
using ZCompileDesc;
using ZCompileDesc.Collections;

namespace ZCompileCore.Contexts
{
    public class ContextImport : IWordDictionary
    {
        public WordDictionary TypeNameDict { get; private set; }
        public ZPackageDescList ImportPackageDescList { get; protected set; }

        public ContextImport()
        {
            TypeNameDict = new WordDictionary("导入类表");
            ImportPackageDescList = new ZPackageDescList();
        }

        #region IWordDictionary实现
        public bool ContainsWord(string text)
        {
            return ImportPackageDescList.ContainsWord(text);
        }

        public WordInfo SearchWord(string text)
        {
            return ImportPackageDescList.SearchWord(text);
        }
        #endregion

        public bool ContainsPackage(string packageName)
        {
            return ImportPackageDescList.Contains(packageName);
        }

        public void AddPackage(ZPackageDesc zdesc)
        {
            ImportPackageDescList.Add(zdesc);
            foreach(var item in zdesc.EnumTypes)
            {
                WordInfo word = new WordInfo(item.ZName, WordKind.TypeName,item);
                TypeNameDict.Add(word);
            }

            foreach (ZDimType item in zdesc.DimTypes)
            {
                foreach(var key in item.Dims.Keys)
                {
                    var name = item.Dims[key];
                    WordInfo word = new WordInfo(item.ZName, WordKind.DimName, ContextImport.GetDataFunc);
                    TypeNameDict.Add(word);
                }
            }

            foreach (var item in zdesc.ClassTypes)
            {
                WordInfo word = new WordInfo(item.ZName, WordKind.TypeName,item);
                TypeNameDict.Add(word);
            }
        }

        private static object GetDataFunc(string name,WordKind wkind)
        {
            IZDescType[] dts= ZTypeManager.GetByMarkName(name);
            if (dts.Length == 0) return null;
            else return dts[0];
        }
    }
}
