using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZCompileCore.Reports;
using ZLangRT;

namespace ZCompiler
{
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                //args = new string[] { "Sample/test.zyy" };
                //args = new string[] { "Sample/你好.zyy" };
                //args = new string[] { "Sample/测试重复.zyy" };
                //args = new string[] { "Sample/测试矩形构造函数.zyy" };
                //args = new string[] { "Sample/测试强制转换.zyy" };
                //args = new string[] { "Sample/测试属性默认值.zyy" };
                //args = new string[] { "Sample/测试Lambda1.zyy" };
                //args = new string[] { "Sample/测试打印乘法.zyy" };
                //args = new string[] { "Sample/测试过程A.zyy" };

                //args = new string[] { "Sample/ZExcel开发包测试/test1.zyy" };
                args = new string[] { "Sample/ZExcel开发包测试/test2.zyy" };

                //args = new string[] { "Sample/Z标准包测试/操作系统/测试定时器.zyy" };
                //args = new string[] { "Sample/Z标准包测试/操作系统/测试剪贴板.zyy" };
                //args = new string[] { "Sample/Z标准包测试/操作系统/测试进程.zyy" };
                //args = new string[] { "Sample/Z标准包测试/操作系统/测试进程窗口.zyy" };
                //args = new string[] { "Sample/Z标准包测试/操作系统/测试快捷方式.zyy" };
                //args = new string[] { "Sample/Z标准包测试/操作系统/测试设置桌面背景图.zyy" };
                //args = new string[] { "Sample/Z标准包测试/操作系统/测试注册表操作.zyy" };

                //args = new string[] { "Sample/Z标准包测试/绘图/测试字体.zyy" };
                //args = new string[] { "Sample/Z标准包测试/设备/测试驱动器.zyy" };
                //args = new string[] { "Sample/Z标准包测试/文件系统/测试文件创建.zyy" };
                //args = new string[] { "Sample/Z标准包测试/桌面控件/测试窗体.zyy" };

                //args = new string[] { "Sample/小飞机游戏/小飞机游戏.zxm" };
            }
              
            CompileCmdModel cmdm = ParseArgs(args);
            if(cmdm==null)
            {
                Console.WriteLine("命令行参数不正确");
                return;
            }
            Compile(cmdm);
            if(cmdm.IsReadKey)
            {
                Console.ReadKey();
            }
        }

        static ProjectCompileResult Compile(CompileCmdModel model)
        {
            CompileMessageCollection MessageCollection = new CompileMessageCollection();

            ProjectCompileResult result = null;
            if(model.IsCompileProject)
            {
                result = CompileFile(model.SrcFile, MessageCollection);
            }
            else
            {
                result = CompileProject(model.SrcFile, MessageCollection);
            }
            if(model.IsShowError)
            {
                if (MessageCollection.HasError())
                {
                    ShowErrors(result);
                }
            }
            if(model.IsRun)
            {
                if (MessageCollection.HasError() == false)
                {
                    Run(result);
                }
            }
            return result;
        }

        static CompileCmdModel ParseArgs(string[] args)
        {
            if(args.Length==1)
            {
                CompileCmdModel model = new CompileCmdModel();
                model.SrcFile = args[0];
                model.IsCompileProject = model.SrcFile.ToLower().EndsWith(Const.FileExt);
                model.IsRun = true;
                model.IsShowError = true;
                model.IsReadKey = false;
                return model;
            }
            else if(args.Length==5)
            {
                CompileCmdModel model = new CompileCmdModel();
                model.SrcFile = args[0];
                model.IsCompileProject =(args[1]=="1"|| args[1].ToLower()=="true");
                model.IsRun = (args[2] == "1" || args[1].ToLower() == "true");
                model.IsShowError = (args[3] == "1" || args[1].ToLower() == "true");
                model.IsReadKey = (args[4] == "1" || args[1].ToLower() == "true");
                return model;
            }
            else
            {
                return null;
            }
        }

        static ProjectCompileResult CompileFile(string srcFile, CompileMessageCollection MessageCollection)
        {
            FileCompiler compiler = new FileCompiler();
            ProjectCompileResult result = compiler.Compile(srcFile, MessageCollection);
            return result;
        }

        static ProjectCompileResult CompileProject(string srcFile, CompileMessageCollection messageCollection)
        {
            ProjectFileCompiler compiler = new ProjectFileCompiler(srcFile, messageCollection);
            ProjectCompileResult result = compiler.Compile();
            return result;
        }

        public static void ShowErrors(ProjectCompileResult compileResult)
        {
            StringBuilder buffBuilder = new StringBuilder();
            //buffBuilder.AppendFormat("文件'{0}'有以下错误:\n", srcFile);
            foreach (CompileMessage compileMessage in compileResult.MessageCollection.Errors)
            {
                if (compileMessage.Line > 0 || compileMessage.Col > 0)
                {
                    buffBuilder.AppendFormat(" {2} 第{0}行,第{1}列", compileMessage.Line, compileMessage.Col, compileMessage.Key.ToString());
                }
                buffBuilder.AppendFormat("错误:{0}\n", compileMessage.Content);
            }
            Console.WriteLine(buffBuilder.ToString());
            Console.ReadKey();
        }

        private static Type CompiledMainType = null;
        public static void Run(ProjectCompileResult result)
        {
            __CompileResult = result;
            if (result.EntrtyZType != null)
            {
                CompiledMainType = result.EntrtyZType.SharpType;
                Thread newThread = new Thread(new ThreadStart(RunMainTypeEntry));
                newThread.SetApartmentState(ApartmentState.STA);
                newThread.Start();
            }
        }

        private static ProjectCompileResult __CompileResult;
        private static void RunMainTypeEntry() //string exeDirectory, string exePath)
        {
            string exePath = __CompileResult.BinaryFilePath;
            string exeDirectory = (new FileInfo(exePath)).DirectoryName;
            var tempEnv = Environment.CurrentDirectory;
            Environment.CurrentDirectory = exeDirectory;
            Invoker.Call(CompiledMainType, "启动");
            Environment.CurrentDirectory = tempEnv;

            //AppDomainSetup ads = new AppDomainSetup();
            //ads.LoaderOptimization = LoaderOptimization.MultiDomainHost;
            //ads.ApplicationBase = exeDirectory;
            ////ads.PrivateBinPath = AppDomain.CurrentDomain.BaseDirectory;
            //AppDomain domain = AppDomain.CreateDomain("RunDomain", null, ads);

            ////domain.SetData("PRIVATE_BINPATH", AppDomain.CurrentDomain.BaseDirectory);
            ////domain.SetData("BINPATH_PROBE_ONLY", AppDomain.CurrentDomain.BaseDirectory);
            ////var m = typeof(AppDomainSetup).GetMethod("UpdateContextProperty", BindingFlags.NonPublic | BindingFlags.Static);
            ////var funsion = typeof(AppDomain).GetMethod("GetFusionContext", BindingFlags.NonPublic | BindingFlags.Instance);
            ////m.Invoke(null, new object[] { funsion.Invoke(AppDomain.CurrentDomain, null), "PRIVATE_BINPATH", AppDomain.CurrentDomain.BaseDirectory });
            ////try
            ////{
            ////    domain.AssemblyResolve += ExeDomain_AssemblyResolve;
            ////}catch(Exception ex)
            ////{
            ////    Console.WriteLine(ex.Message);
            ////}
            ////AppDomain.CurrentDomain.AssemblyResolve += ExeDomain_AssemblyResolve;  
            //var tempEnv = Environment.CurrentDirectory;
            //Environment.CurrentDirectory = exeDirectory;
            //domain.ExecuteAssembly(exePath);
            //AppDomain.Unload(domain);
            //Environment.CurrentDirectory = tempEnv;
            
        }

        //private static Assembly ExeDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        //{
        //    string path = AppDomain.CurrentDomain.BaseDirectory;// System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Libs\");
        //    path = System.IO.Path.Combine(path, args.Name.Split(',')[0]);
        //    path = String.Format(@"{0}.dll", path);
        //    return System.Reflection.Assembly.LoadFrom(path);
        //}  
          
    }
}
