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
    public class TextCodeEditor:CodeEditor
    {
        public TextCodeEditor()
        {
            
        }

        public override void OpenFile(FileInfo fi)
        {
            CurrentFile = fi;
            string fileName = CurrentFile.FullName;
            //Text = Path.GetFileName(fileName);
            //this.TabText = Path.GetFileName(fileName);
            this.Text = File.ReadAllText(fileName);
            this.LexerLanguage = fi.Extension.ToLower();
            /*
            this.StyleResetDefault();
            this.Styles[Style.Default].Font = "Consolas";
            this.Styles[Style.Default].Size = 10;
            this.StyleClearAll();

            this.Styles[TKTLexer.StyleDefault].ForeColor = Color.Black;
            this.Styles[TKTLexer.StyleKeyword].ForeColor = Color.Blue;
            this.Styles[TKTLexer.StyleKeyword].Bold = true;
            this.Styles[TKTLexer.StyleKeyword].Font = "微软雅黑";

            this.Styles[TKTLexer.StyleIdentifier].ForeColor = Color.Teal;
            this.Styles[TKTLexer.StyleNumber].ForeColor = Color.Purple;
            this.Styles[TKTLexer.StyleString].ForeColor = Color.Green;
            this.Styles[TKTLexer.StyleCommentLine].ForeColor = Color.Gray;
            this.Styles[TKTLexer.StyleCommentMutil].ForeColor = Color.Gray;
            */
            this.Lexer = Lexer.Null;

        }
    }
}
