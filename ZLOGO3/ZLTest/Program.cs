using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ZLTest
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            //Application.Run(new ZWindow1());
            int t = 1;
            /*
            using (var game = new Game())
            {
                game.Run(30.0, 0.0);
            }*/
            if (t == 1)
            {
                using (var game = new LogoTest1())
                {
                    game.Run();
                }
            }
            if (t ==2)
            {
                using (var game = new ZWindow1())
                {
                    game.Run(30.0, 0.0);
                }
            }
        }
    }
}
