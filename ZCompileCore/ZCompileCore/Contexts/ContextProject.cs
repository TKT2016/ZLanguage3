using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ZCompileKit.Collections;
using ZCompileCore.Engines;
using ZCompileCore.Lex;
using ZCompileCore.Reports;
using ZLangRT;
using ZLangRT.Utils;
using ZCompileDesc.Descriptions;
using System.IO;
using ZCompileDesc.ZTypes;

namespace ZCompileCore.Contexts
{
    public  class ContextProject
    {
        public string PackageName { get; set; }
        public ZProjectModel ProjectModel { get;private set; }
        public Dictionary<Assembly, ZAssemblyDesc> AssemblyDescDictionary { get; private set; }
        public CompiledTypeCollection CompiledTypes { get; private set; }
        public CompileMessageCollection MessageCollection { get; private set; }

        public ContextProject(ZProjectModel projectModel, CompileMessageCollection messageCollection)
        {
            ProjectModel = projectModel;
            PackageName = ProjectModel.ProjectPackageName;
            MessageCollection = messageCollection;

            AssemblyDescDictionary = new Dictionary<Assembly, ZAssemblyDesc>();
            CompiledTypes = new CompiledTypeCollection();
        }

        public bool AddAssembly(Assembly assembly)
        {
            if (this.AssemblyDescDictionary.ContainsKey(assembly)) return false;
            ZAssemblyDesc assemblyDesc = new ZAssemblyDesc(assembly.FullName,assembly);
            this.AssemblyDescDictionary.Add(assembly, assemblyDesc);
            return true;
        }

        public ZPackageDesc SearchZPackageDesc(string packageName)
        {
            var dict = this.AssemblyDescDictionary;
            foreach (ZAssemblyDesc assemblyDesc in dict.Values)
            {
                ZPackageDesc packageDesc = assemblyDesc.SearhcZPackageDesc(packageName);
                if (packageDesc != null)
                    return packageDesc;
            }
            return null;
        }

        public void AddPackage(string packageName)
        {
            try
            {
                Assembly asm = Assembly.Load(packageName);
                AddAssembly(asm);
            }
            catch (FileNotFoundException)
            {
                Errorf(0, 0, "开发包 ‘" + packageName + "’不存在");
            }
            catch (Exception ex)
            {
                Errorf(0, 0, "加载开发包 ‘" + packageName + "’错误:" + ex.Message);
            }
        }

        #region error

        public void Error(int line, int col, string message)
        {
            var fileName = this.ProjectModel.ProjectFileInfo.ZFileName;
            CompileMessage cmsg = new CompileMessage( new CompileMessageSrcKey( fileName), line, col, message);
            this.MessageCollection.AddError(cmsg);        
        }

        public void Errorf(int line, int col, string messagef, params string[] args)
        {
            string msg = string.Format(messagef, args);
            Error(line, col, msg);
        }

        public void Errorf(CodePosition position, string messagef, params string[] args)
        {
            string msg = string.Format(messagef, args);
            Error(position.Line, position.Col, msg);
        }


        #endregion

        public AssemblyEmitContext EmitContext { get; set; }

        public class AssemblyEmitContext
        {
            public AppDomain CurrentAppDomain { get;private set; }
            public AssemblyName AssemblyName { get;private set; }
            public AssemblyBuilder AssemblyBuilder { get;private set; }
            public ModuleBuilder ModuleBuilder { get;private set; }

            public AssemblyEmitContext(AppDomain appDomain, string assemblyName, string moduleName, string binFileName)
            {
                this.CurrentAppDomain = appDomain;
                this.AssemblyName = new AssemblyName(assemblyName);
                this.AssemblyBuilder = appDomain.DefineDynamicAssembly(this.AssemblyName, AssemblyBuilderAccess.RunAndSave);
                this.ModuleBuilder = this.AssemblyBuilder.DefineDynamicModule(moduleName, binFileName, true);
            }
        }

        public void CreateProjectEmitContext(AppDomain appDomain, string assemblyName, string moduleName, string binFileName)
        {
            this.EmitContext = new AssemblyEmitContext(appDomain,  assemblyName,  moduleName,  binFileName);
        }
    }
}
