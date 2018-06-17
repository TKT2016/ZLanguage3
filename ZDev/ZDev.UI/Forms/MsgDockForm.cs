using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZCompileCore.Reports;

namespace ZDev.Forms
{
    public partial class MsgDockForm : DockFormBase
    {
        public MsgDockForm()
        {
            InitializeComponent();
        }

        public void ResetConsoles()
        {
            this.padConsole.Text = string.Empty;
            this.errorlistView.Items.Clear();
        }

        public void ShowErrors(ProjectCompileResult result)
        {      
            if (result.MessageCollection.HasError())
            {
                List<CompileMessage> errors = result.MessageCollection.Errors;
                foreach (var error in errors)
                {
                    ListViewItem item = new ListViewItem();
                    //item.SubItems.Add("错误");
                    string src = (error.Key as CompileMessageSrcKey).SrcFileName;
                    item.SubItems.Add(src);
                    item.SubItems.Add(error.Line.ToString());
                    item.SubItems.Add(error.Col.ToString());
                    item.SubItems.Add(error.Content);
                    item.ImageIndex = 0;
                    errorlistView.Items.Add(item);
                }
                this.msgTab.SelectedTab = this.errorTabPage;
            }

            if (result.MessageCollection.HasWarning())
            {
                List<CompileMessage> warnings = result.MessageCollection.Warnings;
                foreach (var warning in warnings)
                {
                    ListViewItem item = new ListViewItem();
                    //item.SubItems.Add("警告");
                    string src = (warning.Key as CompileMessageSrcKey).SrcFileName;
                    item.SubItems.Add(src);
                    item.SubItems.Add(warning.Line.ToString());
                    item.SubItems.Add(warning.Col.ToString());
                    item.SubItems.Add(warning.Content);
                    item.ImageIndex = 1;
                    errorlistView.Items.Add(item);
                }
                //foreach (var str in result.Warnings)
                //{
                //    outputConsole.Text += str.Text + "\r\n";
                //}
                this.msgTab.SelectedTab = this.errorTabPage;
            }
        }
    }
}
