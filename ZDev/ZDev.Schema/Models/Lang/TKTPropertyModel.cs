using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZDev.Schema.Models.Lang
{
    public class TKTPropertyModel : TKTModelBase
    {
        public string PropertyName { get; set; }
        public string PropertyType { get; set; }
        public string DefaultValue { get; set; }

        public override string ToString()
        {
            return string.Format("{0}:{1}", PropertyName, PropertyType);
        }
    }
}
