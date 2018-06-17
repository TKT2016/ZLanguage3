using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZCompileCore.Reports;
using ZCompiler;
using ZDev.UI.Forms;
using ZLangRT;
using ZLangRT.Utils;

namespace ZDev.UI.Compilers
{
    public class ZDevCompiler
    {
        public const string ZYYExt = ".zyy";
        public const string ZXMExt = ".zxm";

        private FileInfo srcFileInfo;
        public ProjectCompileResult CompileResult { get; private set;}

        public ZDevCompiler(FileInfo zlogoFileInfo)
        {
            srcFileInfo = zlogoFileInfo;
        }

        ProjectCompileResult CompileFile(string srcFile)
        {
            CompileMessageCollection MessageCollection = new CompileMessageCollection();
            FileCompiler compiler = new FileCompiler();
            ProjectCompileResult result = compiler.Compile(srcFile, MessageCollection);
            return result;
        }

        ProjectCompileResult CompileProject(string srcFile)
        {
            CompileMessageCollection messageCollection = new CompileMessageCollection();
            //FileInfo srcFileInfo = new FileInfo(srcFile);
            //ProjectCompiler compiler = new ProjectCompiler();
            //ProjectCompileResult result = compiler.Compile(srcFileInfo, MessageCollection);
            //return result;

            ProjectFileCompiler compiler = new ProjectFileCompiler(srcFile, messageCollection);
            ProjectCompileResult result = compiler.Compile();
            return result;
        }

        public ProjectCompileResult Compile()
        {
            var SrcFile = this.srcFileInfo.FullName;
            bool IsCompileProject = SrcFile.EndsWith(ZDevCompiler.ZYYExt, StringComparison.InvariantCultureIgnoreCase);
            
            if (IsCompileProject)
            {
                CompileResult = CompileFile(SrcFile);
            }
            else
            {
                CompileResult = CompileProject(SrcFile);
            }

            return CompileResult;
        }

        public void Run()
        {
            if (CompileResult.MessageCollection.Errors.Count==0)
            {
                RunProcess(CompileResult.BinaryFilePath);
                //RunExe(CompileResult.BinaryFilePath, CompileResult.EntrtyZType.SharpType);
            }
        }

        private static void RunExe(string exePath, Type CompiledMainType)
        {
            //string exePath = __CompileResult.BinaryFilePath;
            string exeDirectory = (new FileInfo(exePath)).DirectoryName;
            var tempEnv = Environment.CurrentDirectory;
            Environment.CurrentDirectory = exeDirectory;
            Invoker.Call(CompiledMainType, "启动");
            Environment.CurrentDirectory = tempEnv;
        }

        private void RunProcess(string exe)
        {
            string exeFile = Path.Combine(Application.StartupPath, "ZDev.RunExe.exe");
            string exeArgs = string.Format("runexe \"{0}\"", exe);
            Process runProcess = new Process();
            runProcess.StartInfo.FileName = exeFile;
            runProcess.StartInfo.Arguments = exeArgs;
            runProcess.StartInfo.UseShellExecute = true;
            runProcess.StartInfo.UseShellExecute = true;
            //CurrentProcess.Exited += new EventHandler(CurrentProcess_Exited);
            runProcess.EnableRaisingEvents = true;
            runProcess.Start();
        }

     
    }
}
