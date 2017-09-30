using System;
using System.IO;
using System.Reflection.Emit;
using ZCompileCore.Engines;
using ZCompileCore.Reports;
using ZCompileKit.Infoes;
using ZLangRT;

namespace ZCompiler
{
    public class FileCompiler
    {
        private ZProjectModel projectModel;

        private FileInfo srcFileInfo;

        public ProjectCompileResult CompileResult { get; private set;}

        public FileCompiler( )
        {

        }

        public ProjectCompileResult Compile(string srcPath)
        {
            InitFile(srcPath);
            ZProjectEngine builder2 = new ZProjectEngine();
            ProjectCompileResult result = builder2.Compile(projectModel);
            CompileResult= result;
            return result;
        }

        private void InitFile(string srcPath)
        {
            srcFileInfo = new FileInfo(srcPath);
            projectModel = new ZProjectModel();
            projectModel.ProjectFileInfo = new ZCompileFileInfo(true, srcPath, null, null);
            projectModel.ProjectRootDirectoryInfo = srcFileInfo.Directory;
            projectModel.BinaryFileKind = PEFileKinds.ConsoleApplication;
            projectModel.BinarySaveDirectoryInfo = srcFileInfo.Directory;
            projectModel.ProjectPackageName = "ZLangSingleFile";
            projectModel.EntryClassName = Path.GetFileNameWithoutExtension(srcFileInfo.FullName);
            projectModel.BinaryFileNameNoEx = Path.GetFileNameWithoutExtension(srcFileInfo.FullName);
            projectModel.NeedSave = true;
            
            projectModel.AddRefPackage("Z语言系统");
            projectModel.AddRefPackage("Z标准包");

            ZFileModel classModel = new ZFileModel(new ZCompileFileInfo(false, srcPath, null, null));
            projectModel.AddClass(classModel);
        }
    }
}
