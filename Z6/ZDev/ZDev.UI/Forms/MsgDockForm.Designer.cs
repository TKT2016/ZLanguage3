using ZDev.Controls;
using ZDev.UI.Controls;

namespace ZDev.Forms
{
    partial class MsgDockForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MsgDockForm));
            this.msgTab = new System.Windows.Forms.TabControl();
            this.errorTabPage = new System.Windows.Forms.TabPage();
            this.consoleabPage = new System.Windows.Forms.TabPage();
            this.padConsole = new ConsoleBox();
            this.errorlistView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.errorImageList = new System.Windows.Forms.ImageList(this.components);
            this.msgTab.SuspendLayout();
            this.errorTabPage.SuspendLayout();
            this.consoleabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // msgTab
            // 
            this.msgTab.Controls.Add(this.errorTabPage);
            this.msgTab.Controls.Add(this.consoleabPage);
            this.msgTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.msgTab.Location = new System.Drawing.Point(0, 0);
            this.msgTab.Name = "msgTab";
            this.msgTab.SelectedIndex = 0;
            this.msgTab.Size = new System.Drawing.Size(799, 213);
            this.msgTab.TabIndex = 0;
            // 
            // errorTabPage
            // 
            this.errorTabPage.Controls.Add(this.errorlistView);
            this.errorTabPage.Location = new System.Drawing.Point(4, 22);
            this.errorTabPage.Name = "errorTabPage";
            this.errorTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.errorTabPage.Size = new System.Drawing.Size(791, 187);
            this.errorTabPage.TabIndex = 0;
            this.errorTabPage.Text = "错误";
            this.errorTabPage.UseVisualStyleBackColor = true;
            // 
            // consoleabPage
            // 
            this.consoleabPage.Controls.Add(this.padConsole);
            this.consoleabPage.Location = new System.Drawing.Point(4, 22);
            this.consoleabPage.Name = "consoleabPage";
            this.consoleabPage.Padding = new System.Windows.Forms.Padding(3);
            this.consoleabPage.Size = new System.Drawing.Size(791, 187);
            this.consoleabPage.TabIndex = 1;
            this.consoleabPage.Text = "控制台";
            this.consoleabPage.UseVisualStyleBackColor = true;
            // 
            // padConsole
            // 
            this.padConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.padConsole.InputStream = null;
            this.padConsole.Location = new System.Drawing.Point(3, 3);
            this.padConsole.Multiline = true;
            this.padConsole.Name = "padConsole";
            this.padConsole.OutputStream = null;
            this.padConsole.Size = new System.Drawing.Size(785, 181);
            this.padConsole.TabIndex = 0;
            // 
            // errorlistView
            // 
            this.errorlistView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.errorlistView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.errorlistView.GridLines = true;
            this.errorlistView.Location = new System.Drawing.Point(3, 3);
            this.errorlistView.Name = "errorlistView";
            this.errorlistView.Size = new System.Drawing.Size(785, 181);
            this.errorlistView.TabIndex = 0;
            this.errorlistView.UseCompatibleStateImageBehavior = false;
            this.errorlistView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "错误";
            this.columnHeader1.Width = 50;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "文件";
            this.columnHeader2.Width = 132;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "行";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "列";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "描述";
            this.columnHeader5.Width = 490;
            // 
            // errorImageList
            // 
            this.errorImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("errorImageList.ImageStream")));
            this.errorImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.errorImageList.Images.SetKeyName(0, "Error.png");
            this.errorImageList.Images.SetKeyName(1, "Warning.png");
            // 
            // MsgDockForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(799, 213);
            this.Controls.Add(this.msgTab);
            this.Name = "MsgDockForm";
            this.msgTab.ResumeLayout(false);
            this.errorTabPage.ResumeLayout(false);
            this.consoleabPage.ResumeLayout(false);
            this.consoleabPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl msgTab;
        private System.Windows.Forms.TabPage errorTabPage;
        private System.Windows.Forms.TabPage consoleabPage;
        private ConsoleBox padConsole;
        private System.Windows.Forms.ListView errorlistView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ImageList errorImageList;
    }
}