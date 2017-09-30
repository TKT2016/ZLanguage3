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
            if (result.HasError())
            {
                List<CompileMessage> errors = result.Errors.ValuesToList();
                foreach (var error in errors)
                {
                    ListViewItem item = new ListViewItem();
                    //item.SubItems.Add("错误");
                    item.SubItems.Add(error.SourceFileInfo.ZFileName);
                    item.SubItems.Add(error.Line.ToString());
                    item.SubItems.Add(error.Col.ToString());
                    item.SubItems.Add(error.Text);
                    item.ImageIndex = 0;
                    errorlistView.Items.Add(item);
                }
                this.msgTab.SelectedTab = this.errorTabPage;
            }

            if (result.HasWarning())
            {
                List<CompileMessage> warnings = result.Warnings.ValuesToList();
                foreach (var warning in warnings)
                {
                    ListViewItem item = new ListViewItem();
                    //item.SubItems.Add("警告");
                    item.SubItems.Add(warning.SourceFileInfo.ZFileName);
                    item.SubItems.Add(warning.Line.ToString());
                    item.SubItems.Add(warning.Col.ToString());
                    item.SubItems.Add(warning.Text);
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
