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
        public ZProjectModel ProjectModel { get;private set; }
        public Dictionary<Assembly, ZAssemblyDesc> AssemblyDescDictionary { get; protected set; }

        public ProjectCompileResult CompileResult { get; protected set; }
        public ContextProject(ZProjectModel projectModel)
        {
            ProjectModel = projectModel;
            AssemblyDescDictionary = new Dictionary<Assembly, ZAssemblyDesc>();
            CompileResult = new ProjectCompileResult();
        }

        public string GetBinaryNameEx()
        {
            string binFileName = this.ProjectModel.BinaryFileNameNoEx;
            if (this.ProjectModel.BinaryFileKind == PEFileKinds.Dll)
            {
                binFileName += ".dll";
            }
            else
            {
                binFileName += ".exe";
            }
            return binFileName;
        }

        public bool AddAssembly(Assembly assembly)
        {
            if (this.AssemblyDescDictionary.ContainsKey(assembly)) return false;
            ZAssemblyDesc assemblyDesc = new ZAssemblyDesc(assembly.FullName,assembly);
            this.AssemblyDescDictionary.Add(assembly, assemblyDesc);
            return true;
        }

        public void AddPackage(string packageName)
        {
            try
            {
                    Assembly asm = Assembly.Load(packageName);
                    AddAssembly(asm);
            }
            catch(FileNotFoundException)
            {
                Errorf(0,0,"开发包 ‘"+packageName+"’不存在");
            }
            catch(Exception ex)
            {
                Errorf(0, 0, "加载开发包 ‘" + packageName + "’错误:"+ex.Message);
            }
        }

        #region error

        public void Error(int line, int col, string message)
        {
            var fileName = this.ProjectModel.ProjectFileInfo;
            //CompileConsole.Error("项目" + fileName??(null) + " 第" + line + "行,第" + col + "列错误:" + message);
            CompileMessage cmsg = new CompileMessage(fileName, line, col, message);
            this.CompileResult.Errors.Add(fileName,cmsg);        
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
