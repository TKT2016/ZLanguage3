using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using ZCompileCore;
using ZCompileCore.Engines;
using ZCompileCore.Reports;
using ZCompileKit;
using ZCompileKit.Infoes;

namespace ZCompiler
{
    public class ProjectCompiler
    {
        ZProjFileParser projFileParser = new ZProjFileParser();

        public ProjectCompileResult Compile(FileInfo projectFileInfo)
        {
            ZProjectModel projectModel = ReadModel(projectFileInfo);
            ZProjectEngine builder2 = new ZProjectEngine();
            ProjectCompileResult result = builder2.Compile(projectModel);
            return result;
        }

        private ZProjectModel ReadModel(FileInfo projectFileInfo)
        {
            string[] lines = File.ReadAllLines(projectFileInfo.FullName);
            ZProjectModel projectModel = projFileParser.ParseProjectFile(lines, projectFileInfo.Directory.FullName);
            projectModel.AddRefPackage("Z语言系统");
            projectModel.AddRefPackage("Z标准包");

            return projectModel;
        }

    }
}
