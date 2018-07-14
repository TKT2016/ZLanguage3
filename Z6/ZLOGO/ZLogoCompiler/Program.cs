using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Reports;
using System.Windows.Forms;

namespace ZLogoCompiler
{
    class Program
    {
        static LogoCompiler compiler = new LogoCompiler();

        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //string src = "";
            //src = "例子/第1个ZLOGO程序.zlogo";
            //src = "例子/重复绘图.zlogo";
            //src = "例子/测试画笔颜色.zlogo";

            //if(args.Length>1)
            //{
            //    src = args[0];
            //}
            //ProjectCompileResult result = compiler.Compile(src);
            //if (result.HasError() == false)
            //{
            //    compiler.Run(result);
            //}
            //else
            //{
            //    ShowErrors(result);
            //    Console.ReadKey();
            //}   
        }

        //public static void ShowErrors(ProjectCompileResult compileResult)
        //{
        //    StringBuilder buffBuilder = new StringBuilder();
        //    //buffBuilder.AppendFormat("文件'{0}'有以下错误:\n", srcFile);
        //    foreach (CompileMessage compileMessage in compileResult.Errors.ValuesToList())
        //    {
        //        if (compileMessage.Line > 0 || compileMessage.Col > 0)
        //        {
        //            buffBuilder.AppendFormat(" {2} 第{0}行,第{1}列", compileMessage.Line, compileMessage.Col,compileMessage.SourceFileInfo.ZFileName);
        //        }
        //        buffBuilder.AppendFormat("错误:{0}\n", compileMessage.Text);
        //    }
        //    Console.WriteLine(buffBuilder.ToString());
        //    Console.ReadKey();
        //}
    }
}
