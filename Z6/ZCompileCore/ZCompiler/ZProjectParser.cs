using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore;
using ZCompileCore.Engines;
using ZCompileCore.Reports;
using ZCompileCore.SourceModels;
//using ZCompileKit.Infoes;

namespace ZCompiler
{
    public class ZProjectParser
    {
        public SourceProjectModel ParseProjectFile(CompileMessageCollection messageCollection, string[] lines,
            string folderPath,string projectFilePath)//, ZCompileFileInfo zfileInfo)
        {
            SourceProjectModel projectModel = new SourceProjectModel();
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
                    //string srcPath = Path.Combine(folderPath, src);

                    SourceFileModel classModel = readFileModel(folderPath, src, projectModel.ProjectPackageName);// new SourceFileModel(new ZCompileFileInfo(false, srcPath, null, null));
                    projectModel.AddFile(classModel);
                }
                else if (code.StartsWith("设置启动:"))
                {
                    string name = code.Substring(5);
                    projectModel.EntryClassName = name;
                }
                else if (code.StartsWith("保存:"))
                {
                    string name = code.Substring(3);
                    projectModel.BinaryFileNameNoEx = name;
                }
                else
                {
                    messageCollection.AddError(
                    new CompileMessage(new CompileMessageSrcKey(projectFilePath), i + 1, 0, "项目指令'" + code + "'无效"));
                }
            }
            return projectModel;
        }

        private SourceFileModel readFileModel(string sourcFolder,string sourceName,string packageName)
        {
            string srcfileFullPath = Path.Combine(sourcFolder,sourceName);
            string className = Path.GetFileNameWithoutExtension(srcfileFullPath);
            string sourceCode  =File.ReadAllText(srcfileFullPath);
            SourceFileModel fileModel = new SourceFileModel(sourceName, srcfileFullPath,className, packageName, className, sourceCode, 1);
            return fileModel;
        }

    }
}
