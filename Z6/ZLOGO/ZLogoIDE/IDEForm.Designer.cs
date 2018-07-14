namespace ZLogoIDE
{
    partial class IDEForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.newStripButton = new System.Windows.Forms.ToolStripButton();
            this.openStripButton = new System.Windows.Forms.ToolStripButton();
            this.saveStripButton = new System.Windows.Forms.ToolStripButton();
            this.saveAsStripButton = new System.Windows.Forms.ToolStripButton();
            this.runStripButton = new System.Windows.Forms.ToolStripButton();
            this.aboutStripButton = new System.Windows.Forms.ToolStripButton();
            this.textBoxEditor = new System.Windows.Forms.RichTextBox();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newStripButton,
            this.openStripButton,
            this.saveStripButton,
            this.saveAsStripButton,
            this.runStripButton,
            this.aboutStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(784, 39);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // newStripButton
            // 
            this.newStripButton.Image = global::ZLogoIDE.Properties.Resources.add;
            this.newStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.newStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newStripButton.Name = "newStripButton";
            this.newStripButton.Size = new System.Drawing.Size(68, 36);
            this.newStripButton.Text = "新建";
            this.newStripButton.Click += new System.EventHandler(this.newStripButton_Click);
            // 
            // openStripButton
            // 
            this.openStripButton.Image = global::ZLogoIDE.Properties.Resources.open_folder;
            this.openStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.openStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openStripButton.Name = "openStripButton";
            this.openStripButton.Size = new System.Drawing.Size(68, 36);
            this.openStripButton.Text = "打开";
            this.openStripButton.Click += new System.EventHandler(this.openStripButton_Click);
            // 
            // saveStripButton
            // 
            this.saveStripButton.Image = global::ZLogoIDE.Properties.Resources.script_save;
            this.saveStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.saveStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveStripButton.Name = "saveStripButton";
            this.saveStripButton.Size = new System.Drawing.Size(68, 36);
            this.saveStripButton.Text = "保存";
            this.saveStripButton.Click += new System.EventHandler(this.saveStripButton_Click);
            // 
            // saveAsStripButton
            // 
            this.saveAsStripButton.Image = global::ZLogoIDE.Properties.Resources.document_save_as_5;
            this.saveAsStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.saveAsStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveAsStripButton.Name = "saveAsStripButton";
            this.saveAsStripButton.Size = new System.Drawing.Size(80, 36);
            this.saveAsStripButton.Text = "另存为";
            this.saveAsStripButton.Click += new System.EventHandler(this.saveAsStripButton_Click);
            // 
            // runStripButton
            // 
            this.runStripButton.Image = global::ZLogoIDE.Properties.Resources.clicknrun;
            this.runStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.runStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.runStripButton.Name = "runStripButton";
            this.runStripButton.Size = new System.Drawing.Size(68, 36);
            this.runStripButton.Text = "运行";
            this.runStripButton.Click += new System.EventHandler(this.runStripButton_Click);
            // 
            // aboutStripButton
            // 
            this.aboutStripButton.Image = global::ZLogoIDE.Properties.Resources.help_about_3;
            this.aboutStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.aboutStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.aboutStripButton.Name = "aboutStripButton";
            this.aboutStripButton.Size = new System.Drawing.Size(68, 36);
            this.aboutStripButton.Text = "关于";
            this.aboutStripButton.Click += new System.EventHandler(this.aboutStripButton_Click);
            // 
            // textBoxEditor
            // 
            this.textBoxEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxEditor.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxEditor.ForeColor = System.Drawing.Color.Navy;
            this.textBoxEditor.Location = new System.Drawing.Point(0, 39);
            this.textBoxEditor.Name = "textBoxEditor";
            this.textBoxEditor.Size = new System.Drawing.Size(784, 523);
            this.textBoxEditor.TabIndex = 1;
            this.textBoxEditor.Text = "";
            this.textBoxEditor.TextChanged += new System.EventHandler(this.textBoxEditor_TextChanged);
            // 
            // IDEForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.textBoxEditor);
            this.Controls.Add(this.toolStrip1);
            this.Name = "IDEForm";
            this.Text = "IDEForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.IDEForm_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton newStripButton;
        private System.Windows.Forms.ToolStripButton openStripButton;
        private System.Windows.Forms.ToolStripButton saveStripButton;
        private System.Windows.Forms.ToolStripButton saveAsStripButton;
        private System.Windows.Forms.ToolStripButton runStripButton;
        private System.Windows.Forms.RichTextBox textBoxEditor;
        private System.Windows.Forms.ToolStripButton aboutStripButton;
    }
}

