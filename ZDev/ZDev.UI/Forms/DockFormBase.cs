using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZDev.Forms
{
    public class DockFormBase : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public WeifenLuo.WinFormsUI.Docking.DockPanel ParentDockPanel { get; set; }
        public MainForm MainWindow { get; set; }
    }
}
