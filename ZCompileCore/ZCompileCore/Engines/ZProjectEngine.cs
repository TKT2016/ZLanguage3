using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.AST;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Reports;
using ZCompileKit.Tools;
using ZLangRT;
using ZCompileDesc.Descriptions;
using ZCompileKit;
using ZCompileDesc.ZTypes;

namespace ZCompileCore.Engines
{
    public class ZProjectEngine
    {
        ContextProject projectContext;

        ZProjectModel projectModel;

        List<FileEnum> enumeFiles;
        List<FileDim> dimFiles;
        List<FileClass> classFiles;

        public ZProjectEngine()
        {
            
        }

        public ProjectCompileResult Compile(ZProjectModel zCompileProjectModel)
        {
            this.projectModel = zCompileProjectModel;
            projectContext = new ContextProject(projectModel);
            LoadProjectRef();
            CompileUtil.GenerateBinary(projectContext);

            ParseFiles();

            CompileEnum();
            AnalyImportUse();
            AnalyDimAndClassName();
            EmitDimAndClassName();
 
            CompileFileDim();
            CompileClass();

            SetEntry();
            if (zCompileProjectModel.NeedSave)
            {
                SaveBinary();
            }

            return projectContext.CompileResult;
        }

        private void AnalyImportUse()
        {
            foreach (FileDim dimFile in dimFiles)
            {
                dimFile.AnalyImport();
                dimFile.AnalyUse();
            }

            foreach (FileClass classFile in classFiles)
            {
                classFile.AnalyImport();
                classFile.AnalyUse();
            }
        }

        private void CompileClass()
        {
            AnalyClassMemberName();
            EmitClassMemberName();
            AnalyClassMemberBody();
            EmitClassMemberBody();
            CreateClassZType();
        }

        private void CreateClassZType()
        {
            foreach (FileClass classFile in classFiles)
            {
                classFile.CreateZType();
            }
        }

        private void CompileFileDim()
        {
            foreach (FileDim dimFile in dimFiles)
            {
                dimFile.Compile();
            }

            foreach (FileClass classFile in classFiles)
            {
                classFile.CompileDim();
            }
        }

        private void EmitDimAndClassName()
        {
            foreach (FileDim dimFile in dimFiles)
            {
                dimFile.EmitTypeName();
            }
            foreach (FileClass classFile in classFiles)
            {
                classFile.EmitTypeName();
            }
        }

        private void AnalyDimAndClassName()
        {
            foreach (FileDim dimFile in dimFiles)
            {
                dimFile.AnalyTypeName();
            }
            foreach (FileClass classFile in classFiles)
            {
                classFile.AnalyTypeName();
            }
        }

        private void AnalyClassImportUser()
        {
            foreach (FileClass classFile in classFiles)
            {
                classFile.AnalyImport();
                classFile.AnalyUse();
            }
        }

        private void ParseFiles()
        {
            enumeFiles = new List<FileEnum>();
            dimFiles = new List<FileDim>();
            classFiles = new List<FileClass>();
            ZFileEngine parser = new ZFileEngine(projectContext);
            foreach (var item in projectModel.SouceFileList)
            {
                FileType fileType = parser.Parse(item);
                if(fileType!=null)
                {
                    if(fileType is FileEnum)
                    {
                        enumeFiles.Add(fileType as FileEnum);
                    }
                    else if (fileType is FileDim)
                    {
                        dimFiles.Add(fileType as FileDim);
                    }
                    else if (fileType is FileClass)
                    {
                        classFiles.Add(fileType as FileClass);
                    }
                    else
                    {
                        throw new CompileCoreException();
                    }
                }
            }
        }

        private void CompileEnum()
        {
            foreach (FileEnum enumFile in enumeFiles)
            {
                enumFile.Compile();
            }
        }

        private void SaveBinary()
        {
            if (!this.projectContext.CompileResult.HasError())
            {
                string binFileName = projectContext.GetBinaryNameEx();
                projectContext.EmitContext.AssemblyBuilder.Save(binFileName);
                CompileUtil.MoveBinary(projectContext);
                CompileUtil.DeletePDB(projectContext);
                string exBinFileName = projectContext.GetBinaryNameEx();
                string toFileFullPath = Path.Combine(projectContext.ProjectModel.BinarySaveDirectoryInfo.FullName, exBinFileName);
                projectContext.CompileResult.BinaryFilePath = toFileFullPath;
            }
        }

        private void SetEntry()
        {
            if (projectContext.ProjectModel.BinaryFileKind != PEFileKinds.Dll && !string.IsNullOrEmpty(projectContext.ProjectModel.EntryClassName))
            {
                var entryClassName = projectContext.ProjectModel.EntryClassName;
                projectContext.CompileResult.EntrtyZType = projectContext.CompileResult.GetCompiledType(entryClassName) as ZType;
                if (projectContext.CompileResult.EntrtyZType == null)
                {
                    this.projectContext.Errorf(0, 0, "入口类型'{0}'不存在或编译失败", entryClassName);
                    return;
                }
                Type type = projectContext.CompileResult.EntrtyZType.SharpType;
                MethodInfo main = type.GetMethod("启动");
                if (main == null)
                {
                    this.projectContext.Errorf(0, 0, "入口类型'{0}'不存在'启动'过程", entryClassName);
                }
                else if (!main.IsStatic)
                {
                    this.projectContext.Errorf(0, 0, "入口类型'{0}'不是唯一类型，不能作为启动入口", entryClassName);
                }
                projectContext.EmitContext.AssemblyBuilder.SetEntryPoint(main, projectContext.ProjectModel.BinaryFileKind);

            }
        }

        private void LoadProjectRef()
        {
            if (projectModel.RefPackageList != null && projectModel.RefPackageList.Count > 0)
            {
                foreach (var packageName in projectModel.RefPackageList)
                {
                    projectContext.AddPackage(packageName);
                }
            }

            if (projectModel.RefDllList != null && projectModel.RefDllList.Count > 0)
            {
                foreach (var dll in projectModel.RefDllList)
                {
                    try
                    {
                        Assembly asm = Assembly.LoadFile(dll.FullName);
                        projectContext.AddAssembly(asm);
                    }
                    catch (Exception ex)
                    {
                        this.projectContext.Errorf(0,0,"加载DLL文件" + dll.Name + "错误:" + ex.Message);
                    }
                }
            }
        }

        private void AnalyClassMemberName()
        {
            foreach (FileClass classFile in classFiles)
            {
                classFile.AnalyClassMemberName();
            }
        }

        private void EmitClassMemberName()
        {
            foreach (FileClass classFile in classFiles)
            {
                classFile.EmitClassMemberName();
            }
        }

        private void AnalyClassMemberBody()
        {
            foreach (FileClass classFile in classFiles)
            {
                //classFile.AnalyPropertiesBody();
                classFile.AnalyProcBody();
            }
        }

        private void EmitClassMemberBody()
        {
            foreach (FileClass classFile in classFiles)
            {
                classFile.EmitPropertiesBody();
                classFile.EmitProcBody();
            }
        }
    }
}
