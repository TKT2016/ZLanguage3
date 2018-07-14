using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZDev.Lexers;

namespace ZDev.Controls
{
    public class TKTCodeEditor : CodeEditor
    {
        private ZyyLexer tktLexer = new ZyyLexer();

        public TKTCodeEditor()
        {
            this.StyleNeeded += new System.EventHandler<ScintillaNET.StyleNeededEventArgs>(this.codeEditor_StyleNeeded);
        }

        public override void OpenFile(FileInfo fi)
        {
            CurrentFile = fi;
            string fileName = CurrentFile.FullName;
            //Text = Path.GetFileName(fileName);

            //this.TabText = Path.GetFileName(fileName);
            this.Text = File.ReadAllText(fileName);

            this.LexerLanguage = fi.Extension.ToLower();

            this.StyleResetDefault();
            this.Styles[Style.Default].Font = "Consolas";
            this.Styles[Style.Default].Size = 10;
            this.StyleClearAll();

            this.Styles[ZyyLexer.StyleDefault].ForeColor = Color.Black;
            this.Styles[ZyyLexer.StyleKeyword].ForeColor = Color.Blue;
            this.Styles[ZyyLexer.StyleKeyword].Bold = true;
            this.Styles[ZyyLexer.StyleKeyword].Font = "微软雅黑";

            this.Styles[ZyyLexer.StyleIdentifier].ForeColor = Color.Teal;
            this.Styles[ZyyLexer.StyleNumber].ForeColor = Color.Purple;
            this.Styles[ZyyLexer.StyleString].ForeColor = Color.Green;
            this.Styles[ZyyLexer.StyleCommentLine].ForeColor = Color.Gray;
            this.Styles[ZyyLexer.StyleCommentMutil].ForeColor = Color.Gray;

            this.Lexer = Lexer.Container;

        }

        private void codeEditor_StyleNeeded(object sender, StyleNeededEventArgs e)
        {
            var startPos = this.GetEndStyled();
            var endPos = e.Position;
            tktLexer.Style(this, startPos, endPos);
        }
    }
}
