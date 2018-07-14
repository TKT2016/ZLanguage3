using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ZLogoIDE
{
    public partial class CompileMsgForm : Form
    {
        public CompileMsgForm()
        {
            InitializeComponent();
        }

        private void CompileMsgForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //this.WindowState = FormWindowState.Minimized;
            //e.Cancel = true;  
        }

        public void ShowMessage(string msg)
        {
            this.richTextBox.Text = msg;
        }

        protected override void WndProc(ref   Message m)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_CLOSE = 0xF060;
            if (m.Msg == WM_SYSCOMMAND && (int)m.WParam == SC_CLOSE)
            {
                //捕捉关闭窗体消息      
                //   User   clicked   close   button      
                this.Hide();
                return;
            }
            base.WndProc(ref   m);
        }
    }
}
