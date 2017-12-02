using ScintillaNET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZDev.Lexers;
using ZDev.Controls;
using ZDev.UI.Compilers;

namespace ZDev.Forms
{
    public class EditorDockForm : DockFormBase
    {
        public EditorDockForm()
        {
            //InitializeComponent();
            //setEditor();
            //OpenFile(fi);
        }

        public CodeEditor Editor { get; private set; }

        public void NewFile(int i )
        {
            TKTCodeEditor tktEditor = new TKTCodeEditor();
            Editor = tktEditor;
            this.TabText = "新建" + i + "." + ZDevCompiler.ZYYExt;
            InitializeComponent();
            Editor.DockForm = this;
            //Editor.OpenFile(fi);
        }

        public void OpenFile(FileInfo fi)
        {
            string fileName = fi.FullName;
            string ext = fi.Extension.ToLower();
            if(ext==ZDevCompiler.ZYYExt)
            {
                TKTCodeEditor tktEditor = new TKTCodeEditor();
                Editor = tktEditor;
            }
            else if (ext == ZDevCompiler.ZXMExt)
            {
                TKTXMCodeEditor tktxmEditor = new TKTXMCodeEditor();
                Editor = tktxmEditor;
            }
            if(Editor==null)
            {
                TextCodeEditor txtEditor = new TextCodeEditor();
                Editor = txtEditor;
            }
            this.TabText = Path.GetFileName(fileName);
            InitializeComponent();
            Editor.DockForm = this;
            Editor.OpenFile(fi);

        }

        #region Windows Form Designer generated code
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
        
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            //this.codeEditor = new ScintillaNET.Scintilla();
            this.SuspendLayout();
            // 
            // codeEditor
            // 
            this.Editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Editor.Location = new System.Drawing.Point(0, 0);
            //this.CodeEditor.Name = "codeEditor";
            this.Editor.Size = new System.Drawing.Size(284, 262);
            this.Editor.TabIndex = 0;
             
            //this.codeEditor.StyleNeeded += new System.EventHandler<ScintillaNET.StyleNeededEventArgs>(this.codeEditor_StyleNeeded);
            // 
            // DocEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.Editor);
            this.Name = "EditorDockForm";
            //this.Text = "DocEditor";
            this.ResumeLayout(false);

        }

        #endregion

    }
}
