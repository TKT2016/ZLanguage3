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
        ZProjectEngine builder;
        CompileMessageCollection MessageCollection;

        public ProjectCompileResult Compile(FileInfo projectFileInfo, CompileMessageCollection messageCollection)
        {
            MessageCollection = messageCollection;
            ZProjectModel projectModel = ReadModel(projectFileInfo);
            
            if (projectModel != null)
            {
                projectModel.BinarySaveDirectoryInfo = projectFileInfo.Directory;
                builder = new ZProjectEngine(MessageCollection, projectModel);
                return builder.Compile();
            }
            return null;
        }

        private ZProjectModel ReadModel(FileInfo projectFileInfo)
        {
            ZCompileFileInfo zf=null;
            if(projectFileInfo.Exists==false)
            {
                zf = new ZCompileFileInfo(false,projectFileInfo.FullName,null,null);
                MessageCollection.AddError(
                   new CompileMessage(new CompileMessageSrcKey(projectFileInfo.Name), 0, 0, "项目文件'" + projectFileInfo.Name + "'不存在"));
                return null;
            }
            string[] lines = File.ReadAllLines(projectFileInfo.FullName);
            ZProjectModel projectModel = projFileParser.ParseProjectFile(MessageCollection,
            lines, projectFileInfo.Directory.FullName,zf);
            projectModel.AddRefPackage("Z语言系统");
            projectModel.AddRefPackage("Z标准包");

            return projectModel;
        }

    }
}
