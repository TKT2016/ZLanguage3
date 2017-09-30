using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZLogoIDE
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            this.label1.Text = "ZLOGO \r\n版本 2.0\r\n作者 TKT2016\r\n版权所有 © 2016 。";
        }
        // 下载于www.mycodes.net
        private void aboutFormSubmitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timerGetFocus_Tick(object sender, EventArgs e)
        {
            this.Activate();
            this.Focus();
            timerGetFocus.Stop();
        }

        private void aboutForm_Deactivate(object sender, EventArgs e)
        {
            timerGetFocus.Start();  
        }

        private void aboutForm_Load(object sender, EventArgs e)
        {
            //IDEForm mf = (IDEForm)Application.OpenForms["mainForm"];
            //mf.Enabled = false;
        }

        private void aboutForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //IDEForm mf = (IDEForm)Application.OpenForms["mainForm"];
           // mf.Enabled = true;
        }
    }
}
