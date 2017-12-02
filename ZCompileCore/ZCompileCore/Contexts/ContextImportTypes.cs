using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileDesc;
using ZCompileDesc.Compilings;
using ZCompileDesc.Descriptions;
using ZCompileDesc.ZMembers;
using ZCompileDesc.ZTypes;
using ZCompileKit.Collections;
using ZNLP;

namespace ZCompileCore.Contexts
{
    public class ContextImportTypes
    {
        public UserWordsSegementer FileSegementer { get; protected set; }
        public UserWordsSegementer ArgSegementer { get; protected set; }

        KeyListDictionary<string, ZEnumItemInfo> ZEnumItemsDict;// { get; protected set; }
        KeyListDictionary<string, ZDimItemInfo> ZDimItemsDict;// { get; protected set; }
        KeyListDictionary<string, ZMemberInfo> ZMembersDict;//  { get; protected set; }
        KeyListDictionary<string, ZType> TypeNameLDictionary;//  { get; protected set; }
        KeyListDictionary<string, IZDescType> IZTypeNameLDictionary;

        public ContextFile FileContext { get;private set; }

        public ContextImportTypes(ContextFile fileContext)
        {
            FileContext = fileContext;
            ArgSegementer = new UserWordsSegementer();
            FileSegementer = new UserWordsSegementer();
            ZEnumItemsDict = new KeyListDictionary<string, ZEnumItemInfo>();
            ZDimItemsDict = new KeyListDictionary<string, ZDimItemInfo>();
            TypeNameLDictionary = new KeyListDictionary<string, ZType>();
            ZMembersDict = new KeyListDictionary<string, ZMemberInfo>();
            IZTypeNameLDictionary = new KeyListDictionary<string, IZDescType>();
            AddDefaultPackage();
        }

        private void AddDefaultPackage()
        {
            foreach(var item in this.FileContext.ProjectContext.CompiledTypes.ToList())
            {
                //Debug.WriteLine("AddDefaultPackage " + item.ZName);
                //if(item.ZName=="子弹类型")
                //{
                //    Console.WriteLine("子弹类型");
                //}
                Add(item);
            }
        }

        public bool IsImportClassName(string name)
        {
            return this.TypeNameLDictionary.ContainsKey(name);
        }

        public ZEnumItemInfo[] SearchZEnumItems(string name)
        {
            return ZEnumItemsDict.Get(name).ToArray();
        }

        /// <summary>
        /// 根据类名或者声明项查找类型
        /// </summary>
        public ZType[] SearchZTypesByClassNameOrDimItem(string name)
        {
            List<ZType> types = new List<ZType>();
            List<ZType> nameTypes = TypeNameLDictionary.Get(name);
            types.AddRange(nameTypes);
            List<ZDimItemInfo> dimtypes = ZDimItemsDict.Get(name);
            foreach(var item in dimtypes)
            {
                types.AddRange(item.DimTypes);
            }
            types.Distinct();
            return types.ToArray();
        }

        public IZDescType[] SearchIZTypes(string name)
        {
            return IZTypeNameLDictionary.Get(name).ToArray();
        }

        public void Add(IZDescType dtype)
        {
            AddZTypeWord(dtype as ZType);
            Import(dtype);
        }

        private void Import(IZDescType iztype)
        {
            if (iztype is ZEnumType)
            {
                Import((ZEnumType)iztype);
            }
            else if (iztype is ZDimType)
            {
                Import((ZDimType)iztype);
            }
            //else if (iztype is ZClassCompilingType)
            //{
            //    Import((ZClassCompilingType)iztype);
            //}
            else if (iztype is ZClassType)
            {
                Import((ZClassType)iztype);
            }
            else
            {
                throw new CCException();
            }
        }

        public void AddCompiling_Name(ZClassCompilingType classCompilingType)
        {
            AddZTypeWord(classCompilingType);
        }

        public void ImportCompiling_Body(ZClassCompilingType classCompilingType)
        {
            Import((ZClassCompilingType)classCompilingType);
        }

        private void AddZTypeWord(IZDescType ztype)
        {
            if (ztype == null) return;
            string zname = ztype.ZName;
           
            IZTypeNameLDictionary.Add(zname,ztype);
            FileSegementer.AddWord(zname);
            ArgSegementer.AddWord(zname);

            if(ztype is ZType)
            {
                TypeNameLDictionary.Add(zname, (ZType)ztype);
            }
        }

        private void Import(ZClassCompilingType zclassype)
        {
            foreach (ZMemberInfo memberItem in zclassype.ZMembers)
            {
                string[] itemNames = memberItem.GetZNames();
                foreach (var name in itemNames)
                {
                    ZMembersDict.Add(name, memberItem);
                    FileSegementer.AddWord(name);
                }
            }

            foreach (ZMethodInfo item in zclassype.ZMethods)
            {
                Import(item);
            }
        }

        private void Import(ZClassType zclassype)
        {
            foreach (ZMemberInfo memberItem in zclassype.ZMembers)
            {
                string[] itemNames = memberItem.GetZNames();
                foreach (var name in itemNames)
                {
                    ZMembersDict.Add(name, memberItem);
                    FileSegementer.AddWord(name);
                }
            }

            foreach (ZMethodInfo item in zclassype.ZMethods)
            {
                Import(item);
            }
        }

        private void Import(ZMethodInfo zmethodInfo)
        {
            ZMethodDesc[] itemNames = zmethodInfo.ZDesces;
            foreach (var desc in itemNames)
            {
                Import(desc);
            }
        }

        private void Import(ZMethodDesc desc)
        {
            string[] strparts = desc.GetStringParts();

            foreach (var item in strparts)
            {
                //if (item == "生成随机数")
                //{
                //    Console.WriteLine("生成随机数");
                //}
                if (item.Length > 1)
                {
                    FileSegementer.AddWord(item);
                }
            }
        }

        private void Import(ZDimType iztype)
        {
            foreach (var dimName in iztype.Dims.Keys)
            {
                var dimType = iztype.Dims[dimName];
                ZDimItemsDict.Add(dimName, dimType);
                FileSegementer.AddWord(dimName);
                ArgSegementer.AddWord(dimName);
            }
        }

        private void Import(ZEnumType zenumtype)
        {
            foreach (ZEnumItemInfo enumItem in zenumtype.EnumElements)
            {
                string[] itemNames = enumItem.GetZNames();
                foreach (var name in itemNames)
                {
                    FileSegementer.AddWord(name);
                    ZEnumItemsDict.Add(name, enumItem);
                }
            }
        }
    }
}
