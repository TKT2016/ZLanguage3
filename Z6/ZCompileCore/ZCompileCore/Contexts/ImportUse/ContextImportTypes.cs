using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileDesc;
using ZCompileDesc.Descriptions;

using ZCompileCore.CommonCollections;
using ZCompileNLP;

namespace ZCompileCore.Contexts
{
    public class ContextImportTypes
    {
        public UserWordsSegementer FileSegementer { get; private set; }
        public UserWordsSegementer ArgSegementer { get; private set; }
        public ContextFile FileContext { get; private set; }

        private KeyListDictionary<string, ZCClassInfo> CompilingClassDict = new KeyListDictionary<string, ZCClassInfo>();

        private KeyListDictionary<string, ZLDimInfo> ZLDimDict = new KeyListDictionary<string, ZLDimInfo>();
        private KeyListDictionary<string, ZLDimItemInfo> ZLDimItemDict = new KeyListDictionary<string, ZLDimItemInfo>();
        private KeyListDictionary<string, ZLEnumInfo> ZLEnumDict = new KeyListDictionary<string, ZLEnumInfo>();
        private KeyListDictionary<string, ZLEnumItemInfo> ZLEnumItemDict = new KeyListDictionary<string, ZLEnumItemInfo>();
        private KeyListDictionary<string, ZLClassInfo> ZLClassDict = new KeyListDictionary<string, ZLClassInfo>();

        public ContextImportTypes(ContextFile fileContext)
        {
            FileContext = fileContext;
            ArgSegementer = new UserWordsSegementer();
            FileSegementer = new UserWordsSegementer();
            AddDefaultPackage();
        }

        private void AddDefaultPackage()
        {
            foreach(var item in this.FileContext.ProjectContext.CompiledTypes.ZClasses)
            {
                Import(item);
            }

            foreach (var item in this.FileContext.ProjectContext.CompiledTypes.ZEnums)
            {
                Import(item);
            }

            foreach (var item in this.FileContext.ProjectContext.CompiledTypes.ZDims)
            {
                Import(item);
            }
        }

        #region 搜索,判断
        public bool IsImportClassName(string name)
        {
            return this.ZLClassDict.ContainsKey(name);
        }

        public ZLEnumItemInfo[] SearchZEnumItems(string name)
        {
            return ZLEnumItemDict.Get(name).ToArray();
        }

        /// <summary>
        /// 根据类名或者声明项查找类型
        /// </summary>
        public ZType[] SearchByClassNameOrDimItem(string name)
        {
            List<ZType> types = new List<ZType>();
            var nameTypes1 = CompilingClassDict.Get(name);
            types.AddRange(nameTypes1);
            var nameTypes2 = ZLEnumDict.Get(name);
            types.AddRange(nameTypes2);
            var nameTypes3 = ZLClassDict.Get(name);
            types.AddRange(nameTypes3);
            List<ZLDimItemInfo> dimtypes = ZLDimItemDict.Get(name);
            foreach (var item in dimtypes)
            {
                types.AddRange(item.DimTypes);
            }
            types.Distinct();
            return types.ToArray();
        }

        /// <summary>
        /// 从项目中和导入的类名中查找类
        /// </summary>
        public ZType[] SearchZTypeByClassNameOrDimItem(string name)
        {
            List<ZType> types = new List<ZType>();
            var type1 = this.FileContext.ProjectContext.CompiledTypes.Get(name);
            if(type1!=null)
            {
                types.Add(type1);
            }
            var nameTypes1 = CompilingClassDict.Get(name);
            types.AddRange(nameTypes1);
            var nameTypes3 = ZLClassDict.Get(name);
            types.AddRange(nameTypes3);
            List<ZLDimItemInfo> dimtypes = ZLDimItemDict.Get(name);
            foreach (var item in dimtypes)
            {
                types.AddRange(item.DimTypes);
            }
            types.Distinct();
            return types.ToArray();
        }

        /// <summary>
        /// 根据类型名称查找类型
        /// </summary>
        public ZLType[] SearchZLTypesByZClassName(string zclassName)
        {
            List<ZLType> types = new List<ZLType>();
            var nameTypes2 = ZLEnumDict.Get(zclassName);
            types.AddRange(nameTypes2);
            var nameTypes3 = ZLClassDict.Get(zclassName);
            types.AddRange(nameTypes3);
            types.Distinct();
            return types.ToArray();
        }

        /// <summary>
        /// 根据类型名称查找类型
        /// </summary>
        public ZLDimInfo[] SearchZDimsByZClassName(string dimName)
        {
            return ZLDimDict.Get(dimName).ToArray();
        }
        #endregion

        #region 导入

        public void Import(ZLDimInfo zldim)
        {
            //AddZTypeWord(dtype);
           // ImportZDimItem(zldim);
            foreach (var dimName in zldim.Dims.Keys)
            {
                var dimType = zldim.Dims[dimName];
                ZLDimItemDict.Add(dimName, dimType);
                FileSegementer.AddWord(dimName);
                ArgSegementer.AddWord(dimName);
            }
        }

        public void Import(ZLEnumInfo zlenum)
        {
            string zname = zlenum.ZTypeName;
            ZLEnumDict.Add(zname, zlenum);
            FileSegementer.AddWord(zname);
            ArgSegementer.AddWord(zname);

            foreach (ZLEnumItemInfo enumItem in zlenum.EnumElements)
            {
                string[] itemNames = enumItem.GetZNames();
                foreach (var name in itemNames)
                {
                    FileSegementer.AddWord(name);
                    ZLEnumItemDict.Add(name, enumItem);
                }
            }
        }

        public void Import(ZLClassInfo zclassype)
        {
            string zname = zclassype.ZTypeName;
            ZLClassDict.Add(zname, zclassype);
            FileSegementer.AddWord(zname);
            ArgSegementer.AddWord(zname);

            foreach (ZLFieldInfo memberItem in zclassype.ZFields)
            {
                string[] itemNames = memberItem.GetZFieldZNames();
                foreach (var name in itemNames)
                {
                    //ZLPropertyDict.Add(name, memberItem);
                    FileSegementer.AddWord(name);
                }
            }

            foreach (ZLPropertyInfo memberItem in zclassype.ZPropertys)
            {
                string[] itemNames = memberItem.GetZPropertyZNames();
                foreach (var name in itemNames)
                {
                    //ZLPropertyDict.Add(name, memberItem);
                    FileSegementer.AddWord(name);
                }
            }

            foreach (ZLMethodInfo item in zclassype.ZMethods)
            {
                ImportMethod(item);
            }
        }

        private void ImportMethod(ZLMethodInfo zmethodInfo)
        {
            ZLMethodDesc[] itemNames = zmethodInfo.ZDescs;
            foreach (var desc in itemNames)
            {
                ImportMethodDesc(desc);
            }
        }

        private void ImportMethodDesc(ZLMethodDesc desc)
        {
            string[] strparts = desc.GetTextParts();

            foreach (var item in strparts)
            {
                //if (item.Length > 1) //长度为1的也要添加
                {
                    FileSegementer.AddWord(item);
                    AddProcNamePart(item, desc);
                }
            }
        }

        private Dictionary<string, List<ZLMethodDesc>> ProcNamePartDict = new Dictionary<string, List<ZLMethodDesc>>();
        private void AddProcNamePart(string namePart, ZLMethodDesc methodDesc)
        {
            List<ZLMethodDesc> list = null;
            if(ProcNamePartDict.ContainsKey(namePart))
            {
                list = ProcNamePartDict[namePart];
            }
            else
            {
                list = new List<ZLMethodDesc>();
                ProcNamePartDict.Add(namePart,list);
            }
            list.Add(methodDesc);
        }

        public bool IsProcNamePart(string namePart)
        {
            return ProcNamePartDict.ContainsKey(namePart);
        }

        public void ImportZCompiling(ZCClassInfo zclassype)
        {
            string zname = zclassype.ZTypeName;
            CompilingClassDict.Add(zname, zclassype);
            FileSegementer.AddWord(zname);
            ArgSegementer.AddWord(zname);
        }

        public void BuildZCompiling()
        {
            foreach (ZCClassInfo zclass in CompilingClassDict.ValuesToList())
            {
                this.ImportStruct(zclass);
            }
        }

        public void ImportStruct(ZCClassInfo zclassype)
        {
            /* Z语言程序生成的库没有field,所以不需要导入field */
            foreach (ZCPropertyInfo memberItem in zclassype.ZPropertys)
            {
                string[] itemNames = memberItem.GetZPropertyZNames();
                foreach (var name in itemNames)
                {
                    //ZLPropertyDict.Add(name, memberItem);
                    FileSegementer.AddWord(name);
                }
            }

            foreach (ZCMethodInfo item in zclassype.ZMethods)
            {
                ImportZMethod(item);
            }
        }

        private void ImportZMethod(ZCMethodInfo zmethodInfo)
        {
            ImportZDesc(zmethodInfo.ZMethodDesc);
        }

        private void ImportZDesc(ZCMethodDesc desc)
        {
            string[] strparts = desc.GetTextParts();

            foreach (var item in strparts)
            {
                if (item.Length > 1)
                {
                    FileSegementer.AddWord(item);
                }
            }
        }

        #endregion

        #region 当前是否是正在编译的过程名称部分
        private Dictionary<string, string> CompilingProcNamePartDict;

        private void BuildProcNamePartDict()
        {
            foreach(ZCClassInfo cclassInfo in CompilingClassDict.ValuesToList())
            {
                foreach (ZCMethodInfo cmethodInfo in cclassInfo.ZMethods)
                {
                    ZCMethodDesc cmethodDesc = cmethodInfo.ZMethodDesc;
                    foreach(var text in cmethodDesc.GetTextParts())
                    {
                        CompilingProcNamePartDict.Add(text, text);
                    }
                }
            }
        }

        public bool IsCompilingMehtodNamePart(string text)
        {
            if(CompilingProcNamePartDict==null)
            {
                CompilingProcNamePartDict = new Dictionary<string, string>();
                BuildProcNamePartDict();
            }
            return CompilingProcNamePartDict.ContainsKey(text);
        }

         #endregion
    }
}
