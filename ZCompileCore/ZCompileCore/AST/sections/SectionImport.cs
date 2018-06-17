using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.ASTRaws;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileDesc.Descriptions;

namespace ZCompileCore.AST
{
    public class SectionImport
    {
        private FileAST ASTFile;
        private SectionImportRaw Raw;

        public SectionImport(FileAST fileAST, SectionImportRaw sectionImportRaw)
        {
            ASTFile = fileAST;
            Raw = sectionImportRaw;
        }

        public void Analy()
        {
            foreach (ZCompileCore.ASTRaws.SectionImportRaw.PackageRaw itemPackage in this.Raw.Packages)
            {
                AnalyPackageRaw(itemPackage);
            }
        }

        private string AnalyPackageRaw(ZCompileCore.ASTRaws.SectionImportRaw.PackageRaw packageRaw)
        {
            ContextImportUse contextiu = this.ASTFile.FileContext.ImportUseContext;
            List<LexTokenText> Tokens = packageRaw.Parts;
            string PackageFullName = string.Join("/", Tokens.Select(p => p.Text));
            if (contextiu.ContainsImportPackageName(PackageFullName))
            {
                this.ASTFile.FileContext.Errorf(packageRaw.Position, "开发包'{0}'已经导入", PackageFullName);
            }
            else
            {
                contextiu.AddImportPackageName(PackageFullName);
                LoadPackageTypes(PackageFullName, packageRaw.Position);
                return PackageFullName;
            }
            return null;
        }

        private bool LoadPackageTypes(string PackageFullName, CodePosition position)
        {
            var fileContext = this.ASTFile.FileContext;
            ZPackageInfo packageDesc = fileContext.ProjectContext.SearchZPackageDesc(PackageFullName);
            if (packageDesc == null)
            {
                this.ASTFile.FileContext.Errorf(position, "不存在'{0}'开发包", PackageFullName);
                return false;
            }
            else
            {
                this.AddPackage(packageDesc);
            }
            return true;
        }

        private void AddPackage(ZPackageInfo zdesc)
        {
            ContextImportUse contextiu = this.ASTFile.FileContext.ImportUseContext;
            foreach (var item in zdesc.EnumTypes)
            {
                contextiu.AddImportType(item);
            }

            foreach (ZLDimInfo item in zdesc.DimTypes)
            {
                contextiu.AddDimType(item);
            }

            foreach (var item in zdesc.ClassTypes)
            {
                contextiu.AddImportType(item);
            }
            return;
        }
    }
}
