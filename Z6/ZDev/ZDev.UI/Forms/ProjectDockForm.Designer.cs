namespace ZDev.Forms
{
    partial class ProjectDockForm
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
            this.projTreeView = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // projTreeView
            // 
            this.projTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.projTreeView.Location = new System.Drawing.Point(0, 0);
            this.projTreeView.Name = "projTreeView";
            this.projTreeView.Size = new System.Drawing.Size(284, 461);
            this.projTreeView.TabIndex = 0;
            this.projTreeView.DoubleClick += new System.EventHandler(this.projTreeView_DoubleClick);
            // 
            // ProjectDockForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 461);
            this.Controls.Add(this.projTreeView);
            this.Name = "ProjectDockForm";
            this.Text = "项目文件";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView projTreeView;
    }
}