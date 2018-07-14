namespace ZDev.Forms
{
    partial class ProcDockForm
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
            this.procTreeView = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // procTreeView
            // 
            this.procTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.procTreeView.Location = new System.Drawing.Point(0, 0);
            this.procTreeView.Name = "procTreeView";
            this.procTreeView.Size = new System.Drawing.Size(284, 262);
            this.procTreeView.TabIndex = 0;
            this.procTreeView.DoubleClick += new System.EventHandler(this.procTreeView_DoubleClick);
            // 
            // ProcDockForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.procTreeView);
            this.Name = "ProcDockForm";
            this.Text = "过程属性";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView procTreeView;
    }
}