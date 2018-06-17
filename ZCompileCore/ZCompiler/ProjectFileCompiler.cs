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
using ZCompileCore.SourceModels;

namespace ZCompiler
{
    public class ProjectFileCompiler
    {
        public string ProjectFilePath { get; private set; }
        public CompileMessageCollection MessageCollection { get; private set; }

        private ZProjectParser projFileParser = new ZProjectParser();
        private ZProjectEngine builder;

        public ProjectFileCompiler(string projectFilePath, CompileMessageCollection messageCollection)
        {
            ProjectFilePath = projectFilePath;
            MessageCollection = messageCollection;
        }

        public ProjectCompileResult Compile()
        {
            if(!File.Exists(ProjectFilePath))
            {
                string projectFileKey  =ProjectFilePath;
                 MessageCollection.AddError(
                   new CompileMessage(new CompileMessageSrcKey(projectFileKey), 0, 0, "项目文件'" + projectFileKey + "'不存在"));

                ProjectCompileResult psr = new ProjectCompileResult (){ MessageCollection = MessageCollection};
                return psr;
            }
            SourceProjectModel projectModel = ReadModel(); 
            
            if (projectModel != null)
            {
                builder = new ZProjectEngine(MessageCollection, projectModel);
                var  result =  builder.Compile();
                if (result.ProjectModel==null)
                    result.ProjectModel = projectModel;
                return result;
            }
            return null;
        }

        private SourceProjectModel ReadModel()
        {
            FileInfo srcFileInfo = new FileInfo(ProjectFilePath);
            string[] lines = File.ReadAllLines(ProjectFilePath);
            SourceProjectModel projectModel = projFileParser.ParseProjectFile(MessageCollection, lines, srcFileInfo.Directory.FullName, ProjectFilePath);
            //projectModel.AddRefPackage("Z语言系统");
            projectModel.AddRefPackage("Z标准包");
            if (projectModel.ProjectRootDirectoryInfo==null)
                projectModel.ProjectRootDirectoryInfo = srcFileInfo.Directory;
            if (projectModel.BinarySaveDirectoryInfo == null)
                projectModel.BinarySaveDirectoryInfo = srcFileInfo.Directory;

            return projectModel;
        }

    }
}
