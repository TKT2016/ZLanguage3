using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore;
using ZCompileCore.Engines;
using ZCompileKit.Infoes;

namespace ZCompiler
{
    public class ZProjFileParser
    {
        public ZProjectModel ParseProjectFile(string[] lines, string folderPath)
        {
            ZProjectModel projectModel = new ZProjectModel();
            projectModel.NeedSave = true;
            projectModel.AddRefPackage("Z语言系统");

            for (int i = 0; i < lines.Length; i++)
            {
                string code = lines[i];
                if (string.IsNullOrEmpty(code))
                {
                    continue;
                }
                else if (code.StartsWith("//"))
                {
                    continue;
                }
                else if (code.StartsWith("包名称:"))
                {
                    string name = code.Substring(4);
                    projectModel.ProjectPackageName = name;
                    projectModel.BinaryFileNameNoEx = name;
                }
                else if (code.StartsWith("生成类型:"))
                {
                    string lx = code.Substring(5);
                    PEFileKinds fileKind = PEFileKinds.ConsoleApplication;
                    if (lx == "开发包")
                    {
                        fileKind = PEFileKinds.Dll;
                    }
                    else if (lx == "控制台程序")
                    {
                        fileKind = PEFileKinds.ConsoleApplication;
                    }
                    else if (lx == "桌面程序")
                    {
                        fileKind = PEFileKinds.WindowApplication;
                    }
                    projectModel.BinaryFileKind = fileKind;
                }
                else if (code.StartsWith("编译:"))
                {
                    string src = code.Substring(3);
                    string srcPath = Path.Combine(folderPath, src);

                    ZFileModel classModel = new ZFileModel(new ZCompileFileInfo(false, srcPath, null, null));
                    projectModel.AddClass(classModel);
                }
                else if (code.StartsWith("设置启动:"))
                {
                    string name = code.Substring(5);
                    projectModel.EntryClassName = name;
                }
                else
                {
                    throw new CompileCoreException("无法识别项目编译指令:" + code);
                }
            }
            return projectModel;
        }
    }
}
