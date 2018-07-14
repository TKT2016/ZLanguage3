using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ZDev.UI.Forms
{
    public class ConsoleForm:Form
    {
        private Controls.ConsoleBox consoleBox1;
        //private Process CurrentProcess;

        public ConsoleForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.consoleBox1 = new ZDev.UI.Controls.ConsoleBox();
            this.SuspendLayout();
            // 
            // consoleBox1
            // 
            this.consoleBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.consoleBox1.InputStream = null;
            this.consoleBox1.Location = new System.Drawing.Point(0, 0);
            this.consoleBox1.Multiline = true;
            this.consoleBox1.Name = "consoleBox1";
            this.consoleBox1.OutputStream = null;
            this.consoleBox1.Size = new System.Drawing.Size(584, 362);
            this.consoleBox1.TabIndex = 0;
            // 
            // ConsoleForm
            // 
            this.ClientSize = new System.Drawing.Size(584, 362);
            this.Controls.Add(this.consoleBox1);
            this.Name = "ConsoleForm";
            //this.Activated += new System.EventHandler(this.ConsoleForm_Activated);
            this.Shown += new System.EventHandler(this.ConsoleForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        //private bool isRun = false;
        public Action RunAction { get; set; }

        private void Run()
        {
            //isRun = true;
            try
            {
                if (RunAction!=null)
                    RunAction();
            }
            catch (Exception ex)
            {
                MessageBox.Show("程序运行错误", ex.Message);
            }
        }
        /*
        private void ConsoleForm_Activated(object sender, EventArgs e)
        {
            MessageBox.Show("ConsoleForm_Activated");
           // if (!isRun)
            {
                Run();
            }
        }*/

        private void ConsoleForm_Shown(object sender, EventArgs e)
        {
            Run();
        }
    }
}
