namespace ZLogoIDE
{
    partial class AboutForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.aboutFormSubmitButton = new System.Windows.Forms.Button();
            this.timerGetFocus = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(102, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 12);
            this.label1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ZLogoIDE.Properties.Resources.animals_cow;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(84, 82);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // aboutFormSubmitButton
            // 
            this.aboutFormSubmitButton.Location = new System.Drawing.Point(327, 81);
            this.aboutFormSubmitButton.Name = "aboutFormSubmitButton";
            this.aboutFormSubmitButton.Size = new System.Drawing.Size(75, 23);
            this.aboutFormSubmitButton.TabIndex = 2;
            this.aboutFormSubmitButton.Text = "确定";
            this.aboutFormSubmitButton.UseVisualStyleBackColor = true;
            this.aboutFormSubmitButton.Click += new System.EventHandler(this.aboutFormSubmitButton_Click);
            // 
            // timerGetFocus
            // 
            this.timerGetFocus.Tick += new System.EventHandler(this.timerGetFocus_Tick);
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 115);
            this.Controls.Add(this.aboutFormSubmitButton);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "关于ZLOGO小海龟儿童编程";
            this.Deactivate += new System.EventHandler(this.aboutForm_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.aboutForm_FormClosing);
            this.Load += new System.EventHandler(this.aboutForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button aboutFormSubmitButton;
        private System.Windows.Forms.Timer timerGetFocus;
    }
}