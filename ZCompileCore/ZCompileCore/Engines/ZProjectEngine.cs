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
        public ContextProject ProjectContext { get; private set; }

        CompileMessageCollection MessageCollection;
        ProjectCompileResult result;
        ZProjectModel projectModel;

        public ZProjectEngine(CompileMessageCollection cmc, ZProjectModel zCompileProjectModel)
        {
            MessageCollection = cmc;
            this.projectModel = zCompileProjectModel;
            result = new ProjectCompileResult();
            result.MessageCollection = MessageCollection;
            ProjectContext = new ContextProject(zCompileProjectModel, MessageCollection);
        }

        public ProjectCompileResult Compile()
        {
            if (this.projectModel == null || ProjectContext.ProjectModel==null)
            {
                throw new CCException();
            }

            LoadProjectRef();
            CompileUtil.GenerateBinary(ProjectContext);
            CompileFiles();

            SetEntry();
            if (this.projectModel.NeedSave)
            {
                SaveBinary();
            }

            result.CompiledTypes.Clear();
            result.CompiledTypes.AddRange(this.ProjectContext.CompiledTypes.ToList() );

            return result;
        }

        private void CompileFiles()
        {
            ZFileEngine parser = new ZFileEngine(ProjectContext);
            foreach (var item in projectModel.SouceFileList)
            {
                FileSource fileType = parser.Parse(item);
                if (fileType != null)
                {
                    IZDescType genType = null;
                    if (fileType is FileEnum)
                    {
                        ZType zenum = (fileType as FileEnum).Compile();
                        genType = zenum;
                    }
                    else if (fileType is FileDim)
                    {
                        ZDimType zdim = (fileType as FileDim).Compile();
                        genType = zdim;
                    }
                    else if (fileType is FileClass)
                    {
                        ZClassType zdim = (fileType as FileClass).Compile();
                        genType = zdim;
                    }
                    else
                    {
                        throw new CCException();
                    }
                    if (genType != null)
                    {
                        this.ProjectContext.CompiledTypes.Add(genType);
                    }
                }
                else
                {
                    throw new CCException();
                }
            }
        }

        private void SaveBinary()
        {
            if (!this.MessageCollection.HasError())
            {
                string binFileName = ProjectContext.ProjectModel.GetBinaryNameEx();
                ProjectContext.EmitContext.AssemblyBuilder.Save(binFileName);
                CompileUtil.MoveBinary(ProjectContext);
                CompileUtil.DeletePDB(ProjectContext);
                string toFileFullPath = Path.Combine(ProjectContext.ProjectModel.BinarySaveDirectoryInfo.FullName, binFileName);
                this.result.BinaryFilePath = toFileFullPath;
            }
        }

        private void SetEntry()
        {
            if (ProjectContext.ProjectModel.BinaryFileKind != PEFileKinds.Dll && !string.IsNullOrEmpty(ProjectContext.ProjectModel.EntryClassName))
            {
                var entryClassName = ProjectContext.ProjectModel.EntryClassName;
                result.EntrtyZType = ProjectContext.CompiledTypes.Get(entryClassName) as ZType;
                if (result.EntrtyZType == null)
                {
                    this.ProjectContext.Errorf(0, 0, "入口类型'{0}'不存在或编译失败", entryClassName);
                    return;
                }
                Type type = result.EntrtyZType.SharpType;
                MethodInfo main = type.GetMethod("启动");
                if (main == null)
                {
                    this.ProjectContext.Errorf(0, 0, "入口类型'{0}'不存在'启动'过程", entryClassName);
                }
                else if (!main.IsStatic)
                {
                    this.ProjectContext.Errorf(0, 0, "入口类型'{0}'不是唯一类型，不能作为启动入口", entryClassName);
                }
                ProjectContext.EmitContext.AssemblyBuilder.SetEntryPoint(main, ProjectContext.ProjectModel.BinaryFileKind);
            }
        }

        private void LoadProjectRef()
        {
            if (projectModel.RefPackageList != null && projectModel.RefPackageList.Count > 0)
            {
                foreach (var packageName in projectModel.RefPackageList)
                {
                    ProjectContext.AddPackage(packageName);
                }
            }

            if (projectModel.RefDllList != null && projectModel.RefDllList.Count > 0)
            {
                foreach (var dll in projectModel.RefDllList)
                {
                    try
                    {
                        Assembly asm = Assembly.LoadFile(dll.FullName);
                        ProjectContext.AddAssembly(asm);
                    }
                    catch (Exception ex)
                    {
                        this.ProjectContext.Errorf(0,0,"加载DLL文件" + dll.Name + "错误:" + ex.Message);
                    }
                }
            }
        }

    }
}
