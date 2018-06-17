using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.Lex;

namespace ZDev.Schema.Models.Lang
{
    public class TKTContentModel : TKTModelBase
    {
        public string Content { get; set; }

        public TKTContentModel()
        {

        }

        public TKTContentModel(string content, CodePostion postion)
        {
            this.Content = content;
            this.Postion = postion;
        }

        public TKTContentModel(Token token)
        {
            this.Content = token.GetText();
            this.Postion = token.Postion;
        }
    }
}
