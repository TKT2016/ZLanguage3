using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileDesc.Compilings;
using ZCompileDesc.Descriptions;
using ZCompileDesc.ZMembers;
using ZCompileDesc.ZTypes;
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

        public void AddImportType(IZDescType itype)
        {
            ContextFileManager.Add(itype);
        }

        public void AddCompiling_Name(ZClassCompilingType classCompilingType)
        {
            ContextFileManager.Add(classCompilingType);
        }

        public void ImportCompiling_Body(ZClassCompilingType classCompilingType)
        {
            ContextFileManager.ImportCompiling_Body(classCompilingType);
        }

        public IZDescType[] SearchImportIZType_WithUse(string name)
        {
            return ContextFileManager.SearchIZTypes(name);
        }

        /// <summary>
        /// 根据类型名称搜索导入的类
        /// </summary>
        public ZType[] SearchImportType(string typeName)
        {
            return ContextFileManager.SearchZTypesByClassNameOrDimItem(typeName);
        }
       
        public void AddUseZTypeName(string ztypeName)
        {
            useContext.Add(ztypeName);
        }

        public bool ContainsUserZTypeName(string ztypeName)
        {
            return useContext.Contains(ztypeName);
        }

        public void AddUseType(IZDescType iztype)
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

        public ZMethodInfo[] SearchUseMethod(ZCallDesc calldesc)
        {
            return useContext.SearchUseMethod(calldesc);
        }

        public ZMemberInfo SearchUseZMember(string name)
        {
            return useContext.SearchUseZMember(name);
        }

        public bool IsUseProperty(string name)
        {
            return useContext.IsUseProperty(name);
        }

        /// <summary>
        /// 根据类名或者声明项查找类型
        /// </summary>
        public ZType[] SearchZTypesByClassNameOrDimItem(string name)
        {
            return this.ContextFileManager.SearchZTypesByClassNameOrDimItem(name);
        }

        public ZEnumItemInfo[] SearchUsedZEnumItems(string name)
        {
            return ContextFileManager.SearchZEnumItems(name);
        }

        public bool IsUseEnumItem(string name)
        {
            ZEnumItemInfo[] cu = this.SearchUsedZEnumItems(name);
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
