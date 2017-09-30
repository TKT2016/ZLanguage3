using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZLangRT.Utils;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Words;
using ZCompileDesc.ZTypes;

namespace ZCompileCore.AST
{
    public class PackageNameAST : UnitBase
    {
        public List<Token> Tokens { get; protected set; }
        public string PackageFullName { get; protected set; }
        public ZPackageDesc ZPackage { get; protected set; }

        public PackageNameAST()
        {
            Tokens = new List<Token>();
        }

        public void Add(Token token)
        {
            Tokens.Add(token);
        }

        public void Analy(ContextFile fileContext)
        {
            this.FileContext = fileContext;
            PackageFullName = string.Join("/", Tokens.Select(p => p.GetText()));
            LoadPackage(fileContext);
        }

        private bool LoadPackage(ContextFile fileContext)
        {
            var ImportContext = fileContext.ImportContext;
            var DescDictionary = fileContext.ProjectContext.AssemblyDescDictionary;

            if (ImportContext.ContainsPackage(this.PackageFullName))
            {
                ErrorE(this.Position, "开发包'{0}'已经导入", PackageFullName);
                return false;
            }
            ZPackageDesc packageDesc = SearchZPackageDesc(this.PackageFullName, DescDictionary);
            if (packageDesc == null)
            {
                ErrorE(this.Position, "不存在'{0}'开发包", PackageFullName);
                return false;
            }
            else
            {
                ImportContext.AddPackage(packageDesc);
            }
            return true;
        }

        private ZPackageDesc SearchZPackageDesc(string packageName, Dictionary<Assembly, ZAssemblyDesc> dict)
        {
            foreach (ZAssemblyDesc assemblyDesc in dict.Values)
            {
                ZPackageDesc packageDesc = assemblyDesc.SearhcZPackageDesc(packageName);
                if (packageDesc != null)
                    return packageDesc;
            }
            return null;
        }
        
        public virtual CodePosition Position
        {
            get
            {
                return this.Tokens[0].Position;
            }
        }

        public override string ToString()
        {
            return string.Join("/", Tokens.Select(p => p.GetText()));
        }
    }
}

