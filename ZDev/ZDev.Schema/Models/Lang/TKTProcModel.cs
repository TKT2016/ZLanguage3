using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZDev.Schema.Models.Lang
{
    public class TKTProcModel : TKTModelBase
    {
        public List<object> Elements { get; private set; }
        public string RetType { get; set; }

        public TKTProcModel()
        {
            Elements = new List<object>();
        }

        public bool IsContruct()
        {
            if(Elements.Count==1)
            {
                if (Elements[0] is TKTArgBracket)
                    return true;
            }
            return false;
        }

        public bool IsContruct(string typeName)
        {
            if (IsContruct()) return true;
            if (Elements.Count == 2)
            {
                if (!(Elements[0] is string))
                    return false;

                if (!(Elements[1] is TKTArgBracket))
                    return false;

                if ((Elements[0] as string)!= typeName)
                    return false;

                return true;
            }
            return false;
        }

        public TKTConstructionModel ToContruct()
        {
            if (Elements.Count == 1)
            {
                TKTConstructionModel model = new TKTConstructionModel();
                model.Args = (Elements[0] as TKTArgBracket).Args;
                model.Postion = this.Postion;
                return model;
            }
            else if (Elements.Count == 1)
            {
                TKTConstructionModel model = new TKTConstructionModel();
                model.Args = (Elements[1] as TKTArgBracket).Args;
                model.Postion = this.Postion;
                return model;
            }
            return null;
        }

        public override string ToString()
        {
            List<string> buff = new List<string>();
            //buff.Add("(");
            for (int i = 0; i < Elements.Count; i++)
            {
                object e = Elements[i];
                if(e is TKTArgBracket)
                {
                    buff.Add((e as TKTArgBracket).ToString());
                }
                else
                {
                    buff.Add(e.ToString());
                }
            }
            //buff.Add(")");
            string code = string.Join("", buff);
            return code;
        }
    }
}
