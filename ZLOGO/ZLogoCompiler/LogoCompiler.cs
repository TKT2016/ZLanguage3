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
using ZCompileKit.Infoes;
using ZCompileDesc.ZTypes;
using ZCompileDesc.ZMembers;
using ZLogoEngine.Turtles;

namespace ZLogoCompiler
{
    public class LogoCompiler
    {
        public const string ZLogoExt = ".zlogo";

        public const string PreCode = @"
                导入:Z语言系统,ZLogoEngine/开发包
                使用:常用颜色
                海龟精灵类型:

                ";

        private ZProjectModel Init(string srcPath)
        {
            var srcFileInfo =  new FileInfo(srcPath);
            var projectModel = new ZProjectModel();
            string srcFileTypeName = Path.GetFileNameWithoutExtension(srcFileInfo.FullName);

            projectModel.ProjectRootDirectoryInfo = srcFileInfo.Directory;
            projectModel.BinaryFileKind = PEFileKinds.Dll;
            projectModel.BinarySaveDirectoryInfo = srcFileInfo.Directory;
            projectModel.ProjectPackageName = "ZLOGOEmit";
            projectModel.EntryClassName = srcFileTypeName;
            projectModel.BinaryFileNameNoEx = srcFileTypeName;
            projectModel.ProjectFileInfo = new ZCompileFileInfo( true, srcPath,null,null);
            projectModel.AddRefPackage("Z语言系统");
            projectModel.AddRefPackage("ZLogoEngine");
            projectModel.AddRefDll(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ZLogoEngine.dll")));
            projectModel.NeedSave = true;
            ZFileModel classModel = new ZFileModel(new ZCompileFileInfo(false, srcPath, PreCode,null));
           
            projectModel.AddClass(classModel);
            return projectModel;
        }

        public ProjectCompileResult Compile(string filePath)
        {
            ZProjectModel projectModel = Init(filePath);
            ZProjectEngine builder = new ZProjectEngine();
            ProjectCompileResult result = builder.Compile(projectModel);
            return result;
        }

        public bool CheckRunZLogo(ProjectCompileResult result)
        {
            if (result.CompiledTypes.Count == 0) return false;
            ZClassType zclass = result.CompiledTypes[0] as ZClassType;
            ZMethodInfo method = zclass.FindDeclaredZMethod("RunZLogo");
            return (method != null);
        }

        public void Run(ProjectCompileResult result)
        {
            if (result.CompiledTypes.Count > 0)
            {
                foreach(var item in result.CompiledTypes)
                {
                    if(item is ZClassType)
                    {
                        ZClassType zclass = item as ZClassType;
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
