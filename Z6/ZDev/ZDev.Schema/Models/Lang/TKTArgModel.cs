using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZDev.Schema.Models.Lang
{
    public class TKTArgModel : TKTModelBase
    {
        public string ArgType { get; set; }
        public string ArgName { get; set; }

        public override string ToString()
        {
            return string.Format("{0}:{1}", ArgType, ArgName);
        }
    }
}
