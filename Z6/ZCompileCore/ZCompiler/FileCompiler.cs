using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using ZCompileCore.Engines;
using ZCompileCore.Reports;
using ZCompileCore.SourceModels;
using ZLangRT;

namespace ZCompiler
{
    public class FileCompiler
    {
        private SourceProjectModel projectModel;
        private FileInfo srcFileInfo;
        private Action<SourceProjectModel, string> InitProjectAct;

        public ProjectCompileResult CompileResult { get; private set;}
        CompileMessageCollection MessageCollection;

        public FileCompiler()
        {
            InitProjectAct = InitPorjectModel;
        }

        public FileCompiler(Action<SourceProjectModel,string> initProjectAct)
        {
            InitProjectAct = initProjectAct;
        }

        public ProjectCompileResult Compile(string srcPath, CompileMessageCollection messageCollection)
        {
            MessageCollection = messageCollection;
            projectModel = new SourceProjectModel();
            InitFile(srcPath);
            if(this.InitProjectAct!=null)
            {
                InitProjectAct(projectModel, srcPath);
            }
            ZProjectEngine builder2 = new ZProjectEngine(MessageCollection, projectModel);
            ProjectCompileResult result = builder2.Compile();
            result.ProjectModel = projectModel;
            CompileResult= result;
            return result;
        }

        private void InitFile(string srcPath)
        {
            if(File.Exists(srcPath)==false)
            {
                throw new FileNotFoundException("源文件‘" + srcPath + "’不存在");
            }
            srcFileInfo = new FileInfo(srcPath);         
            projectModel.ProjectFilePath = srcPath;
            projectModel.ProjectRootDirectoryInfo = srcFileInfo.Directory;     
            projectModel.BinarySaveDirectoryInfo = srcFileInfo.Directory;       
            projectModel.EntryClassName = Path.GetFileNameWithoutExtension(srcFileInfo.FullName);
            projectModel.BinaryFileNameNoEx = Path.GetFileNameWithoutExtension(srcFileInfo.FullName);
            projectModel.NeedSave = true;
            
            projectModel.AddRefPackage("Z语言系统");

        }

        private void InitPorjectModel(SourceProjectModel projectModel, string srcPath)
        {
            projectModel.AddRefPackage("Z标准包");
            projectModel.AddRefPackage("ZExcel开发包");
            projectModel.ProjectPackageName = "ZLangSingleFile";
            projectModel.BinaryFileKind = PEFileKinds.ConsoleApplication;
            projectModel.RefDllList = GetRefDllList(srcPath);

            string sourceCode = File.ReadAllText(srcPath);
            SourceFileModel fileModel = new SourceFileModel(srcPath, srcPath, projectModel.EntryClassName,
                projectModel.ProjectPackageName, projectModel.EntryClassName, sourceCode, 1);
            projectModel.AddFile(fileModel);
        }

        private List<FileInfo> GetRefDllList(string srcPath)
        {    
            List<FileInfo> list = new List<FileInfo> ();
            if (File.Exists(srcPath) == false) return list;
            FileInfo fi = new FileInfo(srcPath);
            string folderPath = fi.DirectoryName;
            string zlibpath = Path.Combine(folderPath, ZCConst.LibFolderName);
            if (Directory.Exists(zlibpath) == false) return list;

            DirectoryInfo folder = new DirectoryInfo(zlibpath);
            foreach (FileInfo file in folder.GetFiles("*.dll"))
            {
                if(IsZyyLib(file) )
                {
                    list.Add(file);
                }
            }

            foreach (FileInfo file in folder.GetFiles("*.exe"))
            {
                if (IsZyyLib(file))
                {
                    list.Add(file);
                }
            }
            return list;
        }

        private bool IsZyyLib(FileInfo file)
        {
            try
            {
                Assembly assembly = Assembly.LoadFile(file.FullName);
                Attribute attribute = assembly.GetCustomAttribute(typeof(AssemblyTrademarkAttribute));
                if (attribute == null) return false;
                AssemblyTrademarkAttribute trademarkAttribute = (AssemblyTrademarkAttribute)attribute;
                
                if(! string.IsNullOrWhiteSpace( trademarkAttribute.Trademark))
                {
                    return trademarkAttribute.Trademark == ZCConst.Trademark;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }
    }
}
