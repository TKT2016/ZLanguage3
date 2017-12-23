using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCompileDesc.Descriptions
{
    public class ZMethodCall  : ACall,IParts
    {
        public int GetPartCount() { return _parts.Count; }
        public object[] GetParts() { return _parts.Values.ToArray(); }
        public ZArgCall[] Args { get { return _zlargs.ToArray(); } }

        Dictionary<int, object> _parts = new Dictionary<int, object>();
        List<string> _texts = new List<string>();
        List<ZArgCall> _zlargs = new List<ZArgCall>();
        List<ZBracketCall> _zlbrackets = new List<ZBracketCall>();
        private int index = 0;

        public void Add(string str )
        {
            _parts.Add(index, str);
            _texts.Add(str);
            index++;
        }

        public void Add(ZBracketCall zbracket)
        {
            _parts.Add(index, zbracket);
            _zlbrackets.Add(zbracket);
            var zargs = zbracket.Args;
            _zlargs.AddRange(zargs);
            index++;
        }

        public override string ToZCode() 
        {
            StringBuilder buff = new StringBuilder();
            for (int i = 0; i < index;i++ )
            {
                object item = _parts[i];
                if(item is string)
                {
                    buff.Append((string)item);
                }
                else if (item is ZBracketCall)
                {
                    string zcode = ((ZBracketCall)item).ToZCode();
                    buff.Append(zcode);
                }
                else
                {
                    throw new ZLibRTException();
                }
            }
            return buff.ToString();
        }

        public object GetPart(int i)
        {
            return _parts[i];
        }

        public ZMethodCall CreateTail( )
        {
            ZMethodCall tailDesc = new ZMethodCall();
            var parts = this.GetParts();
            for (int i = 1; i < this.GetPartCount(); i++)
            {
                object item = this.GetPart(i);
                if (item is string)
                {
                    tailDesc.Add(item as string);
                }
                else if (item is ZBracketCall)
                {
                    tailDesc.Add(item as ZBracketCall);
                }
                else
                {
                    throw new ZLibRTException();
                }
            }
            return tailDesc;
        }
    }
}
