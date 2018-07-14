using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ZDev.RunExe
{
    class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0) return;
            string argsCode = string.Join(" ", args);
            string cmd = args[0];
            if (cmd == "runexe")
            {
                try
                {
                    RunExe(args[1]);
                }
                catch (RunCmdExecption ex)
                {
                    Console.WriteLine("argsCode:" + argsCode);
                    Console.WriteLine("args length:" + args.Length);
                    Console.WriteLine("args[0]:" + args[0]);
                    Console.WriteLine("args[1]:" + args[1]);
                }
            }
        }

        private static LocalLoader loader = null;
        private static void Load(string dll)
        {
            string dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dll);
            loader.LoadAssembly(dll);
        }

        public static void RunExe(string exearg)
        {
            string exe = exearg;
            try
            {
                AppDomainSetup setup = new AppDomainSetup();
                setup.ApplicationName = "RunExe";
                setup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
                setup.PrivateBinPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "private");
                setup.CachePath = setup.ApplicationBase;
                setup.ShadowCopyFiles = "true";
                setup.ShadowCopyDirectories = setup.ApplicationBase;

                AppDomain domain = AppDomain.CreateDomain("RunExeDomain", null, setup);

                if (exearg[0] == '"' || exearg[0] == '\'')
                    exe = exearg.Substring(1, exearg.Length - 2);

                loader = new LocalLoader(domain);
                Load("ZLangRT.dll");
                Load("Z语言系统.dll");
                Load("Z标准包.dll");
                domain.ExecuteAssembly(exe);
                loader.Unload();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("exearg:" + exearg);
                Console.WriteLine("exe:" + exe);
                throw new RunCmdExecption(ex.Message);
            }
        }

        public class RunCmdExecption : Exception
        {
            public RunCmdExecption(string Message)
                : base(Message)
            {

            }
        }
    }
}