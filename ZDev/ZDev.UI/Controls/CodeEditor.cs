using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZDev.Forms;
using ZDev.Util;

namespace ZDev.Controls
{
    public class CodeEditor : Scintilla
    {
        public const int StyleCurrentLine = 201;
        //public const int NUM = 8;
        public EditorDockForm DockForm { get; set; }

        /// <summary>
        /// 文档修改状态(-1:初始,0:未改变,1:已经改变)
        /// </summary>
        public int TextChangedStatus { get; set; }
        //public FileInfo CurrentFile { get; set; }

        public CodeEditor()
        {
            this.Margins[0].Width = 20;//显示行号
            //this.Styles[CodeEditor.StyleCurrentLine].BackColor = Color.FromArgb(0x29,0x39,0x55);
            //this.HotspotClick += new System.EventHandler<ScintillaNET.HotspotClickEventArgs>(this.editorClick);
            this.Click += new System.EventHandler(this.editorClick);
            this.TextChanged += new System.EventHandler(this.editorTextChanged);
            this.KeyDown += new KeyEventHandler(this.EditorKeyDown);
            InitCurrentLineStyle();
            TextChangedStatus = -1;
        }

        public FileInfo CurrentFile { get; set; }
        public bool FileExists
        {
            get
            {
                return CurrentFile != null && CurrentFile.Exists;
            }
        }
        
        public virtual void GotoPostion(int position)
        {
            this.GotoPosition(position);
            HighLightCurrentLine(position);
        }

        private void EditorKeyDown(object sender, KeyEventArgs e)
        {
            if(KeyDownAction!=null)
            {
                KeyDownAction(sender, e);
            }
        }

        public Action<object, KeyEventArgs> KeyDownAction{get;set;}

        private void editorTextChanged(object sender, EventArgs e)
        {
            if (TextChangedStatus == 0)
            {
                TextChangedStatus = 1;
                if (!this.DockForm.TabText.EndsWith(" *"))
                {
                    this.DockForm.TabText += " *";
                }
            }
            else
            {
                TextChangedStatus=0;
            }
        }

        private void editorClick(object sender,EventArgs e)
        {
            int position = this.AnchorPosition;//this.CurrentPosition
            HighLightCurrentLine(position);
        }

        public bool Save()
        {
            if (CurrentFile != null)
            {
                return Save(CurrentFile);
            }
            return false;
        }

        public bool Save(FileInfo file)
        {
            if (file != null)
            {
                IoUtility.SaveFile(CurrentFile.FullName, this.Text); //_textEditor.SaveFile(file.FullName);
                CurrentFile = file;
                TextChangedStatus = 0;
                //this.Text = Text;
                if (this.DockForm.TabText.EndsWith(" *"))
                {
                    string title = this.DockForm.TabText;
                    string newTitle = title.Substring(0, title.Length - 2);
                    this.DockForm.TabText = newTitle;
                }
                return true;
            }
            return false;
        }

        public virtual void OpenFile(FileInfo fi)
        {
            CurrentFile = fi;
        }

        public virtual void NewFile()
        {
            TextChangedStatus = 0;
        }

        void InitCurrentLineStyle()
        {
            var scintilla = this;
            // Update indicator appearance
            scintilla.Indicators[StyleCurrentLine].Style = IndicatorStyle.StraightBox;
            scintilla.Indicators[StyleCurrentLine].Under = true;
            scintilla.Indicators[StyleCurrentLine].ForeColor = Color.Red;
            //scintilla.Indicators[StyleCurrentLine].ForeColor = Color.FromArgb(0xF6D807);
            //scintilla.Styles[StyleCurrentLine].BackColor = Color.FromArgb(0xFF, 0xF2, 0x9D);
            //scintilla.Indicators[StyleCurrentLine].OutlineAlpha = 50;
            //scintilla.Indicators[StyleCurrentLine].Alpha = 30;
        }

        public virtual void HighLightCurrentLine(int position)
        {
            if (position < 0 || position >= this.TextLength) return;
            //this.StartStyling(position);
            //this.SetStyling(10, StyleCurrentLine);
            var scintilla = this;
            // Indicators 0-7 could be in use by a lexer
            // so we'll use indicator 8 to highlight words.
            //const int NUM = 8;
            // Remove all uses of our indicator
            scintilla.IndicatorCurrent = StyleCurrentLine;
            scintilla.IndicatorClearRange(0, scintilla.TextLength);

            // Search the document
            scintilla.TargetStart = 0;
            scintilla.TargetEnd = scintilla.TextLength;

            int line = this.LineFromPosition(position);

            int lineStart = this.Lines[line].Position;// GetLineStartPosition(position);
            int lineEnd = this.Lines[line].EndPosition;// GetLineEndPosition(position);
            scintilla.IndicatorFillRange(lineStart, lineEnd - lineStart);
        }
      
    }
}
