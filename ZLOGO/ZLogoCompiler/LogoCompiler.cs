using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.Engines;
using ZCompileCore.Reports;
using ZLangRT;
using ZLangRT.Utils;
using ZLogoEngine;
using ZCompileDesc.Descriptions;
using ZLogoEngine.Turtles;
using ZCompiler;
using ZCompileCore.SourceModels;

namespace ZLogoCompiler
{
    public class LogoCompiler
    {
        public const string ZLogoExt = ".zlogo";

        public const string PreCode = @"
                导入包:Z语言系统,ZLogoEngine/开发包
                导入类:常用颜色
                属于:海龟精灵

                ";

        private void InitPorjectModel(SourceProjectModel projectModel,string srcPath)
        {
            //var srcFileInfo =  new FileInfo(srcPath);
            //var projectModel = new SourceProjectModel();
            string srcFileTypeName = Path.GetFileNameWithoutExtension(srcPath);

            //projectModel.ProjectRootDirectoryInfo = srcFileInfo.Directory;
            projectModel.BinaryFileKind = PEFileKinds.Dll;
            //projectModel.BinarySaveDirectoryInfo = srcFileInfo.Directory;
            projectModel.ProjectPackageName = "ZLOGOEmit";
            projectModel.EntryClassName = srcFileTypeName;
            projectModel.BinaryFileNameNoEx = srcFileTypeName;
            //projectModel.ProjectFileInfo = new ZCompileFileInfo( true, srcPath,null,null);
            //projectModel.AddRefPackage("Z语言系统");
            projectModel.AddRefPackage("ZLogoEngine");
            projectModel.AddRefDll(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ZLogoEngine.dll")));
            projectModel.NeedSave = true;
            //ZFileModel classModel = new ZFileModel(new ZCompileFileInfo(false, srcPath, PreCode,null));      
            //projectModel.AddClass(classModel);
            //return projectModel;

            string sourceCode = File.ReadAllText(srcPath);
            SourceFileModel fileModel = new SourceFileModel(srcPath, srcPath, projectModel.EntryClassName,
                projectModel.ProjectPackageName, projectModel.EntryClassName, sourceCode, 1);
            fileModel.PreSourceCode = PreCode;

            projectModel.AddFile(fileModel);
        }

        private CompileMessageCollection MessageCollection = new CompileMessageCollection();
        public ProjectCompileResult Compile(string srcFile)
        {
            MessageCollection.Clear();

           //CompileMessageCollection MessageCollection = new CompileMessageCollection();
            FileCompiler compiler = new FileCompiler(this.InitPorjectModel);
            ProjectCompileResult result = compiler.Compile(srcFile, MessageCollection);
            return result;
            //ZProjectModel projectModel = Init(filePath);
            //ZProjectEngine builder = new ZProjectEngine(MessageCollection, projectModel);
            //ProjectCompileResult result = builder.Compile();
            //return result;
        }

        public bool CheckRunZLogo(ProjectCompileResult result)
        {
            if (result.MessageCollection.Errors.Count > 0) return false;
            //if (result.CompiledTypes.Count == 0) return false;
            ZLClassInfo zclass = result.CompiledTypes.ZClasses[0] as ZLClassInfo;
            MethodInfo method = zclass.SharpType.GetMethod("RunZLogo");
            return (method != null);
        }

        public void Run(ProjectCompileResult result)
        {
            if (result.MessageCollection.Errors.Count == 0)
            {
                foreach(var item in result.CompiledTypes.ZClasses)
                {
                    if (item is ZLType)
                    {
                        ZLType zclass = item as ZLType;
                        Type type = zclass.SharpType;
                        using (TurtleForm turtleForm = new  TurtleForm())
                        {
                            TurtleSprite turtleSprite = ReflectionUtil.NewInstance(type) as TurtleSprite;
                            turtleSprite.SetForm(turtleForm);
                            turtleForm.Turtle = turtleSprite;
                            turtleForm.Run();
                        }
                        return;
                    }
                }     
            }
        }
       
    }
}
