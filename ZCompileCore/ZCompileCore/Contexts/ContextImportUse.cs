using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileDesc.Descriptions;

using ZNLP;

namespace ZCompileCore.Contexts
{
    public class ContextImportUse
    {
        ContextFile FileContext;
        ContextImportTypes ContextFileManager;
        ContextUse useContext;

        public ContextImportUse(ContextFile fileContext)
        {
            FileContext = fileContext;
            ContextFileManager = new ContextImportTypes(fileContext);
            useContext = new ContextUse();
        }

        Dictionary<string, string> packages = new Dictionary<string, string>();
        public void AddImportPackageName(string packageName)
        {
            packages.Add(packageName, packageName);
        }

        public bool ContainsImportPackageName(string packageName)
        {
            return packages.ContainsKey(packageName);
        }

        public void AddImportType(ZLType itype)
        {
            if(itype is ZLEnumInfo)
            {
                ContextFileManager.Import((ZLEnumInfo)itype);
            }
            else
            {
                ContextFileManager.Import((ZLClassInfo)itype);
            }   
        }

        public void AddDimType(ZLDimInfo itype)
        {
            ContextFileManager.Import(itype);
        }

        public void ImportCompilingName(ZCClassInfo classCompilingType)
        {
            ContextFileManager.ImportZName(classCompilingType);
        }

        public void ImportCompilingBody(ZCClassInfo classCompilingType)
        {
            ContextFileManager.ImportStruct(classCompilingType);
        }

        public ZLType[] SearchImportIZType_WithUse(string name)
        {
            return ContextFileManager.SearchZLTypesByZClassName(name);
        }

        public object[] SearchByTypeName(string typeName)
        {
            List<object> list = new List<object>();
            list.AddRange(ContextFileManager.SearchZLTypesByZClassName(typeName));
            list.AddRange(ContextFileManager.SearchZDimsByZClassName(typeName));
            return list.ToArray();
        }

        /// <summary>
        /// 根据类型名称搜索导入的类
        /// </summary>
        public ZType[] SearchImportType(string typeName)
        {
            return ContextFileManager.SearchByClassNameOrDimItem(typeName);
        }
       
        public void AddUseZTypeName(string ztypeName)
        {
            useContext.Add(ztypeName);
        }

        public bool ContainsUserZTypeName(string ztypeName)
        {
            return useContext.Contains(ztypeName);
        }

        public void AddUseType(ZLType iztype)
        {
            useContext.Add(iztype);
        }

        public UserWordsSegementer GetFileSegementer()
        {
            return this.ContextFileManager.FileSegementer;
        }

        public UserWordsSegementer GetArgSegementer()
        {
            return this.ContextFileManager.ArgSegementer;
        }

        public ZLMethodInfo[] SearchUseMethod(ZMethodCall calldesc)
        {
            return useContext.SearchUseMethod(calldesc);
        }

        public ZLPropertyInfo SearchUseZProperty(string name)
        {
            return useContext.SearchUseZProperty(name);
        }

        public ZLFieldInfo SearchUseZField(string name)
        {
            return useContext.SearchUseZField(name);
        }

        public bool IsUsedProperty(string name)
        {
            return useContext.IsUsedProperty(name);
        }

        public bool IsUsedField(string name)
        {
            return useContext.IsUsedField(name);
        }

        /// <summary>
        /// 根据类名或者声明项查找类型
        /// </summary>
        public ZType[] SearchZTypesByClassNameOrDimItem(string name)
        {
            return this.ContextFileManager.SearchByClassNameOrDimItem(name);
        }

        public ZLEnumItemInfo[] SearchUsedZEnumItems(string name)
        {
            return ContextFileManager.SearchZEnumItems(name);
        }

        public bool IsUseEnumItem(string name)
        {
            ZLEnumItemInfo[] cu = this.SearchUsedZEnumItems(name);
            return cu.Length > 0;
        }

        public bool IsImportClassName(string name)
        {
            return this.ContextFileManager.IsImportClassName(name);
        }

        Dictionary<string, string> _dims = new Dictionary<string, string>();

        public bool ContainsDim(string name)
        {
            return _dims.ContainsKey(name);
        }

        public void AddDim(string name,string type)
        {
            _dims.Add(name, type);
        }
    }
}
