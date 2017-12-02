﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZLangRT.Utils;
using ZCompileDesc.Descriptions;
using ZCompileDesc.ZTypes;

namespace ZCompileCore.AST
{
    public class PackageNameAST : SectionPartFile
    {
        public List<LexToken> Tokens { get; protected set; }
        public string PackageFullName { get; protected set; }
        public ZPackageDesc ZPackage { get; protected set; }
       
        public PackageNameAST()
        {
            Tokens = new List<LexToken>();
        }

        bool isExists = false;
        public override void AnalyText()
        {
            ContextImportUse contextiu = this.FileContext.ImportUseContext;
            PackageFullName = string.Join("/", Tokens.Select(p => p.GetText()));
            if (contextiu.ContainsImportPackageName(PackageFullName))
            {
                isExists = true;
                ErrorF(this.Position, "开发包'{0}'已经导入", PackageFullName);
            }
            else
            {
                contextiu.AddImportPackageName(PackageFullName);
            }
        }

        public override void AnalyType()
        {
            if (isExists) return;
            ContextImportUse contextiu = this.FileContext.ImportUseContext;
            PackageFullName = string.Join("/", Tokens.Select(p => p.GetText()));
            LoadPackageTypes();
        }

        private bool LoadPackageTypes( )
        {
            var fileContext = this.FileContext;
            ZPackageDesc packageDesc = fileContext.ProjectContext.SearchZPackageDesc(this.PackageFullName);
            if (packageDesc == null)
            {
                ErrorF(this.Position, "不存在'{0}'开发包", PackageFullName);
                return false;
            }
            else
            {
                this.AddPackage(packageDesc);
            }
            return true;
        }

        private void AddPackage(ZPackageDesc zdesc)
        {
            ContextImportUse contextiu = this.FileContext.ImportUseContext;
            foreach (var item in zdesc.EnumTypes)
            {
                contextiu.AddImportType(item);
            }

            foreach (ZDimType item in zdesc.DimTypes)
            {
                contextiu.AddImportType(item);
            }

            foreach (var item in zdesc.ClassTypes)
            {
                contextiu.AddImportType(item);
            }
            return;
        }

        public override void AnalyBody()
        {
            
        }

        public override void EmitName()
        {
            
        }

        public override void EmitBody()
        {
            
        }

        public void SetContext(ContextFile fileContext)
        {
            this.FileContext = fileContext;
        }

        public void Add(LexToken token)
        {
            Tokens.Add(token);
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

